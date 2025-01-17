﻿using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Indice.AspNetCore.Identity.Data.Models;
using Indice.Security;
using Microsoft.AspNetCore.Identity;

namespace Indice.AspNetCore.Identity
{
    /// <summary>Token provider that generates tokens for a user that has the role of a developer. Used to support scenarios for local development or test environments.</summary>
    /// <typeparam name="TUser">The type used to represent a user.</typeparam>
    public class DeveloperPhoneNumberTokenProvider<TUser> : ExtendedPhoneNumberTokenProvider<TUser> where TUser : User
    {
        /// <summary>Creates a new instance of <see cref="ExtendedPhoneNumberTokenProvider{TUser}"/>.</summary>
        /// <param name="rfc6238AuthenticationService">Time-Based One-Time Password Algorithm service.</param>
        public DeveloperPhoneNumberTokenProvider(Rfc6238AuthenticationService rfc6238AuthenticationService) : base(rfc6238AuthenticationService) { }

        /// <inheritdoc />
        public override async Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> userManager, TUser user) {
            var userClaims = await userManager.GetClaimsAsync(user);
            var developerTotpClaim = userClaims.FirstOrDefault(claim => claim.Type == BasicClaimTypes.DeveloperTotp);
            var hasDeveloperTotp = developerTotpClaim is not null && await userManager.IsInRoleAsync(user, BasicRoleNames.Developer);
            if (hasDeveloperTotp) {
                return true;
            }
            return await base.CanGenerateTwoFactorTokenAsync(userManager, user);
        }

        /// <inheritdoc />
        public override Task<string> GenerateAsync(string purpose, UserManager<TUser> userManager, TUser user) => Task.FromResult(string.Empty);

        /// <inheritdoc />
        public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> userManager, TUser user) {
            var userClaims = await userManager.GetClaimsAsync(user);
            var developerTotpClaim = userClaims.FirstOrDefault(claim => claim.Type == BasicClaimTypes.DeveloperTotp);
            if (developerTotpClaim?.Value == token) {
                return true;
            }
            return await base.ValidateAsync(purpose, token, userManager, user);
        }
    }
}
