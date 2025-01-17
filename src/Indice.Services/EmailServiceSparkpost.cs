﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Indice.Extensions;
using Microsoft.Extensions.Logging;

namespace Indice.Services
{
    /// <summary>
    /// SparkPost implementation for the email service abstraction.
    /// https://developers.sparkpost.com/api/transmissions.html
    /// </summary>
    public class EmailServiceSparkpost : IEmailService
    {
        /// <summary>
        /// Creates a new instance of <see cref="EmailServiceSparkpost"/>.
        /// </summary>
        /// <param name="settings">An instance of <see cref="EmailServiceSparkPostSettings"/> used to initialize the service.</param>
        /// <param name="httpClient">The http client to use (DI managed)</param>
        /// <param name="logger">Represents a type used to perform logging.</param>
        /// <param name="htmlRenderingEngine">This is an abstraction for the rendering engine.</param>
        public EmailServiceSparkpost(
            EmailServiceSparkPostSettings settings,
            HttpClient httpClient,
            ILogger<EmailServiceSparkpost> logger,
            IHtmlRenderingEngine htmlRenderingEngine
        ) {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            HtmlRenderingEngine = htmlRenderingEngine ?? throw new ArgumentNullException(nameof(htmlRenderingEngine));
            if (HttpClient.BaseAddress == null) {
                HttpClient.BaseAddress = new Uri(Settings.Api.TrimEnd('/') + "/");
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Settings.ApiKey);
            }
        }

        private EmailServiceSparkPostSettings Settings { get; }
        private HttpClient HttpClient { get; }
        private ILogger<EmailServiceSparkpost> Logger { get; }
        /// <inheritdoc/>
        public IHtmlRenderingEngine HtmlRenderingEngine { get; }

        /// <inheritdoc/>
        public async Task SendAsync(string[] recipients, string subject, string body, EmailAttachment[] attachments = null) {
            var bccRecipients = (Settings.BccRecipients ?? "").Split(';', ',');
            var recipientAddresses = recipients.Select(recipient => new SparkPostRecipient {
                Address = new SparkPostRecipientEmailAddress {
                    Email = recipient
                }
            });
            var bccAddresses = bccRecipients.SelectMany(bcc => recipients.Select(recipient => new SparkPostRecipient {
                Address = new SparkPostRecipientEmailAddress {
                    Email = bcc,
                    HeaderTo = recipient
                }
            }));
            var request = new SparkPostRequest {
                Content = new SparkPostContent {
                    From = new SparkPostSenderAddress {
                        Email = Settings.Sender,
                        Name = Settings.SenderName
                    },
                    Subject = subject,
                    Html = body
                },
                Recipients = recipientAddresses.Concat(bccAddresses).ToArray()
            };
            if (attachments?.Length > 0) {
                var attachmentsList = new List<SparkPostAttachment>();
                foreach (var attachment in attachments) {
                    attachmentsList.Add(new SparkPostAttachment {
                        Name = attachment.FileName,
                        Type = FileExtensions.GetMimeType(Path.GetExtension(attachment.FileName)),
                        Data = Convert.ToBase64String(attachment.Data)
                    });
                }
                request.Content.Attachments = attachmentsList.ToArray();
            }
            var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var response = await HttpClient.PostAsync("transmissions", new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json));
            if (!response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();
                var message = $"SparkPost service could not send email to recipients '{string.Join(", ", recipients)}'. Error is: '{content}'.";
                Logger.LogError(message);
                throw new InvalidOperationException(message);
            }
        }
    }

    /// <summary>
    /// Custom settings that are used to send emails via SparkPost.
    /// </summary>
    public class EmailServiceSparkPostSettings
    {
        /// <summary>
        /// The configuration section name.
        /// </summary>
        public const string Name = "SparkPost";
        /// <summary>
        /// The default sender address (ex. no-reply@indice.gr).
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// The default sender name (ex. INDICE OE)
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Optional email addresses that are always added as blind carbon copy recipients.
        /// </summary>
        public string BccRecipients { get; set; }
        /// <summary>
        /// The SparkPost API key.
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// The SparkPost API URL (ex. https://api.eu.sparkpost.com/api/v1/).
        /// </summary>
        public string Api { get; set; } = "https://api.eu.sparkpost.com/api/v1/";
    }

    #region SparkPost Models
    internal class SparkPostRequest
    {
        public SparkPostContent Content { get; set; }
        public SparkPostRecipient[] Recipients { get; set; }
    }

    internal class SparkPostContent
    {
        public SparkPostSenderAddress From { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public SparkPostAttachment[] Attachments { get; set; }
    }

    internal class SparkPostSenderAddress
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    internal class SparkPostRecipient
    {
        public SparkPostRecipientEmailAddress Address { get; set; }
    }

    internal class SparkPostRecipientEmailAddress
    {
        public string Email { get; set; }
        /// <summary>
        /// Decides whether this email address will be associated with an other one. 
        /// If left blank the address will receive a separate email.
        /// </summary>
        [JsonPropertyName("header_to")]
        public string HeaderTo { get; set; }
    }

    internal class SparkPostAttachment
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
    #endregion
}
