using System;

namespace MovieSearch.Application.Videos.Dtos;

public class VideoDto
{
    public string Id { get; init; }
    public string Iso_3166_1 { get; init; }
    public string Iso_639_1 { get; init; }
    public string Key { get; init; }
    public string Name { get; init; }
    public string Site { get; init; }
    public int Size { get; init; }
    public string Type { get; init; }
    public DateTime? PublishedAt { get; init; }
}