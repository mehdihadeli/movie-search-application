using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindUpcomingMovies
{
    public class FindUpcomingMoviesQuery : IQuery<FindUpcomingMoviesQueryResult>
    {
        public int Page { get; init; }
    }
}