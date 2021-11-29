using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Videos.Features.FindMovieTrailers
{
    public class FindMovieTrailersQuery : IQuery<FindMovieTrailersQueryResult>
    {
        public string PageToken { get; init; } = "";
        public int PageSize { get; init; } = 20;
        public int MovieId { get; init; }

        public class CachePolicy : ICachePolicy<FindMovieTrailersQuery, FindMovieTrailersQueryResult>
        {
            public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

            public string GetCacheKey(FindMovieTrailersQuery query)
            {
                return CacheKey.With(query.GetType(),
                    $"{nameof(MovieId)}_{query.MovieId}_{nameof(PageSize)}_{query.PageSize}_{nameof(PageToken)}_{query.PageToken}");
            }
        }
    }
}