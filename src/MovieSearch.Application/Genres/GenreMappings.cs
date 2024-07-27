using AutoMapper;
using MovieSearch.Application.Genres.Dtos;
using MovieSearch.Core.Genres;

namespace MovieSearch.Application.Genres;

public class GenreMappings : Profile
{
    public GenreMappings()
    {
        CreateMap<Genre, GenreDto>();
    }
}
