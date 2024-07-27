using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Core.Movies;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Movies;

public class MoviesMappingTests : IClassFixture<MappingFixture>
{
    private readonly IMapper _mapper;

    public MoviesMappingTests(MappingFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[]
            {
                // these types will instantiate with reflection in the future
                typeof(Movie),
                typeof(MovieDto)
            };
            yield return new object[] { typeof(MovieInfo), typeof(MovieInfoDto) };
            yield return new object[] { typeof(MovieCredit), typeof(MovieCreditDto) };
            yield return new object[] { typeof(MovieCastMember), typeof(MovieCastMemberDto) };
            yield return new object[] { typeof(MovieCrewMember), typeof(MovieCrewMemberDto) };
        }
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination, params object[] parameters)
    {
        var instance = Activator.CreateInstance(source, parameters);

        _mapper.Map(instance, source, destination);
    }
}
