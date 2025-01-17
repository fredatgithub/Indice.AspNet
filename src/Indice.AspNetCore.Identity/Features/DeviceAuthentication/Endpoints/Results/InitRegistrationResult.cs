﻿using System;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Indice.AspNetCore.Identity.DeviceAuthentication.ResponseHandling;
using Microsoft.AspNetCore.Http;

namespace Indice.AspNetCore.Identity.DeviceAuthentication.Endpoints.Results
{
    internal class InitRegistrationResult : IEndpointResult
    {
        public InitRegistrationResult(InitRegistrationResponse response) {
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public InitRegistrationResponse Response { get; }

        public async Task ExecuteAsync(HttpContext context) {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.SetNoCache();
            var result = new DeviceAuthenticationResultDto(Response.Challenge);
            await context.Response.WriteJsonAsync(result);
        }
    }
}
