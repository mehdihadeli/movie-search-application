using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Mongo;
using BuildingBlocks.Test.Factories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;
using Xunit.Abstractions;

namespace BuildingBlocks.Test.Fixtures;

public class IntegrationTestFixture<TEntryPoint, TDbContext> : IntegrationTestFixture<TEntryPoint>
    where TEntryPoint : class
    where TDbContext : class, IMongoDbContext
{
    // protected readonly Mongo2GoFixture MongoDbFixture;
    protected readonly MongoDbFixture MongoDbFixture;

    public IntegrationTestFixture()
    {
        MongoDbFixture = new MongoDbFixture();
        var mongoOptions = ServiceProvider.GetService<MongoOptions>();
        if (mongoOptions is { })
            mongoOptions.DatabaseName = MongoDbFixture.MongoOptions.DatabaseName;

        // MongoDbFixture = new Mongo2GoFixture();
        // var mongoOptions = ServiceProvider.GetService<MongoOptions>();
        // if (mongoOptions is { })
        //     mongoOptions.ConnectionString = MongoDbFixture.MongoOptions.ConnectionString;
    }

    private async Task ResetState()
    {
        try
        {
            var mongoOptions = ServiceProvider.GetService<MongoOptions>();
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await dbContext.MongoClient.DropDatabaseAsync(mongoOptions?.DatabaseName);
        }
        catch
        {
            // ignored
        }
    }

    public Task ExecuteDbContextAsync(Func<TDbContext, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TDbContext>()));
    }

    public Task ExecuteDbContextAsync(Func<TDbContext, ValueTask> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TDbContext>()).AsTask());
    }

    public Task ExecuteDbContextAsync(Func<TDbContext, IMediator, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TDbContext>(), sp.GetService<IMediator>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TDbContext, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TDbContext>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TDbContext, ValueTask<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TDbContext>()).AsTask());
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TDbContext, IMediator, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TDbContext>(), sp.GetService<IMediator>()));
    }

    public Task InsertAsync<T>(params T[] entities) where T : class, IAggregate
    {
        return ExecuteDbContextAsync(db => db.GetCollection<T>().InsertManyAsync(entities));
    }

    public Task InsertAsync<TEntity>(TEntity entity) where TEntity : class, IAggregate
    {
        return ExecuteDbContextAsync(db => db.GetCollection<TEntity>().InsertOneAsync(entity));
    }

    public Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
        where TEntity : class, IAggregate
        where TEntity2 : class, IAggregate
    {
        return ExecuteDbContextAsync(db =>
        {
            db.GetCollection<TEntity>().InsertOneAsync(entity);
            db.GetCollection<TEntity2>().InsertOneAsync(entity2);
            return Task.CompletedTask;
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
        where TEntity : class, IAggregate
        where TEntity2 : class, IAggregate
        where TEntity3 : class, IAggregate
    {
        return ExecuteDbContextAsync(db =>
        {
            db.GetCollection<TEntity>().InsertOneAsync(entity);
            db.GetCollection<TEntity2>().InsertOneAsync(entity2);
            db.GetCollection<TEntity3>().InsertOneAsync(entity3);
            return Task.CompletedTask;
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2,
        TEntity3 entity3, TEntity4 entity4)
        where TEntity : class, IAggregate
        where TEntity2 : class, IAggregate
        where TEntity3 : class, IAggregate
        where TEntity4 : class, IAggregate
    {
        return ExecuteDbContextAsync(db =>
        {
            db.GetCollection<TEntity>().InsertOneAsync(entity);
            db.GetCollection<TEntity2>().InsertOneAsync(entity2);
            db.GetCollection<TEntity3>().InsertOneAsync(entity3);
            db.GetCollection<TEntity4>().InsertOneAsync(entity4);
            return Task.CompletedTask;
        });
    }

    public Task<T> FindAsync<T>(int id) where T : class, IAggregate<int>
    {
        return ExecuteDbContextAsync(db =>
            db.GetCollection<T>().AsQueryable().SingleOrDefaultAsync(x => x.Id == id));
    }

    public override Task InitializeAsync()
    {
        return base.InitializeAsync();
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await ResetState();
    }
}

public class IntegrationTestFixture<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    protected readonly CustomApplicationFactory<TEntryPoint> Factory;

    public IntegrationTestFixture()
    {
        Factory = new CustomApplicationFactory<TEntryPoint>();
    }
    // protected readonly LaunchSettingsFixture LaunchSettings;

    protected HttpClient Client => Factory.CreateClient();


    public IHttpClientFactory HttpClientFactory =>
        ServiceProvider.GetRequiredService<IHttpClientFactory>();

    public IHttpContextAccessor HttpContextAccessor =>
        ServiceProvider.GetRequiredService<IHttpContextAccessor>();

    public IServiceProvider ServiceProvider => Factory.Services;
    public IConfiguration Configuration => Factory.Configuration;

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        Factory?.Dispose();
        return Task.CompletedTask;
    }

    public void SetOutput(ITestOutputHelper output)
    {
        Factory.OutputHelper = output;
        Factory.Server.AllowSynchronousIO = true;
    }

    public void RegisterTestServices(Action<IServiceCollection> services)
    {
        Factory.TestRegistrationServices = services;
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = ServiceProvider.CreateScope();

        await action(scope.ServiceProvider);
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = ServiceProvider.CreateScope();

        var result = await action(scope.ServiceProvider);

        return result;
    }

    public Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, INotification
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Publish(@event, cancellationToken);
        });
    }

    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request, cancellationToken);
        });
    }

    public Task SendAsync<T>(T request, CancellationToken cancellationToken) where T : class, ICommand
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request, cancellationToken);
        });
    }

    public Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
        where TResponse : class
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(query, cancellationToken);
        });
    }
}