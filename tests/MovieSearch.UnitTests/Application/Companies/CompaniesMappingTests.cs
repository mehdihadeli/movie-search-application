using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Application.Companies.Dtos;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Movies;
using MovieSearch.Core.TV;
using Orders.UnitTests.Common;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using Xunit;

namespace MovieSearch.UnitTests.Application.Companies
{
    public class CompaniesMappingTests: IClassFixture<MappingFixture>
    {
        private readonly IMapper _mapper;

        public CompaniesMappingTests(MappingFixture fixture)
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
                    typeof(ProductionCompany), typeof(ProductionCompanyDto)
                };
            }
        }
    }
}