using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.TvShows.Features.SearchTVShow
{
    public class SearchTVShowQuery : IQuery<SearchTVShowQueryResult>
    {
        public SearchTVShowQuery(string searchKeywords, int page = 1, int firstAirDateYear = 0,
            bool includeAdult = false)
        {
            SearchKeywords = searchKeywords;
            Page = page;
            FirstAirDateYear = firstAirDateYear;
            IncludeAdult = includeAdult;
        }

        public int FirstAirDateYear { get; }
        public bool IncludeAdult { get; }
        public string SearchKeywords { get; }
        public int Page { get; }

        public class CachePolicy : ICachePolicy<SearchTVShowQuery, SearchTVShowQueryResult>
        {
            public DateTimeOffset? AbsoluteExpirationRelativeToNow => DateTimeOffset.Now.AddMinutes(15);

            public string GetCacheKey(SearchTVShowQuery query)
            {
                return CacheKey.With(query.GetType(),
                    $"SearchKeywords_{query.SearchKeywords?.ToLower().Trim()}_Page_{query.Page}_IncludeAdult_{query.IncludeAdult.ToString()}_FirstAirDateYear_{query.FirstAirDateYear}");
            }
        }
    }
}