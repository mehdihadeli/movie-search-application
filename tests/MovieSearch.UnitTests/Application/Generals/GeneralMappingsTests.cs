using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Application.Companies.Dtos;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Generals
{
    public class GeneralMappingsTests : IClassFixture<MappingFixture>
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
    }
}