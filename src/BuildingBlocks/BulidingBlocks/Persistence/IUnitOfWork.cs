using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task<bool> CommitAsync();
}