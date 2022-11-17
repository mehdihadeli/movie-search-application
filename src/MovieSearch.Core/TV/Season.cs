using System;

namespace MovieSearch.Core.TV;

public class Season
{
    public int Id { get; init; }
    public DateTime? AirDate { get; init; }
    public int EpisodeCount { get; init; }
    public string PosterPath { get; init; }
    public string Overview { get; init; }
    public int SeasonNumber { get; init; }

    public override string ToString()
    {
        return $"({SeasonNumber} - {AirDate:yyyy-MM-dd})";
    }
}