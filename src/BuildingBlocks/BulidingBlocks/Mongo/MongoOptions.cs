using System;

namespace BuildingBlocks.Mongo;

public class MongoOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public static Guid UniqueId { get; set; } = Guid.NewGuid();
}
