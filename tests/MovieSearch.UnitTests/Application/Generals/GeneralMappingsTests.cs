using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Application.Companies.Dtos;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Generals
{
    public class GeneralMappingsTests: IClassFixture<MappingFixture>
    {
        private readonly IMapper _mapper;

        public GeneralMappingsTests(MappingFixture fixture)
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
                };
            }
        }
    }
}