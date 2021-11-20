using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Videos.Features.FindTVShowTrailers
{
    public class FindTVShowTrailersQuery : IQuery<FindTVShowTrailersQueryResult>
    {
        public string PageToken { get; init; } = "";
        public int PageSize { get; init; } = 20;
        public int TVShowId { get; init; }

        public class CachePolicy : ICachePolicy<FindTVShowTrailersQuery, FindTVShowTrailersQueryResult>
        {
            public DateTimeOffset? AbsoluteExpirationRelativeToNow => DateTimeOffset.Now.AddMinutes(15);

            public string GetCacheKey(FindTVShowTrailersQuery query)
            {
                return CacheKey.With(query.GetType(),
                    $"{nameof(TVShowId)}_{query.TVShowId}_{nameof(PageSize)}_{query.PageSize}_{nameof(PageToken)}_{query.PageToken}");
            }
        }
    }
}