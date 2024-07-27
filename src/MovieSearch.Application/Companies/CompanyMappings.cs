using AutoMapper;
using MovieSearch.Application.Companies.Dtos;
using MovieSearch.Core.Companies;

namespace MovieSearch.Application.Companies;

public class CompanyMappings : Profile
{
    public CompanyMappings()
    {
        CreateMap<ProductionCompany, ProductionCompanyDto>();
    }
}
