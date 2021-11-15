using System;

namespace MovieSearch.Core.Aggregates.Credits
{
    public class CreditSeason
    {
        public DateTime? AirDate { get; set; }
        public string PosterPath { get; set; }
        public int SeasonNumber { get; set; }
    }
}