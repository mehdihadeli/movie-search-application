using BuildingBlocks.Exception;

namespace MovieSearch.Application.People.Exceptions;

public class PersonMovieCreditsNotFoundException : NotFoundException
{
    public PersonMovieCreditsNotFoundException(int personId) : base(
        $"can't find a person_movie_credits for a person with personId '{personId}' in the database.")
    {
    }
}