using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.FindMovieByImdbId;

public class FindByImdbIdQueryResult
{
    public MovieDto Movie { get; init; }
}