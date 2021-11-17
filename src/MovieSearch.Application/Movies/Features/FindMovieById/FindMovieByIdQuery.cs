using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindById
{
    public class FindMovieByIdQuery : IQuery<FindMovieByIdQueryResult>
    {
        public int Id { get; init; }
    }
}