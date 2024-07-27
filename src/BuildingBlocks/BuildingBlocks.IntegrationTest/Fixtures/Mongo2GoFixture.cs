using System;
using BuildingBlocks.Mongo;
using BuildingBlocks.Test.Helpers;
using Mongo2Go;
using MongoDB.Driver;

namespace BuildingBlocks.Test.Fixtures;

public class Mongo2GoFixture : IDisposable
{
    private readonly MongoDbRunner _mongoRunner;

    public Mongo2GoFixture()
    {
        // initializes the instance
        _mongoRunner = MongoDbRunner.Start();

        // store the connection string with the chosen port number
        ConnectionString = _mongoRunner.ConnectionString;
        MongoOptions = OptionsHelper.GetOptions<MongoOptions>("mongo");
        MongoOptions.ConnectionString = _mongoRunner.ConnectionString;
        // create a client and database for use outside the class
        Client = new MongoClient(ConnectionString);

        Database = Client.GetDatabase(MongoOptions.DatabaseName);
    }

    public MongoClient Client { get; }
    public IMongoDatabase Database { get; }
    public MongoOptions MongoOptions { get; }
    public string ConnectionString { get; }

    public void Dispose()
    {
        _mongoRunner.Dispose();
    }
}
