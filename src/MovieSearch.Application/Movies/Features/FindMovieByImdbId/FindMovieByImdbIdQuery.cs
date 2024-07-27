using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindMovieByImdbId;

public class FindMovieByImdbIdQuery : IQuery<FindByImdbIdQueryResult>
{
    public string ImdbId { get; init; }

    public class CachePolicy : ICachePolicy<FindMovieByImdbIdQuery, FindByImdbIdQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(FindMovieByImdbIdQuery query)
        {
            return CacheKey.With(query.GetType(), query.ImdbId);
        }
    }
}
