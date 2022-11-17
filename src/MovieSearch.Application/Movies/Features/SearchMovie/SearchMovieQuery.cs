using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.SearchMovie;

public class SearchMovieQuery : IQuery<SearchMovieQueryResult>
{
    public SearchMovieQuery(string searchKeywords, int page = 1, int year = 0, int primaryReleaseYear = 0,
        bool includeAdult = false)
    {
        SearchKeywords = searchKeywords;
        Page = page;
        Year = year;
        PrimaryReleaseYear = primaryReleaseYear;
        IncludeAdult = includeAdult;
    }

    public int Year { get; }
    public int PrimaryReleaseYear { get; }
    public bool IncludeAdult { get; }
    public string SearchKeywords { get; }
    public int Page { get; }


    public class CachePolicy : ICachePolicy<SearchMovieQuery, SearchMovieQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(SearchMovieQuery query)
        {
            return CacheKey.With(query.GetType(),
                $"SearchKeywords_{query.SearchKeywords?.ToLower().Trim()}_Page_{query.Page}_IncludeAdult_{query.IncludeAdult.ToString()}_PrimaryReleaseYear_{query.PrimaryReleaseYear}_Year_{query.Year}");
        }
    }
}