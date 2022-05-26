﻿using System.IO.Compression;
using System.Text.Json;
using Indice.Features.Messages.Core;
using Indice.Features.Messages.Core.Events;
using Indice.Features.Messages.Core.Handlers;
using Indice.Serialization;
using Indice.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Indice.Features.Messages.Worker.Azure
{
    internal class QueueTriggers
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = JsonSerializerOptionDefaults.GetDefaultSettings();

        public QueueTriggers(
            MessageJobHandlerFactory campaignJobHandlerFactory,
            Func<string, IEventDispatcher> getEventDispatcher
        ) {
            CampaignJobHandlerFactory = campaignJobHandlerFactory ?? throw new ArgumentNullException(nameof(campaignJobHandlerFactory));
            GetEventDispatcher = getEventDispatcher ?? throw new ArgumentNullException(nameof(getEventDispatcher));
        }

        private MessageJobHandlerFactory CampaignJobHandlerFactory { get; }
        private Func<string, IEventDispatcher> GetEventDispatcher { get; }

        [Function(EventNames.CampaignCreated)]
        public async Task CampaignPublishedHandler(
            [QueueTrigger("%ENVIRONMENT%-" + EventNames.CampaignCreated, Connection = "StorageConnection")] byte[] message,
            FunctionContext executionContext
        ) {
            LogExecution(executionContext, EventNames.CampaignCreated);
            var originalMessage = await CompressionUtils.Decompress(message);
            var @event = JsonSerializer.Deserialize<CampaignCreatedEvent>(originalMessage, JsonSerializerOptions);
            var campaignStart = @event.ActivePeriod?.From;
            // Azure queues can store a queue message with a visibility window up to 7 days. So if a campaign must start (appear on queue) after more than 7 days then we should check the campaign start date and re-enqueue the message.
            if (campaignStart > DateTimeOffset.UtcNow) {
                var nextExecutionTimeSpan = campaignStart.Value - DateTimeOffset.UtcNow;
                var visibilityWindow = nextExecutionTimeSpan > TimeSpan.FromDays(5) ? TimeSpan.FromDays(5) : nextExecutionTimeSpan;
                var eventDispatcher = GetEventDispatcher(KeyedServiceNames.EventDispatcherServiceKey);
                await GetEventDispatcher(KeyedServiceNames.EventDispatcherServiceKey).RaiseEventAsync(@event, options => options.WrapInEnvelope(false).Delay(visibilityWindow).WithQueueName(EventNames.CampaignCreated));
                return;
            }
            await CampaignJobHandlerFactory.Create<CampaignCreatedEvent>().Process(@event);
        }

        [Function(EventNames.ResolveMessage)]
        public async Task ResolveMessageHandler(
            [QueueTrigger("%ENVIRONMENT%-" + EventNames.ResolveMessage, Connection = "StorageConnection")] byte[] message,
            FunctionContext executionContext
        ) {
            LogExecution(executionContext, EventNames.ResolveMessage);
            var originalMessage = await CompressionUtils.Decompress(message);
            var @event = JsonSerializer.Deserialize<ResolveMessageEvent>(originalMessage, JsonSerializerOptions);
            await CampaignJobHandlerFactory.Create<ResolveMessageEvent>().Process(@event);
        }

        [Function(EventNames.SendPushNotification)]
        public async Task SendPushNotificationHandler(
            [QueueTrigger("%ENVIRONMENT%-" + EventNames.SendPushNotification, Connection = "StorageConnection")] byte[] message,
            FunctionContext executionContext
        ) {
            LogExecution(executionContext, EventNames.SendPushNotification);
            var originalMessage = await CompressionUtils.Decompress(message);
            var @event = JsonSerializer.Deserialize<SendPushNotificationEvent>(originalMessage, JsonSerializerOptions);
            await CampaignJobHandlerFactory.Create<SendPushNotificationEvent>().Process(@event);
        }

        [Function(EventNames.SendEmail)]
        public async Task SendEmailHandler(
            [QueueTrigger("%ENVIRONMENT%-" + EventNames.SendEmail, Connection = "StorageConnection")] byte[] message,
            FunctionContext executionContext
        ) {
            LogExecution(executionContext, EventNames.SendEmail);
            var originalMessage = await CompressionUtils.Decompress(message);
            var @event = JsonSerializer.Deserialize<SendEmailEvent>(originalMessage, JsonSerializerOptions);
            await CampaignJobHandlerFactory.Create<SendEmailEvent>().Process(@event);
        }

        [Function(EventNames.SendSms)]
        public async Task SendSmsHandler(
            [QueueTrigger("%ENVIRONMENT%-" + EventNames.SendSms, Connection = "StorageConnection")] byte[] message,
            FunctionContext executionContext
        ) {
            LogExecution(executionContext, EventNames.SendSms);
            var originalMessage = await CompressionUtils.Decompress(message);
            var @event = JsonSerializer.Deserialize<SendSmsEvent>(originalMessage, JsonSerializerOptions);
            await CampaignJobHandlerFactory.Create<SendSmsEvent>().Process(@event);
        }

        private static void LogExecution(FunctionContext executionContext, string eventName) {
            var logger = executionContext.GetLogger(eventName);
            logger.LogInformation("Function '{FunctionName}' was triggered.", eventName);
        }
    }
}
