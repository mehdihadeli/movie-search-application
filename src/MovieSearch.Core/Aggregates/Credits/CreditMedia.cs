using System.Collections.Generic;

namespace MovieSearch.Core.Aggregates.Credits
{
    public class CreditMedia
    {
        public string Character { get; init; }
        public List<CreditEpisode> Episodes { get; init; }
        public int Id { get; init; }
        public string Name { get; init; }
        public string OriginalName { get; init; }
        public List<CreditSeason> Seasons { get; init; }
    }
}