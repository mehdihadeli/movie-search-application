using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindMovieCredits;

public class FindMovieCreditsQuery : IQuery<FindMovieCreditsQueryResult>
{
    public FindMovieCreditsQuery(int movieId)
    {
        MovieId = movieId;
    }

    public int MovieId { get; }
}
