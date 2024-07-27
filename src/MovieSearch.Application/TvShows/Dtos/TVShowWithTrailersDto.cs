using System.Collections.Generic;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.TvShows.Dtos;

public class TVShowWithTrailersDto
{
    public TVShowDto TVShow { get; init; }
    public IEnumerable<VideoDto> Trailers { get; init; }
}
