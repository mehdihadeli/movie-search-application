using BuildingBlocks.Domain;

namespace MovieSearch.Application.People.Features.FindPersonTVShowCredits
{
    public class FindPersonTVShowCreditsQuery : IQuery<FindPersonTVShowCreditsQueryResult>
    {
        public FindPersonTVShowCreditsQuery(int personId)
        {
            PersonId = personId;
        }

        public int PersonId { get; }
    }
}