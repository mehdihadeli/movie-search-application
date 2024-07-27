using System;
using System.Collections.Generic;
using AutoMapper;
using MovieSearch.Application.Companies.Dtos;
using MovieSearch.Core.Companies;
using Orders.UnitTests.Common;
using Xunit;

namespace MovieSearch.UnitTests.Application.Companies;

public class CompaniesMappingTests : IClassFixture<MappingFixture>
{
    private readonly IMapper _mapper;

    public CompaniesMappingTests(MappingFixture fixture)
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
                typeof(ProductionCompany),
                typeof(ProductionCompanyDto)
            };
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
