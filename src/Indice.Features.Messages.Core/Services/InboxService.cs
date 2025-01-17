﻿using Indice.Features.Messages.Core.Data;
using Indice.Features.Messages.Core.Data.Models;
using Indice.Features.Messages.Core.Exceptions;
using Indice.Features.Messages.Core.Models;
using Indice.Features.Messages.Core.Models.Requests;
using Indice.Features.Messages.Core.Services.Abstractions;
using Indice.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Indice.Features.Messages.Core.Services
{
    /// <summary>
    /// An implementation of <see cref="IInboxService"/> for Entity Framework Core.
    /// </summary>
    public class InboxService : IInboxService
    {
        /// <summary>
        /// Creates a new instance of <see cref="InboxService"/>.
        /// </summary>
        /// <param name="dbContext">The <see cref="Microsoft.EntityFrameworkCore.DbContext"/> for Campaigns API feature.</param>
        /// <param name="campaignInboxOptions">Options used to configure the Campaigns inbox API feature.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public InboxService(
            CampaignsDbContext dbContext,
            IOptions<MessageInboxOptions> campaignInboxOptions
        ) {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            CampaignInboxOptions = campaignInboxOptions?.Value ?? throw new ArgumentNullException(nameof(campaignInboxOptions));
        }

        private CampaignsDbContext DbContext { get; }
        private MessageInboxOptions CampaignInboxOptions { get; }

        /// <inheritdoc />
        public async Task<ResultSet<Message>> GetList(string userCode, ListOptions<MessagesFilter> options) {
            var userMessages = await GetUserInboxQuery(userCode, options).ToResultSetAsync(options);
            return userMessages;
        }

        /// <inheritdoc />
        public Task<Message> GetById(Guid id, string recipientId) => GetUserInboxQuery(recipientId).SingleOrDefaultAsync(x => x.Id == id);

        /// <inheritdoc />
        public async Task MarkAsDeleted(Guid id, string recipientId) {
            var message = await DbContext.Messages.SingleOrDefaultAsync(x => x.CampaignId == id && x.RecipientId == recipientId);
            if (message is not null) {
                if (message.IsDeleted) {
                    throw MessageExceptions.MessageAlreadyRead(id);
                }
                message.IsDeleted = true;
                message.DeleteDate = DateTime.UtcNow;
            } else {
                DbContext.Messages.Add(new DbMessage {
                    CampaignId = id,
                    DeleteDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    IsDeleted = true,
                    RecipientId = recipientId
                });
            }
            await DbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task MarkAsRead(Guid id, string recipientId) {
            var message = await DbContext.Messages.SingleOrDefaultAsync(x => x.CampaignId == id && x.RecipientId == recipientId);
            if (message is not null) {
                if (message.IsRead) {
                    throw MessageExceptions.MessageAlreadyRead(id);
                }
                message.IsRead = true;
                message.ReadDate = DateTime.UtcNow;
            } else {
                DbContext.Messages.Add(new DbMessage {
                    CampaignId = id,
                    Id = Guid.NewGuid(),
                    IsRead = true,
                    ReadDate = DateTime.UtcNow,
                    RecipientId = recipientId
                });
            }
            await DbContext.SaveChangesAsync();
        }

        private IQueryable<Message> GetUserInboxQuery(string recipientId, ListOptions<MessagesFilter> options = null) {
            var query = DbContext
                .Campaigns
                .AsNoTracking()
                .Include(x => x.Attachment)
                .Include(x => x.Type)
                .SelectMany(
                    collectionSelector: campaign => DbContext.Messages.AsNoTracking().Where(x => x.CampaignId == campaign.Id && x.RecipientId == recipientId).DefaultIfEmpty(),
                    resultSelector: (campaign, message) => new { Campaign = campaign, Message = message }
                )
                .Where(x => x.Campaign.Published
                    && x.Campaign.MessageChannelKind.HasFlag(MessageChannelKind.Inbox)
                    && (x.Message == null || !x.Message.IsDeleted)
                    && (x.Campaign.IsGlobal || x.Message != null && x.Message.RecipientId == recipientId)
                );
            if (options?.Filter is not null) {
                if (options.Filter.ShowExpired.HasValue) {
                    query = query.Where(x => !x.Campaign.ActivePeriod.To.HasValue || x.Campaign.ActivePeriod.To.Value >= DateTime.UtcNow);
                }
                if (options.Filter.TypeId.Length > 0) {
                    query = query.Where(x => x.Campaign.Type == null || options.Filter.TypeId.Contains(x.Campaign.Type.Id));
                }
                if (options.Filter.ActiveFrom.HasValue) {
                    query = query.Where(x => (x.Campaign.ActivePeriod.To ?? DateTimeOffset.MaxValue) > options.Filter.ActiveFrom.Value);
                }
                if (options.Filter.ActiveTo.HasValue) {
                    query = query.Where(x => (x.Campaign.ActivePeriod.From ?? DateTimeOffset.MinValue) < options.Filter.ActiveTo.Value);
                }
                if (options.Filter.IsRead.HasValue) {
                    query = query.Where(x => ((bool?)x.Message.IsRead ?? false) == options.Filter.IsRead);
                }
            }
            return query.Select(x => new Message {
                ActionLink = x.Campaign.ActionLink != null ? new Hyperlink {
                    Text = x.Campaign.ActionLink.Text,
                    Href = !string.IsNullOrEmpty(x.Campaign.ActionLink.Href)
                        ? $"_tracking/messages/cta/{(Base64Id)x.Campaign.Id}"
                        : null
                } : null,
                ActivePeriod = x.Campaign.ActivePeriod,
                AttachmentUrl = x.Campaign.Attachment != null
                    ? $"{CampaignInboxOptions.ApiPrefix}/messages/attachments/{(Base64Id)x.Campaign.Attachment.Guid}.{Path.GetExtension(x.Campaign.Attachment.Name).TrimStart('.')}"
                    : null,
                // TODO: Fix substitution when message is null.
                Title = x.Message != null ? x.Message.Content[nameof(MessageChannelKind.Inbox)].Title : x.Campaign.Content[nameof(MessageChannelKind.Inbox)].Title,
                Content = x.Message != null ? x.Message.Content[nameof(MessageChannelKind.Inbox)].Body : x.Campaign.Content[nameof(MessageChannelKind.Inbox)].Body,
                CreatedAt = x.Campaign.CreatedAt,
                Id = x.Campaign.Id,
                IsRead = x.Message != null && x.Message.IsRead,
                Type = x.Campaign.Type != null ? new MessageType {
                    Id = x.Campaign.Type.Id,
                    Name = x.Campaign.Type.Name
                } : null
            });
        }
    }
}
