﻿using System.Globalization;
using System.Linq.Expressions;
using System.Security.Claims;
using Indice.Features.Cases.Data;
using Indice.Features.Cases.Data.Models;
using Indice.Features.Cases.Events;
using Indice.Features.Cases.Interfaces;
using Indice.Features.Cases.Models;
using Indice.Features.Cases.Models.Responses;
using Indice.Features.Cases.Resources;
using Indice.Security;
using Indice.Types;
using Microsoft.EntityFrameworkCore;

namespace Indice.Features.Cases.Services
{
    internal class MyCaseService : BaseCaseService, IMyCaseService
    {
        private const string SchemaSelector = "frontend";
        private readonly CasesDbContext _dbContext;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICaseEventService _caseEventService;
        private readonly IMyCaseMessageService _caseMessageService;
        private readonly IJsonTranslationService _jsonTranslationService;
        private readonly CaseSharedResourceService _caseSharedResourceService;

        public MyCaseService(
            CasesDbContext dbContext,
            ICaseTypeService caseTypeService,
            ICaseEventService caseEventService,
            IMyCaseMessageService caseMessageService,
            IJsonTranslationService jsonTranslationService,
            CaseSharedResourceService caseSharedResourceService) : base(dbContext) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _caseTypeService = caseTypeService ?? throw new ArgumentNullException(nameof(caseTypeService));
            _caseEventService = caseEventService ?? throw new ArgumentNullException(nameof(caseEventService));
            _caseMessageService = caseMessageService ?? throw new ArgumentNullException(nameof(caseMessageService));
            _jsonTranslationService = jsonTranslationService ?? throw new ArgumentNullException(nameof(jsonTranslationService));
            _caseSharedResourceService = caseSharedResourceService;
        }

        public async Task<CreateCaseResponse> CreateDraft(ClaimsPrincipal user,
            string caseTypeCode,
            string groupId,
            CustomerMeta customer,
            Dictionary<string, string> metadata,
            string channel) {
            if (user is null) throw new ArgumentNullException(nameof(user));
            if (caseTypeCode == null) throw new ArgumentNullException(nameof(caseTypeCode));

            var caseType = await _caseTypeService.Get(caseTypeCode);
            var entity = await CreateDraftInternal(
                _caseMessageService,
                user,
                caseType,
                groupId,
                customer,
                metadata,
                string.IsNullOrEmpty(channel) ? CasesApiConstants.Channels.Customer : channel);

            return new CreateCaseResponse {
                Id = entity.Id,
                Created = entity.CreatedBy!.When!.Value
            };
        }

        public async Task UpdateData(ClaimsPrincipal user, Guid caseId, dynamic data) {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (caseId == default) throw new ArgumentNullException(nameof(caseId));
            if (data == null) throw new ArgumentNullException(nameof(data));
            await _caseMessageService.Send(caseId, user, new Message { Data = data });
        }

        public async Task Submit(ClaimsPrincipal user, Guid caseId) {
            if (caseId == default) throw new ArgumentNullException(nameof(caseId));

            var @case = await GetDbCaseForCustomer(caseId, user);
            if (!@case.Draft) {
                throw new Exception("Case status is not draft."); // todo proper exception (BadRequest)
            }

            @case.Draft = false;
            await _dbContext.SaveChangesAsync();
            await _caseEventService.Publish(new CaseSubmittedEvent(@case, @case.CaseType.Code));
        }

