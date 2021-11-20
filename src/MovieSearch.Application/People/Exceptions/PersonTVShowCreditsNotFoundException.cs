using BuildingBlocks.Exception;

namespace MovieSearch.Application.People.Exceptions
{
    public class PersonTVShowCreditsNotFoundException : NotFoundException
    {
        public PersonTVShowCreditsNotFoundException(int personId) : base($"can't find a person_tvshow_credits for a person with personId '{personId}' in the database.")
        {
        }
    }
}