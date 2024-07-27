using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersByImdbId;

public class FindMovieWithTrailersByImdbIdQueryResult
{
    public MovieWithTrailersDto MovieWithTrailers { get; init; }
}
