using BuildingBlocks.Domain;

namespace MovieSearch.Application.People.Features.FindPersonMovieCredits;

public class FindPersonMovieCreditsQuery : IQuery<FindPersonMovieCreditsQueryResult>
{
    public FindPersonMovieCreditsQuery(int personId)
    {
        PersonId = personId;
    }

    public int PersonId { get; }
}