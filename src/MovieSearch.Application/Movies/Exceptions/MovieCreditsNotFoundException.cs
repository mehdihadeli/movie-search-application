using BuildingBlocks.Exception;

namespace MovieSearch.Application.Movies.Exceptions
{
    public class MovieCreditsNotFoundException : NotFoundException
    {
        public MovieCreditsNotFoundException(int id) : base(
            $"can't find a movie_credits for a movie with movieId '{id}' in the database.")
        {
        }
    }
}