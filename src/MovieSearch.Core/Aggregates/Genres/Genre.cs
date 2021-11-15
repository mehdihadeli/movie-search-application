using BuildingBlocks.Domain;

namespace MovieSearch.Core.Aggregates.Genres
{
    public class Genre : IAggregate
    {
        public Genre(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}