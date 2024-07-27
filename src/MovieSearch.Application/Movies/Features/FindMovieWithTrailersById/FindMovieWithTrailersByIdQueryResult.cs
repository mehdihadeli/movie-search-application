using MovieSearch.Application.Movies.Dtos;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;

public class FindMovieWithTrailersByIdQueryResult
{
    public FindMovieWithTrailersByIdQueryResult(MovieWithTrailersDto movieWithTrailers)
    {
        MovieWithTrailers = movieWithTrailers;
    }

    public MovieWithTrailersDto MovieWithTrailers { get; }
}
