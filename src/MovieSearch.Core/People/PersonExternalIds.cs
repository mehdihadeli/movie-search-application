using MovieSearch.Core.Generals;

namespace MovieSearch.Core.People
{
    public class PersonExternalIds : ExternalIds
    {
        public string FacebookId { get; init; }
        public string ImdbId { get; init; }
        public string TwitterId { get; init; }
        public string InstagramId { get; init; }
    }
}