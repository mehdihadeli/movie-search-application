using System;
using System.Linq;
using Asp.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Web;

public static class ServiceCollectionExtensions
{
    public static void AddCustomVersioning(
        this IServiceCollection services,
        Action<ApiVersioningOptions> configurator = null
    )
    {
        // https://www.meziantou.net/versioning-an-asp-net-core-api.htm
        // https://dotnetthoughts.net/aspnetcore-api-versioning-with-net-6-minimal-apis/
        // https://im5tu.io/article/2022/10/asp.net-core-versioning-minimal-apis/
        // https://www.youtube.com/watch?v=YRJGKyzjFlY
        // https://www.nuget.org/packages/Asp.Versioning.Http

        // Support versioning in minimal apis with (Asp.Versioning.Http) dll
        services
            .AddApiVersioning(options =>
            {
                // Add the headers "api-supported-versions" and "api-deprecated-versions"
                // This is better for discoverability
                options.ReportApiVersions = true;

                // AssumeDefaultVersionWhenUnspecified should only be enabled when supporting legacy services that did not previously
                // support API versioning. Forcing existing clients to specify an explicit API version for an
                // existing service introduces a breaking change. Conceptually, clients in this situation are
                // bound to some API version of a service, but they don't know what it is and never explicit request it.
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Defines how an API version is read from the current HTTP request
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("api-version"),
                    new QueryStringApiVersionReader(),
                    new UrlSegmentApiVersionReader()
                );

                configurator?.Invoke(options);
            })
            .AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note1: this option is only necessary when versioning by url segment.
                // note2:if we set it to true version will infer automatically in swagger based on mapped versions to api (swagger infer api versions in different tab in header Swagger UI),
                // but if we set it to false we should put version for api endpoint manually.
                // https://github.com/dotnet/aspnet-api-versioning/issues/909
                options.SubstituteApiVersionInUrl = true;
            })
            // Support versioning in mvc with with (Asp.Versioning.Mvc.ApiExplorer) dll
            .AddMvc(); // https://www.nuget.org/packages/Asp.Versioning.Mvc.ApiExplorer
    }

    public static IServiceCollection AddCustomHealthCheck(
        this IServiceCollection services,
        Action<IHealthChecksBuilder> configurator = null
    )
    {
        var healCheckBuilder = services.AddHealthChecks();
        configurator?.Invoke(healCheckBuilder);

        //// Health Check UI does not work with .NET 6.0
        // services.AddHealthChecksUI(setup =>
        // {
        //     setup.SetEvaluationTimeInSeconds(60); //time in seconds between check
        //     setup.AddHealthCheckEndpoint("Basic Health Check", "/healthz");
        // }).AddInMemoryStorage();

        return services;
    }

    public static void Unregister<TService>(this IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
        services.Remove(descriptor);
    }

    public static void Replace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.Unregister<TService>();
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
    }

    /// <summary>
    ///     Adds a new transient registration to the service collection only when no existing registration of the same service
    ///     type and implementation type exists.
    ///     In contrast to TryAddTransient, which only checks the service type.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="serviceType">Service type</param>
    /// <param name="implementationType">Implementation type</param>
    private static void TryAddTransientExact(
        this IServiceCollection services,
        Type serviceType,
        Type implementationType
    )
    {
        if (services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType))
            return;

        services.AddTransient(serviceType, implementationType);
    }

    /// <summary>
    ///     Adds a new transient registration to the service collection only when no existing registration of the same service
    ///     type and implementation type exists.
    ///     In contrast to TryAddScoped, which only checks the service type.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="serviceType">Service type</param>
    /// <param name="implementationType">Implementation type</param>
    private static void TryAddScopeExact(this IServiceCollection services, Type serviceType, Type implementationType)
    {
        if (services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType))
            return;

        services.AddScoped(serviceType, implementationType);
    }

    /// <summary>
    ///     Adds a new transient registration to the service collection only when no existing registration of the same service
    ///     type and implementation type exists.
    ///     In contrast to TryAddSingleton, which only checks the service type.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="serviceType">Service type</param>
    /// <param name="implementationType">Implementation type</param>
    private static void TryAddSingletonExact(
        this IServiceCollection services,
        Type serviceType,
        Type implementationType
    )
    {
        if (services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType))
            return;

        services.AddSingleton(serviceType, implementationType);
    }

    public static void ReplaceScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.Unregister<TService>();
        services.AddScoped<TService, TImplementation>();
    }

    public static void ReplaceScoped<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory
    )
        where TService : class
    {
        services.Unregister<TService>();
        services.AddScoped(implementationFactory);
    }

    public static void ReplaceTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.Unregister<TService>();
        services.AddTransient<TService, TImplementation>();
    }

    public static void ReplaceTransient<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory
    )
        where TService : class
    {
        services.Unregister<TService>();
        services.AddTransient(implementationFactory);
    }

    public static void ReplaceSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        services.Unregister<TService>();
        services.AddSingleton<TService, TImplementation>();
    }

    public static void ReplaceSingleton<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory
    )
        where TService : class
    {
        services.Unregister<TService>();
        services.AddSingleton(implementationFactory);
    }

    public static void RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class, new()
    {
        var options = new TOptions();
        configuration.Bind(typeof(TOptions).Name, options);

        services.AddSingleton(options);
    }

    public static void RegisterOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string name
    )
        where TOptions : class, new()
    {
        var options = new TOptions();
        configuration.Bind(name, options);

        services.AddSingleton(options);
    }
}
