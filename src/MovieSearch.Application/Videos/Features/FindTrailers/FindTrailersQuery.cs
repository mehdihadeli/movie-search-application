using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Videos.Features.FindTrailers;

public class FindTrailersQuery : IQuery<FindTrailersQueryResult>
{
    public string MovieName { get; init; }
    public string PageToken { get; init; } = "";
    public int PageSize { get; init; } = 20;
    public DateTime? PublishedBefore { get; init; }
    public DateTime? PublishedAfter { get; init; }

    public class CachePolicy : ICachePolicy<FindTrailersQuery, FindTrailersQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(FindTrailersQuery query)
        {
            return CacheKey.With(query.GetType(),
                $"{nameof(MovieName)}_{query.MovieName?.ToLower().Trim()}_{nameof(PageSize)}_{query.PageSize}_{nameof(PageToken)}_{query.PageToken}_{nameof(PublishedBefore)}_{query.PublishedBefore}_{nameof(PublishedAfter)}_{query.PublishedAfter}");
        }
    }
}