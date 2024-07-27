using BuildingBlocks.Domain;
using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.FindUpcomingMovies;

public class FindUpcomingMoviesQueryResult
{
    public ListResultModel<MovieInfoDto> MovieList { get; set; }
}
