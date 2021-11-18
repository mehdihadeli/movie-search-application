using BuildingBlocks.Domain;
using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.SearchMovie
{
    public class SearchMovieQueryResult
    {
        public ListResultModel<MovieInfoDto> MovieList { get; set; }
    }
}