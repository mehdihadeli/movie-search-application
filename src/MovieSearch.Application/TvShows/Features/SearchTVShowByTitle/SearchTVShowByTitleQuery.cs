using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.TvShows.Features.SearchTVShowByTitle;

public class SearchTVShowByTitleQuery : IQuery<SearchTVShowByTitleQueryResult>
{
    public SearchTVShowByTitleQuery(string searchKeywords, int page = 1)
    {
        SearchKeywords = searchKeywords;
        Page = page;
    }

    public string SearchKeywords { get; }
    public int Page { get; }

    public class CachePolicy : ICachePolicy<SearchTVShowByTitleQuery, SearchTVShowByTitleQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(SearchTVShowByTitleQuery query)
        {
            return CacheKey.With(
                query.GetType(),
                $"SearchKeywords_{query.SearchKeywords?.ToLower().Trim()}_Page_{query.Page}"
            );
        }
    }
}
