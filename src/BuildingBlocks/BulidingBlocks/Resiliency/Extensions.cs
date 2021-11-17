using System.Collections.Generic;
using System.Reflection;
using BuildingBlocks.Resiliency.Fallback;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace BuildingBlocks.Resiliency
{
    public static class Extensions
    {
        public static IServiceCollection AddMediaterRetryPolicy(IServiceCollection services,
            IReadOnlyList<Assembly> assemblies)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IRetryableRequest<,>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            return services;
        }

        public static IServiceCollection AddMediaterFallbackPolicy(IServiceCollection services,
            IReadOnlyList<Assembly> assemblies)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FallbackBehavior<,>));

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IFallbackHandler<,>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithTransientLifetime());
            
            return services;
        }
    }
}