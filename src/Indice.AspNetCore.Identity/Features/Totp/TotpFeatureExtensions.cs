﻿using System;
using System.Security;
using IdentityServer4.Configuration;
using Indice.AspNetCore.Identity;
using Indice.AspNetCore.Identity.Features;
using Indice.Configuration;
using Indice.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Adds feature extensions to the <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class TotpFeatureExtensions
    {
        /// <summary>
        /// Adds all required stuff in order for TOTP strong customer authentication (SCA) to work.
        /// </summary>
        /// <param name="builder">IdentityServer builder Interface.</param>
        /// <param name="configure">Configuration used in <see cref="Rfc6238AuthenticationService"/> service.</param>
        public static IMvcBuilder AddTotp(this IMvcBuilder builder, Action<TotpOptions> configure = null) => AddTotp<TotpService>(builder, configure);

        /// <summary>
        /// Adds all required stuff in order for TOTP strong customer authentication (SCA) to work.
        /// </summary>
        /// <typeparam name="TotpService">The type of <see cref="ITotpService"/> service implementation to use.</typeparam>
        /// <param name="builder">IdentityServer builder Interface.</param>
        /// <param name="configure">Configuration used in <see cref="Rfc6238AuthenticationService"/> service.</param>
        public static IMvcBuilder AddTotp<TotpService>(this IMvcBuilder builder, Action<TotpOptions> configure = null) where TotpService : class, ITotpService {
            builder.Services.AddDefaultTotpService(configure);
            builder.ConfigureApplicationPartManager(x => x.FeatureProviders.Add(new TotpFeatureProvider()));
            return builder;
        }

        /// <summary>
        /// Adds all required stuff in order for TOTP strong customer authentication (SCA) to work.
        /// </summary>
        /// <param name="builder">IdentityServer builder Interface.</param>
        /// <param name="configure">Configuration used in <see cref="Rfc6238AuthenticationService"/>.</param>
        public static IIdentityServerBuilder AddTotp(this IIdentityServerBuilder builder, Action<TotpOptions> configure = null) => AddTotp<TotpService>(builder, configure);

        /// <summary>
        /// Adds all required stuff in order for TOTP strong customer authentication (SCA) to work.
        /// </summary>
        /// <typeparam name="TotpService">The type of <see cref="ITotpService"/> service implementation to use.</typeparam>
        /// <param name="builder">IdentityServer builder Interface.</param>
        /// <param name="configure">Configuration used in <see cref="Rfc6238AuthenticationService"/> service.</param>
        public static IIdentityServerBuilder AddTotp<TotpService>(this IIdentityServerBuilder builder, Action<TotpOptions> configure = null) where TotpService : class, ITotpService {
            builder.Services.AddDefaultTotpService(configure);
            builder.Services.Configure<IdentityServerOptions>((options) => {
                options.Discovery.CustomEntries.Add("totp", new {
                    endpoint = options.IssuerUri.TrimEnd('/') + "/totp",
                    providers = new[] { $"{TotpProviderType.Phone}", $"{TotpProviderType.EToken}" },
                    channels = new[] { $"{TotpDeliveryChannel.Sms}", $"{TotpDeliveryChannel.PushNotification}" }
                });
            });
            builder.AddExtensionGrantValidator<TotpGrantValidator>();
            return builder;
        }

        /// <summary>
        /// Registers the <see cref="TotpDbContext"/> to be used by the application.
        /// </summary>
        /// <param name="options">Options for configuring the IdentityServer API feature.</param>
        /// <param name="configureAction">Configuration for <see cref="TotpDbContext"/>.</param>
        public static void AddDbContext(this TotpOptions options, Action<TotpDbContextOptions> configureAction) {
            var dbContextOptions = new TotpDbContextOptions();
            configureAction?.Invoke(dbContextOptions);
            options.Services.AddSingleton(dbContextOptions);
            if (dbContextOptions.ResolveDbContextOptions != null) {
                options.Services.AddDbContext<TotpDbContext>(dbContextOptions.ResolveDbContextOptions);
            } else {
                options.Services.AddDbContext<TotpDbContext>(dbContextOptions.ConfigureDbContext);
            }
            options.Services.AddTransient<Func<TotpDbContext>>(provider => provider.GetService<TotpDbContext>);
        }
    }
}
