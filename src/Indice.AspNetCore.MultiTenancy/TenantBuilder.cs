﻿/* 
 * Attribution: https://michael-mckenna.com/multi-tenant-asp-dot-net-core-application-tenant-resolution 
 */

using System;
using Indice.AspNetCore.MultiTenancy.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Indice.AspNetCore.MultiTenancy
{
    /// <summary>A builder used to configure the multi-tenancy feature.</summary>
    public class TenantBuilder<T> where T : Tenant
    {
        private readonly IServiceCollection _services;

        /// <summary>Constructs the <see cref="TenantBuilder{T}"/> class.</summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public TenantBuilder(IServiceCollection services) {
            if (typeof(T).Equals(typeof(Tenant))) {
                services.AddTransient<TenantAccessService>();
            }
            services.AddTransient<TenantAccessService<T>>();
            services.AddTransient<ITenantAccessor<T>, TenantAccessor<T>>();
            services.AddTransient<IAuthorizationHandler, BeTenantMemberHandler <T>>();
            _services = services;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithResolutionStrategy<V>(ServiceLifetime lifetime = ServiceLifetime.Transient) where V : class, ITenantResolutionStrategy {
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(V), lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="implementationFactory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithResolutionStrategy<V>(Func<IServiceProvider, V> implementationFactory, ServiceLifetime lifetime = ServiceLifetime.Transient) where V : class, ITenantResolutionStrategy {
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), implementationFactory, lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithStore<V>(ServiceLifetime lifetime = ServiceLifetime.Transient) where V : class, ITenantStore<T> {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(V), lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="implementationFactory"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithStore<V>(Func<IServiceProvider, V> implementationFactory, ServiceLifetime lifetime = ServiceLifetime.Transient) where V : class, ITenantStore<T> {
            _services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), implementationFactory, lifetime));
            return this;
        }
    }
}
