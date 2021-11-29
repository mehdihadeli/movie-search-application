using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById
{
    public class FindTVShowWithTrailersByIdQuery : IQuery<FindTVShowWithTrailersByIdQueryResult>
    {
        public FindTVShowWithTrailersByIdQuery(int tvShowId, int trailersCount)
        {
            TvShowId = tvShowId;
            TrailersCount = trailersCount;
        }

        public int TvShowId { get; }
        public int TrailersCount { get; }

        public class CachePolicy : ICachePolicy<FindTVShowWithTrailersByIdQuery, FindTVShowWithTrailersByIdQueryResult>
        {
            public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

            public string GetCacheKey(FindTVShowWithTrailersByIdQuery query)
            {
                return CacheKey.With(query.GetType(),
                    $"{nameof(TvShowId)}_{query.TvShowId}_{nameof(TrailersCount)}_{query.TrailersCount}");
            }
        }
    }
}