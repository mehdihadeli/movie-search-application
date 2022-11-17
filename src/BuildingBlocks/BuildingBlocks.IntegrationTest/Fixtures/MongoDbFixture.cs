using System;
using BuildingBlocks.Mongo;
using BuildingBlocks.Test.Helpers;
using MongoDB.Driver;

namespace BuildingBlocks.Test.Fixtures;

public class MongoDbFixture : IDisposable
{
    public MongoDbFixture()
    {
        MongoOptions = OptionsHelper.GetOptions<MongoOptions>("mongo");
        MongoOptions.DatabaseName = $"test_db_{Guid.NewGuid()}";
    }

    public MongoOptions MongoOptions { get; }
    public string ConnectionString { get; }

    public void Dispose()
    {
        var client = new MongoClient(MongoOptions.ConnectionString);
        client.DropDatabase(MongoOptions.DatabaseName);
    }
}