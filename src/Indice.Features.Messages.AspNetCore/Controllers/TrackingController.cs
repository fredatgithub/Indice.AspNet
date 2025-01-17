﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Indice.Features.Messages.Core;
using Indice.Features.Messages.Core.Services.Abstractions;
using Indice.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Indice.Features.Messages.AspNetCore.Controllers
{
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    [ApiController]
    [ApiExplorerSettings(GroupName = ApiGroups.MessageInboxEndpoints, IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = MessagesApi.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    [Route("_tracking")]
    internal class TrackingController : ControllerBase
    {
        public TrackingController(
            IInboxService inboxService,
            ICampaignService campaignService,
            IMessageTypeService messageTypeService,
            IOptions<MessageInboxOptions> campaignEndpointOptions
        ) {
            CampaignService = campaignService ?? throw new ArgumentNullException(nameof(campaignService));
            CampaignInboxOptions = campaignEndpointOptions?.Value ?? throw new ArgumentNullException(nameof(campaignEndpointOptions));
        }
        
        public ICampaignService CampaignService { get; }
        public MessageInboxOptions CampaignInboxOptions { get; }
        public string UserCode => User.FindFirstValue(CampaignInboxOptions.UserClaimType);

        [AllowAnonymous]
        [HttpGet("messages/cta/{trackingCode}")]
        public async Task<IActionResult> Track([FromRoute] Base64Id trackingCode) {
            var campaignId = trackingCode.Id;
            var campaign = await CampaignService.GetById(campaignId);
            if (campaign is null) {
                return NotFound();
            }
            await CampaignService.UpdateHit(trackingCode.Id);
            return Redirect(campaign.ActionLink.Href);
        }
    }
}
