using AutoMapper;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Core.Movies;

namespace MovieSearch.Application.Movies;

public class MovieMappings : Profile
{
    public MovieMappings()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<MovieInfo, MovieInfoDto>();
        CreateMap<MovieCredit, MovieCreditDto>();
        CreateMap<MovieCastMember, MovieCastMemberDto>();
        CreateMap<MovieCrewMember, MovieCrewMemberDto>();
    }
}