using BuildingBlocks.Domain;
using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.FindPopularMovies
{
    public class FindPopularMoviesQueryResult
    {
        public ListResultModel<MovieInfoDto> MovieList { get; set; }
    }
}