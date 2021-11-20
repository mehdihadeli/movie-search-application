using System.Collections.Generic;
using MovieSearch.Application.TvShows.Dtos;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById
{
    public class FindTVShowWithTrailersByIdQueryResult
    {
        public TVShowWithTrailersDto TVShowWithTrailers { get; init; }
    }
}