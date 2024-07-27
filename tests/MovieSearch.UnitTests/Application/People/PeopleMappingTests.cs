using AutoMapper;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.People;

public class PeopleMappingTests : IClassFixture<MappingFixture>
{
    private readonly IMapper _mapper;

    public PeopleMappingTests(MappingFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
