using AutoMapper;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Videos;

public class VideoMappingTests : IClassFixture<MappingFixture>
{
    private readonly IMapper _mapper;

    public VideoMappingTests(MappingFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
