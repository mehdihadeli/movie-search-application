using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindById
{
    public class FindMovieByIdQuery : IQuery<FindMovieByIdQueryResult>
    {
        public int Id { get; init; }

        public class CachePolicy : ICachePolicy<FindMovieByIdQuery, FindMovieByIdQueryResult>
        {
            public DateTimeOffset? AbsoluteExpirationRelativeToNow => DateTimeOffset.Now.AddMinutes(15);

            public string GetCacheKey(FindMovieByIdQuery query)
            {
                return CacheKey.With(query.GetType(), query.Id.ToString());
            }
        }
    }
}