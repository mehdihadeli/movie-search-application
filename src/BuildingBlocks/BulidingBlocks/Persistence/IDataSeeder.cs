using System.Threading.Tasks;

namespace BuildingBlocks.Persistence;

public interface IDataSeeder
{
    Task SeedAllAsync();
}