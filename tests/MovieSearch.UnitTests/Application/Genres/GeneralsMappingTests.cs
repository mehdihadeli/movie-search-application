using AutoMapper;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Genres;

public class GeneralsMappingTests : IClassFixture<MappingFixture>
{
    private readonly IMapper _mapper;

    public GeneralsMappingTests(MappingFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _mapper.ConfigurationProvider
            .AssertConfigurationIsValid();
    }
}