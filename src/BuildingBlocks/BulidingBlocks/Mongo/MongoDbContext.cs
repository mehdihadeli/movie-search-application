using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace BuildingBlocks.Mongo;

public class MongoDbContext : IMongoDbContext
{
    private readonly string _databaseName;

    static MongoDbContext()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
    }

    public MongoDbContext(MongoOptions options)
    {
        RegisterConventions();

        MongoClient = new MongoClient(options.ConnectionString);
        _databaseName = options.DatabaseName;
        Database = MongoClient.GetDatabase(_databaseName);
    }

    public IClientSessionHandle Session { get; set; }
    public IMongoDatabase Database { get; }
    public IMongoClient MongoClient { get; }

    public IMongoCollection<T> GetCollection<T>()
    {
        return Database.GetCollection<T>(typeof(T).Name.ToLower());
    }

    public void Dispose()
    {
        while (Session != null && Session.IsInTransaction)
            Thread.Sleep(TimeSpan.FromMilliseconds(100));

        GC.SuppressFinalize(this);
    }

    public async Task BeginTransactionAsync()
    {
        Session = await MongoClient.StartSessionAsync();
        Session.StartTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        if (Session.IsInTransaction)
            await Session.CommitTransactionAsync();

        Session.Dispose();
    }

    public async Task RollbackTransactionAsync()
    {
        await Session.AbortTransactionAsync();
    }

    private static void RegisterConventions()
    {
        ConventionRegistry.Register(
            "conventions",
            new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreIfDefaultConvention(true),
                new ImmutablePocoConvention()
            },
            _ => true
        );
    }
}
