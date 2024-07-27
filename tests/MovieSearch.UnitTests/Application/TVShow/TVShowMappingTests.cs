using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Core.TV;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.TVShow;

public class TVShowMappingTests : IClassFixture<MappingFixture>
{
    private readonly IMapper _mapper;

    public TVShowMappingTests(MappingFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    public static IEnumerable<object[]> Data
    {
        get
        {
            yield return new object[] { typeof(Core.TV.TVShow), typeof(TVShowDto) };
            yield return new object[] { typeof(TVShowInfo), typeof(TVShowInfoDto) };
            yield return new object[] { typeof(TVShowCredit), typeof(TVShowCreditDto) };
            yield return new object[] { typeof(TVShowCastMember), typeof(TVShowCastMemberDto) };
            yield return new object[] { typeof(TVShowCrewMember), typeof(TVShowCrewMemberDto) };
            yield return new object[] { typeof(TVShowCreator), typeof(TVShowCreatorDto) };
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
