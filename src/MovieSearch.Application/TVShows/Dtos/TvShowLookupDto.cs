using System;
using System.Collections.Generic;

namespace MovieSearch.Application.TVShows.Dtos
{
    public class TvShowLookupDto
    {
        public DateTime? FirstAirDate { get; init; }
        public string Name { get; init; }
        public string OriginalName { get; init; }
        public List<string> OriginCountry { get; init; }
        public string BackdropPath { get; init; }
        public List<int> GenreIds { get; init; }
        public string OriginalLanguage { get; init; }
        public string Overview { get; init; }
        public string PosterPath { get; init; }
        public double VoteAverage { get; init; }
        public int VoteCount { get; init; }
        public int Id { get; init; }
        public double Popularity { get; init; }
    }
}