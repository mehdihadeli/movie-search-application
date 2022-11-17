using BuildingBlocks.Exception;

namespace MovieSearch.Application.Movies.Exceptions;

public class MovieNotFoundException : NotFoundException
{
    public MovieNotFoundException(int id) : base($"can't find movie with id '{id}' in the database.")
    {
    }

    public MovieNotFoundException(string imdbId) : base($"can't find movie with imdbid '{imdbId}' in the database.")
    {
    }
}