using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Videos.FindTrailers
{
    public class FindTrailersQuery : IQuery<FindTrailersQueryResult>
    {
        public string MovieName { get; init; }
        public string PageToken { get; init; } = "";
        public int PageSize { get; init; } = 20;

        public class CachePolicy : ICachePolicy<FindTrailersQuery, FindTrailersQueryResult>
        {
            public DateTimeOffset? AbsoluteExpirationRelativeToNow => DateTimeOffset.Now.AddMinutes(15);

            public string GetCacheKey(FindTrailersQuery query)
            {
                return CacheKey.With(query.GetType(),
                    $"{nameof(MovieName)}_{query.MovieName?.ToLower().Trim()}_{nameof(PageSize)}_{query.PageSize}_{nameof(PageToken)}_{query.PageToken}");
            }
        }
    }
}