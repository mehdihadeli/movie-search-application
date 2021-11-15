using BuildingBlocks.Domain;

namespace MovieSearch.Core.Aggregates.Reviews
{
    public class Review : IAggregate<string>
    {
        public string Id { get; init; }
        public string Author { get; init; }
        public string Content { get; init; }
        public string Url { get; init; }
        public string Iso_639_1 { get; init; }
        public int MediaId { get; init; }
        public string MediaTitle { get; init; }
        public MediaType MediaType { get; init; }
    }
}