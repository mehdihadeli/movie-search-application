using System.Collections.Generic;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Movies.Dtos;

public class MovieWithTrailersDto
{
    public MovieDto Movie { get; init; }
    public IEnumerable<VideoDto> Trailers { get; init; }
}