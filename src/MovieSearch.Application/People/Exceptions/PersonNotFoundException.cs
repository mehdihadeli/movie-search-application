using BuildingBlocks.Exception;

namespace MovieSearch.Application.People.Exceptions;

public class PersonNotFoundException : NotFoundException
{
    public PersonNotFoundException(int id) : base($"can't find a person with id '{id}' in the database.")
    {
    }
}