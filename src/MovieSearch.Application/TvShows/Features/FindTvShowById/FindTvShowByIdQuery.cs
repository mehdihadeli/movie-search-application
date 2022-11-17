using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.TvShows.Features.FindTvShowById;

public class FindTvShowByIdQuery : IQuery<FindTvShowByIdQueryResult>
{
    public FindTvShowByIdQuery(int tvShowId)
    {
        TvShowId = tvShowId;
    }

    public int TvShowId { get; }

    public class CachePolicy : ICachePolicy<FindTvShowByIdQuery, FindTvShowByIdQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(FindTvShowByIdQuery query)
        {
            return CacheKey.With(query.GetType(), query.TvShowId.ToString());
        }
    }
}