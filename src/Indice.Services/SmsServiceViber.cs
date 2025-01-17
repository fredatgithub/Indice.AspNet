﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Indice.Services
{
    /// <summary>Implementation of <see cref="ISmsService"/> using Viber's REST API.</summary>
    public class SmsServiceViber : ISmsService
    {
        /// <summary>The settings required to configure the service.</summary>
        protected SmsServiceSettings Settings { get; }
        /// <summary>The <see cref="System.Net.Http.HttpClient"/>.</summary>
        protected HttpClient HttpClient { get; }
        /// <summary>Represents a type used to perform logging.</summary>
        protected ILogger<SmsServiceYuboto> Logger { get; }

        /// <summary>Constructs the <see cref="SmsServiceViber"/> using the <seealso cref="SmsServiceViberSettings"/>.</summary>
        /// <param name="settings">The settings required to configure the service.</param>
        /// <param name="httpClient">Injected <see cref="System.Net.Http.HttpClient"/> managed by the DI.</param>
        /// <param name="logger">Represents a type used to perform logging.</param>
        public SmsServiceViber(HttpClient httpClient, SmsServiceViberSettings settings, ILogger<SmsServiceViber> logger) { }

        /// <inheritdoc/>
        public Task SendAsync(string destination, string subject, string body) {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Supports(string deliveryChannel) => "Viber".Equals(deliveryChannel, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Settings class for configuring <see cref="SmsServiceViber"/>.</summary>
    public class SmsServiceViberSettings
    {
        /// <summary>Key in the configuration.</summary>
        public static readonly string Name = "Viber";
        /// <summary>The API key.</summary>
        public string ApiKey { get; set; }
    }
}
