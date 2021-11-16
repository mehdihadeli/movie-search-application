using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Caching
{
    public static class Extensions
    {
        public static IServiceCollection AddCachingRequestPolicies(this IServiceCollection services,
            IList<Assembly> assembliesToScan)
        {
            // ICachePolicy discovery and registration
            services.Scan(scan => scan
                .FromAssemblies(assembliesToScan ?? AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.AssignableTo(typeof(ICachePolicy<,>)),
                    publicOnly: false)
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            // IInvalidateCachePolicy discovery and registration
            services.Scan(scan => scan
                .FromAssemblies(assembliesToScan ?? AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.AssignableTo(typeof(IInvalidateCachePolicy<,>)),
                    publicOnly: false)
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return services;
        }
    }
}