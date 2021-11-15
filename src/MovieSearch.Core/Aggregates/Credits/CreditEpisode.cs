using System;

namespace MovieSearch.Core.Aggregates.Credits
{
    public class CreditEpisode
    {
        public DateTime? AirDate { get; init; }
        public int EpisodeNumber { get; init; }
        public string Name { get; init; }
        public string Overview { get; init; }
        public int SeasonNumber { get; init; }
        public string StillPath { get; init; }
    }
}