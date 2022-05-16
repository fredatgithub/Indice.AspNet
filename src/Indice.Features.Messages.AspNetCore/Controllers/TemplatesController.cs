﻿using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Indice.Features.Messages.Core;
using Indice.Features.Messages.Core.Models;
using Indice.Features.Messages.Core.Models.Requests;
using Indice.Features.Messages.Core.Services.Abstractions;
using Indice.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Indice.Features.Messages.AspNetCore.Controllers
{
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    [ApiController]
    [ApiExplorerSettings(GroupName = ApiGroups.CampaignManagementEndpoints)]
    [Authorize(AuthenticationSchemes = MessagesApi.AuthenticationScheme, Policy = MessagesApi.Policies.BeCampaignManager)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [Route($"{ApiPrefixes.CampaignManagementEndpoints}/templates")]
    internal class TemplatesController : ControllerBase
    {
        public TemplatesController(ITemplateService templateService) {
            TemplateService = templateService;
        }

        public ITemplateService TemplateService { get; }

        /// <summary>Creates a new template in the store.</summary>
        /// <param name="request">The request model used to create a new template.</param>
        /// <response code="201">Created</response>
        [HttpPost]
        [ProducesResponseType(typeof(TemplateBase), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateRequest request) {
            var createdTemplate = await TemplateService.Create(request);
            return CreatedAtAction(nameof(GetTemplateById), new { templateId = createdTemplate.Id }, createdTemplate);
        }

        /// <summary>Gets the list of all templates using the provided <see cref="ListOptions"/>.</summary>
        /// <param name="options">List parameters used to navigate through collections. Contains parameters such as sort, search, page number and page size.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResultSet<TemplateBase>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTemplates([FromQuery] ListOptions options) {
            var templates = await TemplateService.GetList(options);
            return Ok(templates);
        }

        /// <summary>Gets a template by it's unique id.</summary>
        /// <param name="templateId">The id of the template.</param>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{templateId:guid}")]
        [ProducesResponseType(typeof(ResultSet<Template>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTemplateById([FromRoute] Guid templateId) {
            var template = await TemplateService.GetById(templateId);
            if (template is null) {
                return NotFound();
            }
            return Ok(template);
        }
    }
}
