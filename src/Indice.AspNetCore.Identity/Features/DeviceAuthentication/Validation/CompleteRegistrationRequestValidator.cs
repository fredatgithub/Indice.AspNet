﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Indice.AspNetCore.Identity.Data.Models;
using Indice.AspNetCore.Identity.DeviceAuthentication.Configuration;
using Indice.AspNetCore.Identity.DeviceAuthentication.Models;
using Indice.AspNetCore.Identity.DeviceAuthentication.Stores;
using Indice.Configuration;
using Indice.Extensions;
using Indice.Security;
using Indice.Services;
using Indice.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Indice.AspNetCore.Identity.DeviceAuthentication.Validation
{
    internal class CompleteRegistrationRequestValidator : RequestValidatorBase<CompleteRegistrationRequestValidationResult>
    {
        public CompleteRegistrationRequestValidator(
            IDeviceAuthenticationCodeChallengeStore codeChallengeStore,
            IClientStore clientStore,
            ILogger<CompleteRegistrationRequestValidator> logger,
            ISystemClock systemClock,
            ITokenValidator tokenValidator,
            TotpServiceFactory totpServiceFactory,
            ExtendedUserManager<User> userManager
        ) : base(clientStore, tokenValidator) {
            CodeChallengeStore = codeChallengeStore;
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            TotpServiceFactory = totpServiceFactory ?? throw new ArgumentNullException(nameof(totpServiceFactory));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public IDeviceAuthenticationCodeChallengeStore CodeChallengeStore { get; }
        public ILogger<CompleteRegistrationRequestValidator> Logger { get; }
        public ISystemClock SystemClock { get; }
        public TotpServiceFactory TotpServiceFactory { get; }
        public ExtendedUserManager<User> UserManager { get; }

        public override async Task<CompleteRegistrationRequestValidationResult> Validate(NameValueCollection parameters, string accessToken = null) {
            Logger.LogDebug($"[{nameof(CompleteRegistrationRequestValidator)}] Started trusted device registration request validation.");
            // The access token needs to be valid and have at least the OpenID scope.
            var tokenValidationResult = await TokenValidator.ValidateAccessTokenAsync(accessToken, IdentityServerConstants.StandardScopes.OpenId);
            if (tokenValidationResult.IsError) {
                return Error(tokenValidationResult.Error, "Provided access token is not valid.");
            }
            // The access token must have at lease a 'sub' and 'client_id' claim.
            var claimsToValidate = new[] {
                JwtClaimTypes.Subject,
                JwtClaimTypes.ClientId
            };
            foreach (var claim in claimsToValidate) {
                var claimValue = tokenValidationResult.Claims.SingleOrDefault(x => x.Type == claim)?.Value;
                if (string.IsNullOrWhiteSpace(claimValue)) {
                    return Error(OidcConstants.ProtectedResourceErrors.InvalidToken, $"Access token must contain the '{claim}' claim.");
                }
            }
            // Get 'code' parameter and retrieve it from the store.
            var code = parameters.Get(RegistrationRequestParameters.Code);
            if (string.IsNullOrWhiteSpace(code)) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, $"Parameter '{nameof(RegistrationRequestParameters.Code)}' is not specified.");
            }
            var authorizationCode = await CodeChallengeStore.GetDeviceAuthenticationCode(code);
            if (authorizationCode == null) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, "Authorization code is invalid.");
            }
            // Validate that the consumer specified all required parameters.
            var parametersToValidate = new List<string> {
                RegistrationRequestParameters.CodeSignature,
                RegistrationRequestParameters.CodeVerifier,
                RegistrationRequestParameters.DeviceId,
                RegistrationRequestParameters.DevicePlatform
            };
            if (authorizationCode.InteractionMode == InteractionMode.Fingerprint) {
                parametersToValidate.Add(RegistrationRequestParameters.PublicKey);
            }
            if (authorizationCode.InteractionMode == InteractionMode.Pin) {
                parametersToValidate.Add(RegistrationRequestParameters.Pin);
            }
            var otpAuthenticatedValue = tokenValidationResult.Claims.FirstOrDefault(x => x.Type == BasicClaimTypes.OtpAuthenticated)?.Value;
            var otpAuthenticated = !string.IsNullOrWhiteSpace(otpAuthenticatedValue) && bool.Parse(otpAuthenticatedValue);
            if (!otpAuthenticated) {
                parametersToValidate.Add(RegistrationRequestParameters.OtpCode);
            }
            foreach (var parameter in parametersToValidate) {
                var parameterValue = parameters.Get(parameter);
                if (string.IsNullOrWhiteSpace(parameterValue)) {
                    return Error(OidcConstants.TokenErrors.InvalidGrant, $"Parameter '{parameter}' is not specified.");
                }
            }
            var isValidPlatform = Enum.TryParse(typeof(DevicePlatform), parameters.Get(RegistrationRequestParameters.DevicePlatform), out var platform);
            if (!isValidPlatform) {
                return Error(OidcConstants.TokenErrors.InvalidRequest, $"Parameter '{nameof(RegistrationRequestParameters.DevicePlatform)}' does not have a valid value.");
            }
            // Load client and validate that it allows the 'password' flow.
            var client = await LoadClient(tokenValidationResult);
            if (client == null) {
                return Error(OidcConstants.AuthorizeErrors.UnauthorizedClient, $"Client is unknown or not enabled.");
            }
            if (!client.AllowedGrantTypes.Contains(CustomGrantTypes.DeviceAuthentication)) {
                return Error(OidcConstants.AuthorizeErrors.UnauthorizedClient, $"Trusted device flow is not enabled for this client.");
            }
            // Validate authorization code against code verifier given by the client.
            var codeVerifier = parameters.Get(RegistrationRequestParameters.CodeVerifier);
            var authorizationCodeValidationResult = await ValidateAuthorizationCode(code, authorizationCode, codeVerifier, parameters.Get(RegistrationRequestParameters.DeviceId), client);
            if (authorizationCodeValidationResult.IsError) {
                return Error(authorizationCodeValidationResult.Error, authorizationCodeValidationResult.ErrorDescription);
            }
            // Validate given public key against signature for fingerprint.
            string publicKey = null;
            if (authorizationCode.InteractionMode == InteractionMode.Fingerprint) {
                publicKey = parameters.Get(RegistrationRequestParameters.PublicKey);
                var codeSignature = parameters.Get(RegistrationRequestParameters.CodeSignature);
                var publicKeyValidationResult = ValidateSignature(publicKey, code, codeSignature);
                if (publicKeyValidationResult.IsError) {
                    return Error(publicKeyValidationResult.Error, publicKeyValidationResult.ErrorDescription);
                }
            }
            // Find requested scopes.
            var requestedScopes = tokenValidationResult.Claims.Where(claim => claim.Type == JwtClaimTypes.Scope).Select(claim => claim.Value).ToList();
            // Create principal from incoming access token excluding protocol claims.
            var claims = tokenValidationResult.Claims.Where(x => !Constants.ProtocolClaimsFilter.Contains(x.Type));
            var principal = Principal.Create("TrustedDevice", claims.ToArray());
            var userId = tokenValidationResult.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user is null) {
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken, "User does not exists.");
            }
            // Validate OTP code, if needed.
            if (!otpAuthenticated) {
                var totpResult = await TotpServiceFactory.Create<User>().VerifyAsync(user, parameters.Get(RegistrationRequestParameters.OtpCode), Constants.DeviceAuthenticationOtpPurpose(userId, authorizationCode.DeviceId));
                if (!totpResult.Success) {
                    return Error(totpResult.Error);
                }
            }
            // Finally return result.
            return new CompleteRegistrationRequestValidationResult {
                IsError = false,
                Client = client,
                DeviceId = authorizationCode.DeviceId,
                DeviceName = parameters.Get(RegistrationRequestParameters.DeviceName),
                DevicePlatform = (DevicePlatform)platform,
                InteractionMode = authorizationCode.InteractionMode,
                Pin = parameters.Get(RegistrationRequestParameters.Pin),
                Principal = principal,
                PublicKey = publicKey,
                RequestedScopes = requestedScopes,
                User = user
            };
        }

        private async Task<ValidationResult> ValidateAuthorizationCode(string code, DeviceAuthenticationCode authorizationCode, string codeVerifier, string deviceId, Client client) {
            // Validate that the current client is not trying to use an authorization code of a different client.
            if (authorizationCode.ClientId != client.ClientId) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, "Authorization code is invalid.");
            }
            // Validate that the current device is not trying to use an authorization code of a different device.
            if (authorizationCode.DeviceId != deviceId) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, "Authorization code is invalid.");
            }
            // Remove authorization code.
            await CodeChallengeStore.RemoveDeviceAuthenticationCode(code);
            // Validate code expiration.
            if (authorizationCode.CreationTime.HasExceeded(authorizationCode.Lifetime, SystemClock.UtcNow.UtcDateTime)) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, "Authorization code is invalid.");
            }
            if (authorizationCode.CreationTime.HasExceeded(client.AuthorizationCodeLifetime, SystemClock.UtcNow.UtcDateTime)) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, "Authorization code is invalid.");
            }
            if (authorizationCode.RequestedScopes == null || !authorizationCode.RequestedScopes.Any()) {
                return Error(OidcConstants.TokenErrors.InvalidGrant, "Authorization code is invalid.");
            }
            var proofKeyParametersValidationResult = ValidateAuthorizationCodeWithProofKeyParameters(codeVerifier, authorizationCode);
            if (proofKeyParametersValidationResult.IsError) {
                return Error(proofKeyParametersValidationResult.Error, proofKeyParametersValidationResult.ErrorDescription);
            }
            return Success();
        }
    }
}
