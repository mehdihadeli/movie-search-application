using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Mongo;

public static class Extensions
{
    private const string SectionName = "Mongo";

    public static IServiceCollection AddMongoDbContext<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = SectionName,
        Action<MongoOptions> optionsAction = null
    )
        where TContext : MongoDbContext
    {
        return services.AddMongoDbContext<TContext, TContext>(configuration, sectionName, optionsAction);
    }

    public static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = SectionName,
        Action<MongoOptions> optionsAction = null
    )
        where TContextService : IMongoDbContext
        where TContextImplementation : MongoDbContext, TContextService
    {
        var mongoOptions = configuration.GetSection(sectionName).Get<MongoOptions>() ?? new MongoOptions();
        services.AddSingleton(mongoOptions);

        optionsAction?.Invoke(mongoOptions);

        services.AddScoped(typeof(TContextService), typeof(TContextImplementation));
        services.AddScoped(typeof(TContextImplementation));

        services.AddScoped<IMongoDbContext>(sp => sp.GetService<TContextService>());

        // // transaction doesn't support in standalone mode
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>));

        return services;
    }
}
