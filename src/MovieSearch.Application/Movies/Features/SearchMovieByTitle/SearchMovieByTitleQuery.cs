using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.SearchMovieByTitle
{
    public class SearchMovieByTitleQuery : IQuery<SearchMovieByTitleQueryResult>
    {
        public string SearchKeywords { get; init; }
        public int Page { get; init; } = 1;

        public class CachePolicy : ICachePolicy<SearchMovieByTitleQuery, SearchMovieByTitleQueryResult>
        {
            public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

            public string GetCacheKey(SearchMovieByTitleQuery query)
            {
                return CacheKey.With(query.GetType(), $"SearchKeywords_{query.SearchKeywords?.ToLower().Trim()}_Page_{query.Page}");
            }
        }
    }
}