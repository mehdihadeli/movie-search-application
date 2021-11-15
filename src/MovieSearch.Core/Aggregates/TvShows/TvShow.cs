using BuildingBlocks.Domain;

namespace MovieSearch.Core.Aggregates.TvShows
{
    public class TvShow : IAggregate
    {
        public TvShow(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}