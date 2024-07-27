using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BuildingBlocks.Mongo;

public interface IMongoDbContext : IDisposable
{
    IMongoDatabase Database { get; }
    IMongoClient MongoClient { get; }
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    IMongoCollection<T> GetCollection<T>();
}
