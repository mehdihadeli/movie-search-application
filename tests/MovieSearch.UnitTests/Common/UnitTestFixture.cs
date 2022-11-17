using System;
using AutoMapper;

namespace Orders.UnitTests.Common;

public class UnitTestFixture : IDisposable
{
    public UnitTestFixture()
    {
        Mapper = MapperFactory.Create();
        //Context = DbContextFactory.Create().GetAwaiter().GetResult();
    }

    public IMapper Mapper { get; }
    // public ApplicationDbContext Context { get; }

    public void Dispose()
    {
        //DbContextFactory.Destroy(Context).GetAwaiter().GetResult();
    }
}