        public async Task<Case> GetCaseById(ClaimsPrincipal user, Guid caseId) {
            var userId = user.FindSubjectId();
            var query =
                from c in GetCasesInternal(userId, includeAttachmentData: true, SchemaSelector)
                let isCustomer = userId == c.CustomerId
                let isCreator = userId == c.CreatedById
                let isOwner = isCustomer || isCreator
                where c.Id == caseId && isOwner
                select c;

            var @case = await query.FirstOrDefaultAsync();
            if (@case == null) {
                return null;
            }

            // the customer should be able to see only cases that have been created from him/herself!
            if (@case.Channel == CasesApiConstants.Channels.Agent) {
                throw new Exception("Case not found.");
            }

            @case.CaseType = TranslateCaseType(@case.CaseType, CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
            return @case;
        }

        public async Task<ResultSet<MyCasePartial>> GetCases(ClaimsPrincipal user, ListOptions<GetMyCasesListFilter> options) {
            var userId = user.FindSubjectId();
            var dbCaseQueryable = _dbContext.Cases
                .AsQueryable()
                .Where(p => (p.CreatedBy.Id == userId || p.Customer.UserId == userId) && !p.Draft && p.PublicCheckpoint.CheckpointType.Status != CaseStatus.Deleted)
                .Where(options.Filter.Metadata);

            foreach (var tag in options.Filter?.CaseTypeTags ?? new List<string>()) {
                // If there are more than 1 tag, the linq will be translated into "WHERE [Tag] LIKE %tag1% AND [Tag] LIKE %tag2% ..."
                dbCaseQueryable = dbCaseQueryable.Where(dbCase => EF.Functions.Like(dbCase.CaseType.Tags, $"%{tag}%"));
            }

            // filter Statuses
            if (options.Filter?.Statuses != null && options.Filter.Statuses.Count() > 0) {
                var expressions = options.Filter.Statuses.Select(status => (Expression<Func<DbCase, bool>>)(c => c.PublicCheckpoint.CheckpointType.Status == status));
                // Aggregate the expressions with OR that resolves to SQL: dbCase.PublicCheckpoint.CheckpointType.Status == status1 OR == status2 etc
                var aggregatedExpression = expressions.Aggregate((expression, next) => {
                    var orExp = Expression.OrElse(expression.Body, Expression.Invoke(next, expression.Parameters));
                    return Expression.Lambda<Func<DbCase, bool>>(orExp, expression.Parameters);
                });
                dbCaseQueryable = dbCaseQueryable.Where(aggregatedExpression);
            }

            // filter CreatedFrom
            if (options.Filter?.CreatedFrom != null) {
                dbCaseQueryable = dbCaseQueryable.Where(c => c.CreatedBy.When >= options.Filter.CreatedFrom);
            }
            // filter CreatedTo
            if (options.Filter?.CreatedTo != null) {
                dbCaseQueryable = dbCaseQueryable.Where(c => c.CreatedBy.When <= options.Filter.CreatedTo);
            }
            // filter CompletedFrom
            if (options.Filter?.CompletedFrom != null) {
                dbCaseQueryable = dbCaseQueryable.Where(c => c.CompletedBy != null && c.CompletedBy.When != null && c.CompletedBy.When >= options.Filter.CompletedFrom);
            }
            // filter CompletedTo
            if (options.Filter?.CompletedTo != null) {
                dbCaseQueryable = dbCaseQueryable.Where(c => c.CompletedBy != null && c.CompletedBy.When != null && c.CompletedBy.When <= options.Filter.CompletedTo);
            }

            // filter by Checkpoint Code
            if (options.Filter?.Checkpoints != null && options.Filter.Checkpoints.Count() > 0) {
                var expressions = options.Filter.Checkpoints.Select(checkpoints => (Expression<Func<DbCase, bool>>)(c => c.PublicCheckpoint.CheckpointType.Code == checkpoints));
                // Aggregate the expressions with OR that resolves to SQL: dbCase.PublicCheckpoint.CheckpointType.Code == checkpoint1 OR == checkpoint2 etc
                var aggregatedExpression = expressions.Aggregate((expression, next) => {
                    var orExp = Expression.OrElse(expression.Body, Expression.Invoke(next, expression.Parameters));
                    return Expression.Lambda<Func<DbCase, bool>>(orExp, expression.Parameters);
                });
                dbCaseQueryable = dbCaseQueryable.Where(aggregatedExpression);
            }

            // filter CaseTypeCodes
            if (options.Filter?.CaseTypeCodes != null && options.Filter.CaseTypeCodes.Count() > 0) {
                dbCaseQueryable = dbCaseQueryable.Where(c => options.Filter.CaseTypeCodes.Contains(c.CaseType.Code));
            }


            var myCasePartialQueryable =
                    dbCaseQueryable.Select(p => new MyCasePartial {
                        Id = p.Id,
                        Created = p.CreatedBy.When,
                        CaseTypeCode = p.CaseType.Code,
                        Status = p.PublicCheckpoint.CheckpointType.Status,
                        Checkpoint = p.PublicCheckpoint.CheckpointType.Code,
                        Message = _caseSharedResourceService.GetLocalizedHtmlString(p.Comments // get the translated version of the comment (if exist)
                            .OrderByDescending(p => p.CreatedBy.When)
                            .FirstOrDefault(c => !c.Private)
                            .Text ?? string.Empty),
                        Translations = TranslationDictionary<MyCasePartialTranslation>.FromJson(p.CaseType.Translations)
                    });

            // sorting option
            if (string.IsNullOrEmpty(options.Sort)) {
                options.Sort = $"{nameof(MyCasePartial.Created)}-";
            }

            var result = await myCasePartialQueryable.ToResultSetAsync(options);

            if (options.Filter?.Data != null) {
                var casesIdList = result.Items.Select(c => c.Id).ToList();
                // note: this searches all "data history" of case
                var filteredcasesIdList = await _dbContext.CaseData
                    .AsNoTracking()
                    .Where(dbCaseData => casesIdList.Contains(dbCaseData.CaseId))
                    .Where(options.Filter.Data)
                    .Select(d => d.CaseId)
                    .Distinct()
                    .AsQueryable()
                    .ToListAsync();

                result.Items = result.Items.Where(x => filteredcasesIdList.Contains(x.Id)).ToArray();
                result.Count = result.Items.Count();
            }

            // translate
            for (var i = 0; i < result.Items.Length; i++) {
                result.Items[i] = result.Items[i].Translate(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
            }

            return result;
        }

        public async Task<CaseTypePartial> GetCaseType(string caseTypeCode) {
            if (caseTypeCode == null) throw new ArgumentNullException(nameof(caseTypeCode));
            var dbCaseType = await GetCaseTypeInternal(caseTypeCode);
            if (dbCaseType == null) {
                throw new Exception("Case type not found."); // todo  proper exception & handle from problemConfig (NotFound)
            }

            var caseType = new CaseTypePartial {
                Code = dbCaseType.Code,
                DataSchema = GetSingleOrMultiple(SchemaSelector, dbCaseType.DataSchema),
                Layout = GetSingleOrMultiple(SchemaSelector, dbCaseType.Layout),
                LayoutTranslations = dbCaseType.LayoutTranslations,
                Title = dbCaseType.Title,
                Order = dbCaseType.Order,
                Translations = TranslationDictionary<CaseTypeTranslation>.FromJson(dbCaseType.Translations)
            };

            // translate case type
            caseType = TranslateCaseType(caseType, CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);

            return caseType;
        }

        public async Task<ResultSet<CaseTypePartial>> GetCaseTypes(ListOptions<GetMyCaseTypesListFilter> options) {
            var caseTypesQueryable = _dbContext.CaseTypes
                .AsQueryable();

            foreach (var tag in options.Filter?.CaseTypeTags ?? new List<string>()) {
                // If there are more than 1 tag, the linq will be translated into "WHERE [Tag] LIKE %tag1% AND [Tag] LIKE %tag2% ..."
                caseTypesQueryable = caseTypesQueryable.Where(caseType => EF.Functions.Like(caseType.Tags, $"%{tag}%"));
            }

            var caseTypes = await caseTypesQueryable
                .OrderBy(x => x.Category == null ? null : x.Category.Order)
                .ThenBy(x => x.Order)
                .Select(dbCaseType => new CaseTypePartial {
                    Id = dbCaseType.Id,
                    Title = dbCaseType.Title,
                    Description = dbCaseType.Description,
                    Category = dbCaseType.Category == null ? null : new Category {
                        Id = dbCaseType.Category.Id,
                        Name = dbCaseType.Category.Name,
                        Description = dbCaseType.Category.Description,
                        Order = dbCaseType.Category.Order,
                        Translations = TranslationDictionary<CategoryTranslation>.FromJson(dbCaseType.Category.Translations)
                    },
                    DataSchema = GetSingleOrMultiple(SchemaSelector, dbCaseType.DataSchema),
                    Layout = GetSingleOrMultiple(SchemaSelector, dbCaseType.Layout),
                    LayoutTranslations = dbCaseType.LayoutTranslations,
                    Code = dbCaseType.Code,
                    Order = dbCaseType.Order,
                    Translations = TranslationDictionary<CaseTypeTranslation>.FromJson(dbCaseType.Translations)
                })
                .ToListAsync();

            // translate case types
            for (var i = 0; i < caseTypes.Count; i++) {
                caseTypes[i] = TranslateCaseType(caseTypes[i], CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
                if (caseTypes[i].Category is not null) {
                    caseTypes[i].Category = TranslateCaseTypeCategory(caseTypes[i].Category, CultureInfo.CurrentCulture.TwoLetterISOLanguageName, true);
                }
            }

            return caseTypes.ToResultSet();
        }

        private CaseTypePartial TranslateCaseType(CaseTypePartial caseTypePartial, string culture, bool includeTranslations) {
            var caseType = caseTypePartial.Translate(culture, includeTranslations);
            caseType.Layout = _jsonTranslationService.Translate(caseType.Layout, caseTypePartial.LayoutTranslations, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            caseType.DataSchema = _jsonTranslationService.Translate(caseType.DataSchema, caseTypePartial.LayoutTranslations, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            return caseType;
        }

        private Category TranslateCaseTypeCategory(Category category, string culture, bool includeTranslations) =>
            category.Translate(culture, includeTranslations);
    }
}