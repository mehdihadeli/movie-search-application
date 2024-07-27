using AutoMapper;
using MovieSearch.Application.Generals.Dtos;
using MovieSearch.Core.Generals;

namespace MovieSearch.Application.Generals;

public class GeneralMappings : Profile
{
    public GeneralMappings()
    {
        CreateMap<Language, LanguageDto>();
        CreateMap<Country, CountryDto>();
        CreateMap<MultiInfo, MultiInfoDto>();
    }
}
