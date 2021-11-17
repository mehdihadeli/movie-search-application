using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Movies;
using MovieSearch.Core.TV;
using Orders.UnitTests.Common;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using Xunit;

namespace MovieSearch.UnitTests.Infrastructure
{
    //https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
    //https://stackoverflow.com/questions/22093843/pass-complex-parameters-to-theory
    public class InfrastructureMappingsTests : IClassFixture<MappingFixture>
    {
        private readonly IMapper _mapper;

        public InfrastructureMappingsTests(MappingFixture fixture)
        {
            _mapper = fixture.Mapper;
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _mapper.ConfigurationProvider
                .AssertConfigurationIsValid();
        }

        [Theory, MemberData(nameof(Data))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination,
            params object[] parameters)
        {
            var instance = Activator.CreateInstance(source, parameters);

            _mapper.Map(instance, source, destination);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[]
                {
                    // these types will instantiate with reflection in the future
                    typeof(SearchMovie), typeof(MovieInfo)
                };
                yield return new object[]
                {
                    typeof(SearchTv), typeof(TVShowInfo)
                };
                yield return new object[]
                {
                    typeof(SearchTv), typeof(TVShowInfo)
                };
                yield return new object[]
                {
                    typeof(SearchCompany), typeof(CompanyInfo)
                };
                yield return new object[]
                {
                    typeof(Genre), typeof(Core.Genres.Genre)
                };
                yield return new object[]
                {
                    typeof(TMDbLib.Objects.Movies.Movie), typeof(Movie)
                };
                yield return new object[]
                {
                    typeof(TvShow), typeof(TVShow)
                };
            }
        }
    }
}