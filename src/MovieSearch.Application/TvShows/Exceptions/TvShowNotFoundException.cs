using BuildingBlocks.Exception;

namespace MovieSearch.Application.TvShows.Exceptions
{
    public class TvShowNotFoundException : NotFoundException
    {
        public TvShowNotFoundException(int id) : base($"can't find a tv-show with id '{id}' in the database.")
        {
        }
    }
}