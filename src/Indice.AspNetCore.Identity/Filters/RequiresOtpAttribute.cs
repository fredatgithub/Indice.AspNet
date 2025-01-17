﻿using System;
using System.Threading.Tasks;
using IdentityModel;
using Indice.AspNetCore.Identity.Api;
using Indice.AspNetCore.Identity.Data.Models;
using Indice.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Indice.AspNetCore.Identity.Filters
{
    /// <summary>An attribute that can be applied to action methods that require OTP verification.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequiresOtpAttribute : Attribute, IAsyncActionFilter
    {
        private IServiceProvider _serviceProvider;

        /// <summary>The default header value for capturing the TOTP code.</summary>
        public const string DEFAULT_HEADER_NAME = "X-TOTP";
        /// <summary>The name of the header that contains the TOTP code.</summary>
        public string HeaderName { get; set; } = DEFAULT_HEADER_NAME;

        /// <inheritdoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            var httpContext = context.HttpContext;
            var principal = httpContext.User;
            if (principal is null || !principal.Identity.IsAuthenticated) {
                var problemDetails = new ValidationProblemDetails {
                    Detail = "Principal is not present or not authenticated.",
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            // Check if user has an elevated access token and is already TOTP authenticated.
            var isOtpAuthenticated = principal.FindFirstValue<bool>(BasicClaimTypes.OtpAuthenticated) ?? false;
            if (isOtpAuthenticated) {
                await next();
                return;
            }
            var subject = principal.FindSubjectId();
            if (string.IsNullOrWhiteSpace(subject)) {
                var problemDetails = new ValidationProblemDetails {
                    Detail = "A subject does not exist in user claims.",
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            var phoneNumber = principal.FindFirst(JwtClaimTypes.PhoneNumber)?.Value;
            if (string.IsNullOrWhiteSpace(phoneNumber)) {
                var problemDetails = new ValidationProblemDetails {
                    Detail = "A phone number does not exist in user claims.",
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            _serviceProvider = httpContext.RequestServices;
            var totpServiceFactory = _serviceProvider.GetRequiredService<TotpServiceFactory>();
            var totpService = totpServiceFactory.Create<User>();
            var totp = httpContext.Request.Headers[HeaderName].ToString();
            var purpose = GetTotpPurpose(subject, phoneNumber);
            // No TOTP is present in the request, so will try to send one using the preferred delivery channel.
            if (string.IsNullOrWhiteSpace(totp)) {
                var deliveryChannel = _serviceProvider.GetRequiredService<IOptions<DeviceOptions>>().Value.DefaultTotpDeliveryChannel;
                await totpService.SendAsync(totp =>
                    totp.ToPrincipal(principal)
                        .WithMessage(GetTotpMessage())
                        .UsingDeliveryChannel(deliveryChannel)
                        .WithPurpose(purpose)
                );
                var problemDetails = new ValidationProblemDetails {
                    Detail = "An TOTP code is required to call this endpoint.",
                    Status = StatusCodes.Status400BadRequest
                };
                problemDetails.Extensions.Add("requiresOtp", true);
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            // If a TOTP exists in the request, then we need to verify it.
            var totpResult = await totpService.VerifyAsync(principal, totp, purpose);
            if (!totpResult.Success) {
                var problemDetails = new ValidationProblemDetails {
                    Detail = "The TOTP code could not be verified.",
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            await next();
        }

        /// <summary>Constructs the TOTP message to be sent.</summary>
        protected virtual string GetTotpMessage() {
            var messageDescriber = _serviceProvider.GetRequiredService<IdentityMessageDescriber>();
            return messageDescriber.RequiresOtpMessage();
        }

        /// <summary>Constructs the TOTP message to be sent.</summary>
        /// <param name="subject">The subject of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        protected virtual string GetTotpPurpose(string subject, string phoneNumber) {
            var purpose = $"{nameof(RequiresOtpAttribute)}:{subject}:{phoneNumber}";
            return purpose;
        }
    }
}
