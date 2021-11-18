using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindPopularMovies
{
    public class FindPopularMoviesQuery : IQuery<FindPopularMoviesQueryResult>
    {
        public int Page { get; init; } = 1;
    }
}