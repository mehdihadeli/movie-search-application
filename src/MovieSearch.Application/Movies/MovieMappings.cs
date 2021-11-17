using AutoMapper;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Core.Companies;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;
using MovieSearch.Core.Movies;

namespace MovieSearch.Application.Movies
{
    public class MovieMappings : Profile
    {
        public MovieMappings()
        {
            CreateMap<Movie, MovieDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Language, LanguageDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<ProductionCompany, ProductionCompanyDto>();
        }
    }
}