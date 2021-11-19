using System.Collections.Generic;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById
{
    public class FindTVShowWithTrailersByIdQueryResult
    {
        public TVShowDto TVShow { get; init; }
        public IEnumerable<VideoDto> Trailers { get; init; }
    }
}