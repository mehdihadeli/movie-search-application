using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersById
{
    public class FindMovieWithTrailersByIdQuery : IQuery<FindMovieWithTrailersByIdQueryResult>
    {
        public FindMovieWithTrailersByIdQuery(int movieId, int trailersCount = 20)
        {
            MovieId = movieId;
            TrailersCount = trailersCount;
        }

        public int MovieId { get; }
        public int TrailersCount { get; }

        public class CachePolicy : ICachePolicy<FindMovieWithTrailersByIdQuery, FindMovieWithTrailersByIdQueryResult>
        {
            public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

            public string GetCacheKey(FindMovieWithTrailersByIdQuery query)
            {
                return CacheKey.With(query.GetType(),
                    $"{nameof(MovieId)}_{query.MovieId}_{nameof(TrailersCount)}_{query.TrailersCount}");
            }
        }
    }
}