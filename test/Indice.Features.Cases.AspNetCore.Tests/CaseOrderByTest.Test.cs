﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using IdentityModel;
using Indice.Features.Cases.Data;
using Indice.Features.Cases.Interfaces;
using Indice.Features.Cases.Models;
using Indice.Features.Cases.Resources;
using Indice.Features.Cases.Services;
using Indice.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace Indice.Features.Cases.Tests
{
    public class CaseOrderByTest : IDisposable
    {
        public CaseOrderByTest() {
            var inMemorySettings = new Dictionary<string, string> {
                ["ConnectionStrings:CasesDb"] = "Server=(localdb)\\MSSQLLocalDB;Database=ChaniaBank.Cases;Trusted_Connection=True;MultipleActiveResultSets=true",
            };
            Microsoft.Extensions.Configuration.IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var collection = new ServiceCollection()
                .AddDbContext<CasesDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("CasesDb")));
            ServiceProvider = collection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; }

        public void Dispose() {
        }

        [Fact]
        public async Task FilterBasedOnCode() {
            var dbContext = ServiceProvider.GetRequiredService<CasesDbContext>();
            var a = await dbContext.Cases.ToListAsync();
            var mockCaseTypeService = new Mock<ICaseTypeService>();
            var mockCaseEventService = new Mock<ICaseEventService>();
            var mockMyCaseMessageService = new Mock<IMyCaseMessageService>();
            var mockJsonTranslationService = new Mock<IJsonTranslationService>();
            var mockResourceService = new CaseSharedResourceService(new Mock<IStringLocalizerFactory>().Object);

            var myCaseService = new MyCaseService(dbContext,
                mockCaseTypeService.Object,
                mockCaseEventService.Object,
                mockMyCaseMessageService.Object,
                mockJsonTranslationService.Object,
                mockResourceService);
            var options = new ListOptions<GetMyCasesListFilter>() { };
            //options.AddSort(new SortByClause("checkpointcontainsDownloaded", "DESC"));
            options.AddSort(new SortByClause("Created", "DESC"));
            var result = await myCaseService.GetCases(User(), options);
        }

        private static ClaimsPrincipal User() {
            var claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Scope, CasesApiConstants.Scope),
                    new Claim(JwtClaimTypes.Subject, "ab9769f1-d532-4b7d-9922-3da003157ebd"),
                    new Claim(JwtClaimTypes.Email, "Case API"),
                    new Claim(JwtClaimTypes.GivenName, "Case API"),
                    new Claim(JwtClaimTypes.FamilyName, "Case API"),
                };
            var identity = new ClaimsIdentity(claims, "Basic"); // By setting "Basic" we are making the identity "Authenticated" so we can user user.IsAuthenticated() property later in our code
            return new ClaimsPrincipal(identity);
        }
    }

    
}
