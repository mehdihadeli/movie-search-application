using System.Collections.Generic;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Videos.Dtos;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersByImdbId
{
    public class FindMovieWithTrailersByImdbIdQueryResult
    {
        public MovieDto Movie { get; init; }
        public IEnumerable<VideoDto> Trailers { get; init; }
    }
}