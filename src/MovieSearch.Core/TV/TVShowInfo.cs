using System;
using System.Collections.Generic;
using MovieSearch.Core.Generals;
using MovieSearch.Core.Genres;

namespace MovieSearch.Core.TV
{
    public class TVShowInfo : MultiInfo
    {
        public string Name { get; init; }
        public string OriginalName { get; init; }
        public string PosterPath { get; init; }
        public string BackdropPath { get; init; }
        public double VoteAverage { get; init; }
        public int VoteCount { get; init; }
        public string Overview { get; init; }
        public DateTime? FirstAirDate { get; init; }
        public IReadOnlyList<string> OriginCountry { get; init; }
        public IReadOnlyList<int> GenreIds { get; init; }
        public string OriginalLanguage { get; init; }
        public override MediaType MediaType { get; init; } = MediaType.Tv;

        public TVShowInfo()
        {
            OriginCountry = Array.Empty<string>();
            GenreIds = Array.Empty<int>();
        }

        public override string ToString()
            => $"{Name} ({Id} - {FirstAirDate:yyyy-MM-dd})";
    }
}