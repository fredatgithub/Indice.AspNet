﻿using System.Reflection;
using Elsa;
using Elsa.Activities.UserTask.Extensions;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Indice.Features.Cases.Converters;
using Indice.Features.Cases.Data;
using Indice.Features.Cases.Events;
using Indice.Features.Cases.Factories;
using Indice.Features.Cases.Handlers;
using Indice.Features.Cases.Interfaces;
using Indice.Features.Cases.Mvc.Conventions;
using Indice.Features.Cases.Resources;
using Indice.Features.Cases.Services;
using Indice.Features.Cases.Services.CaseMessageService;
using Indice.Features.Cases.Services.NoOpServices;
using Indice.Features.Cases.Workflows.Activities;
using Indice.Features.Cases.Workflows.Bookmarks.AwaitApproval;
using Indice.Features.Cases.Workflows.Interfaces;
using Indice.Features.Cases.Workflows.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Indice.Features.Cases
{
    /// <summary>
    /// Register Case Api Features.
    /// </summary>
    public static class CasesApiFeatureExtensions
    {
        /// <summary>
        /// Add case management Api endpoints for Customer (api/my prefix).
        /// </summary>
        /// <param name="mvcBuilder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="configureAction">The <see cref="IConfiguration"/>.</param>
        public static IMvcBuilder AddCasesApiEndpoints(this IMvcBuilder mvcBuilder, Action<CasesApiOptions> configureAction = null) {
            // Add
            mvcBuilder.ConfigureApplicationPartManager(x => x.FeatureProviders.Add(new CasesApiFeatureProviderMyCases()));
            var services = mvcBuilder.Services;

            // Build service provider and get IConfiguration instance.
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Try add general settings.
            services.AddGeneralSettings(configuration);

            // This lines resolves the CaseData dynamic deserialization from SystemText
            services.AddMvc()
                .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new SystemTextConverter()));

            // Configure options given by the consumer.
            var casesApiOptions = new CasesApiOptions();
            configureAction?.Invoke(casesApiOptions);
            services.Configure<CasesApiOptions>(options => {
                options.ApiPrefix = casesApiOptions.ApiPrefix;
                options.ConfigureDbContext = casesApiOptions.ConfigureDbContext;
                options.DatabaseSchema = casesApiOptions.DatabaseSchema;
                options.ExpectedScope = casesApiOptions.ExpectedScope;
                options.UserClaimType = casesApiOptions.UserClaimType;
                options.GroupIdClaimType = casesApiOptions.GroupIdClaimType;
            }).AddSingleton(casesApiOptions);

            // Post configure MVC options.
            services.PostConfigure<MvcOptions>(options => {
                options.Conventions.Add(new ApiPrefixControllerModelConvention(casesApiOptions));
            });

            // Register framework services.
            services.AddHttpContextAccessor();

            // Register no op services.
            services.AddLookupService<NoOpLookupService>(nameof(NoOpLookupService)); // needed for factory instantiation
            services.AddTransient<ICustomerIntegrationService, NoOpCustomerIntegrationService>();
            services.AddTransient<ICasePdfService, NoOpCasePdfService>();

            // Register LookupService Factory
            services.AddTransient<ILookupServiceFactory, DefaultLookupServiceFactory>();

            // Register custom services.
            services.AddTransient<IMyCaseService, MyCaseService>();
            services.AddTransient<ICaseTypeService, CaseTypeService>();
            services.AddTransient<ISchemaValidator, SchemaValidator>();
            services.AddTransient<ICheckpointTypeService, CheckpointTypeService>();
            services.AddTransient<ICaseTemplateService, CaseTemplateService>();
            services.AddTransient<IMyCaseMessageService, MyCaseMessageService>();
            services.AddTransient<IJsonTranslationService, JsonTranslationService>();
            services.AddSingleton<CaseSharedResourceService>(); // Add the service even if there is no resx file, so the runtime will not throw exception

            // Register events.
            services.AddTransient<ICaseEventService, CaseEventService>();

            // Register internal handlers
            services.AddCaseEventHandler<CaseSubmittedEvent, StartWorkflowHandler>();

            // Register application DbContext.
            services.AddDbContext<CasesDbContext>(casesApiOptions.ConfigureDbContext ?? (builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))));

            return mvcBuilder;
        }

        /// <summary>
        /// Add case management Api endpoints for manage cases from back-office (api/manage prefix).
        /// </summary>
        /// <param name="mvcBuilder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="configureAction">The <see cref="IConfiguration"/>.</param>
        public static IMvcBuilder AddAdminCasesApiEndpoints(this IMvcBuilder mvcBuilder, Action<CasesApiOptions> configureAction = null) {
            // Add
            mvcBuilder.ConfigureApplicationPartManager(x => x.FeatureProviders.Add(new CasesApiFeatureProviderAdminCases()));
            var services = mvcBuilder.Services;

            // Build service provider and get IConfiguration instance.
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Try add general settings.
            services.AddGeneralSettings(configuration);

            // Configure options given by the consumer.
            var casesApiOptions = new CasesApiOptions();
            configureAction?.Invoke(casesApiOptions);
            services.Configure<CasesApiOptions>(options => {
                options.ApiPrefix = casesApiOptions.ApiPrefix;
                options.ConfigureDbContext = casesApiOptions.ConfigureDbContext;
                options.DatabaseSchema = casesApiOptions.DatabaseSchema;
                options.ExpectedScope = casesApiOptions.ExpectedScope;
                options.UserClaimType = casesApiOptions.UserClaimType;
                options.GroupIdClaimType = casesApiOptions.GroupIdClaimType;
            }).AddSingleton(casesApiOptions);

            // Post configure MVC options.
            services.PostConfigure<MvcOptions>(options => {
                options.Conventions.Add(new ApiPrefixControllerModelConvention(casesApiOptions));
            });

            // Register framework services.
            services.AddHttpContextAccessor();

            // Register custom services.
            services.AddTransient<IAdminCaseService, AdminCaseService>();
            services.AddTransient<IQueryService, QueryService>();
            services.AddTransient<ICaseAuthorizationService, RoleCaseTypeService>();
            services.AddTransient<ICaseActionsService, CaseActionsService>();
            services.AddTransient<IAdminCaseMessageService, AdminCaseMessageService>();
            services.AddTransient<ISchemaValidator, SchemaValidator>();
            services.AddTransient<ICaseApprovalService, CaseApprovalService>();
            services.AddTransient<INotificationSubscriptionService, NotificationSubscriptionService>();
            services.AddSmsServiceYubotoOmni(configuration)
                .AddViberServiceYubotoOmni(configuration)
                .AddEmailServiceSparkpost(configuration)
                .WithMvcRazorRendering();
            services.AddTransient<CasesMessageDescriber>();
            services.AddTransient<IJsonTranslationService, JsonTranslationService>();
            services.AddSingleton<CaseSharedResourceService>(); // Add the service even if there is no resx file, so the runtime will not throw exception

            //add the provider that filters through all available ICaseAuthorizationServices
            services.AddTransient<ICaseAuthorizationProvider, AggregateCaseAuthorizationProvider>();

            // Register events.
            services.AddTransient<ICaseEventService, CaseEventService>();

            // Register internal handlers
            services.AddCaseEventHandler<CaseSubmittedEvent, StartWorkflowHandler>();

            // Register application DbContext.
            services.AddDbContext<CasesDbContext>(casesApiOptions.ConfigureDbContext ?? (builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))));

            return mvcBuilder;
        }

        /// <summary>
        /// Adding a Case Authorization Service serve as an
        /// extra way to filter the list of cases a BO user
        /// can see or have access to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddCaseAuthorizationService<T>(this IMvcBuilder mvcBuilder) where T : ICaseAuthorizationService {
            mvcBuilder.Services.AddTransient(typeof(ICaseAuthorizationService), typeof(T));
            return mvcBuilder;
        }

        /// <summary>
        /// Registers an implementation of <see cref="ICaseEventHandler{TEvent}"/> for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to handler.</typeparam>
        /// <typeparam name="TEventHandler">The handler to user for the specified event.</typeparam>
        /// <param name="services">The services available in the application.</param>
        public static IServiceCollection AddCaseEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
            where TEvent : ICaseEvent
            where TEventHandler : class, ICaseEventHandler<TEvent> {
            var serviceDescriptor = new ServiceDescriptor(typeof(ICaseEventHandler<TEvent>), typeof(TEventHandler), ServiceLifetime.Transient);
            services.TryAddEnumerable(serviceDescriptor);
            return services;
        }

        /// <summary>
        /// Add workflow services to the case management.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="workflowAssembly">The assembly with the workflow activities and definitions to register.</param>
        public static void AddWorkflow(this IServiceCollection services, IConfiguration configuration, Assembly workflowAssembly) {
            services.AddElsa(elsa => {
                elsa.UseEntityFrameworkPersistence(ef => ef.UseSqlServer(configuration.GetConnectionString("WorkflowDb")), false)
                    .AddQuartzTemporalActivities()
                    .AddHttpActivities(configuration.GetSection("Elsa").GetSection("Server").Bind)
                    .AddEmailActivities(configuration.GetSection("Elsa").GetSection("Smtp").Bind)
                    .AddUserTaskActivities()
                    .AddActivitiesFrom(typeof(BaseCaseActivity).Assembly);

                // Register consumer assembly
                if (workflowAssembly != null) {
                    elsa.AddWorkflowsFrom(workflowAssembly);
                    elsa.AddActivitiesFrom(workflowAssembly);
                }
            });

            // Elsa API endpoints.
            services.AddElsaApiEndpoints();

            // For Dashboard.
            services.AddRazorPages();

            // Register Indices' bookmarks
            services.AddBookmarkProvidersFrom(typeof(AwaitApprovalBookmark).Assembly);

            // Register bookmarks from consumer assembly
            if (workflowAssembly != null) {
                services.AddBookmarkProvidersFrom(workflowAssembly);
            }

            // Register Custom Services
            services.AddScoped<IAwaitApprovalInvoker, AwaitApprovalInvoker>();
            services.AddScoped<IAwaitEditInvoker, AwaitEditInvoker>();
            services.AddScoped<IAwaitAssignmentInvoker, AwaitAssignmentInvoker>();
            services.AddScoped<IAwaitActionInvoker, AwaitActionInvoker>();
        }

        /// <summary>
        /// Add workflow services to middleware.
        /// </summary>
        /// <param name="app"></param>
        public static void UseWorkflow(this IApplicationBuilder app) {
            app.UseHttpActivities();
        }

        /// <summary>
        /// Override cases resources from <see cref="CasesMessageDescriber"/>.
        /// </summary>
        /// <typeparam name="TDescriber">The type of cases message describer.</typeparam>       
        /// <param name="services"></param>
        /// <returns></returns>        
        public static IServiceCollection AddCasesMessageDescriber<TDescriber>(this IServiceCollection services) where TDescriber : CasesMessageDescriber {
            services.AddScoped<CasesMessageDescriber, TDescriber>();
            return services;
        }
    }
}
