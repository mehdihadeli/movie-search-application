using System;
using System.Collections.Generic;
using MovieSearch.Core.Generals;

namespace MovieSearch.Core.TV;

public class TVShowInfo : MultiInfo
{
    public TVShowInfo()
    {
        OriginCountry = Array.Empty<string>();
        GenreIds = Array.Empty<int>();
    }

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

    public override string ToString()
    {
        return $"{Name} ({Id} - {FirstAirDate:yyyy-MM-dd})";
    }
}
