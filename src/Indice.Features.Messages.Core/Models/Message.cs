﻿using Indice.Types;

namespace Indice.Features.Messages.Core.Models
{
    /// <summary>
    /// Models a user message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The unique identifier of the user message.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The title of the user message.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The content of the user message.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Determines if a message is read by the user.
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// Defines a (call-to-action) link.
        /// </summary>
        public Hyperlink ActionLink { get; set; }
        /// <summary>
        /// The URL to the attachment.
        /// </summary>
        public string AttachmentUrl { get; set; }
        /// <summary>
        /// Defines when the message was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        /// <summary>
        /// Specifies the time period that a campaign is active.
        /// </summary>
        public Period ActivePeriod { get; set; }
        /// <summary>
        /// The type details of the campaign.
        /// </summary>
        public MessageType Type { get; set; }
    }
}
