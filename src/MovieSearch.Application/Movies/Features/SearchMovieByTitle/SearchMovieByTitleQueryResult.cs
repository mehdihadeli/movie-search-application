using BuildingBlocks.Domain;
using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.SearchMovieByTitle;

public class SearchMovieByTitleQueryResult
{
    public ListResultModel<MovieInfoDto> MovieList { get; set; }
}
