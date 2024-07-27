using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersByImdbId;

public class FindMovieWithTrailersByImdbIdQuery : IQuery<FindMovieWithTrailersByImdbIdQueryResult>
{
    public FindMovieWithTrailersByImdbIdQuery(string imdbId, int trailersCount = 20)
    {
        ImdbId = imdbId;
        TrailersCount = trailersCount;
    }

    public string ImdbId { get; }
    public int TrailersCount { get; }

    public class CachePolicy
        : ICachePolicy<FindMovieWithTrailersByImdbIdQuery, FindMovieWithTrailersByImdbIdQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(FindMovieWithTrailersByImdbIdQuery query)
        {
            return CacheKey.With(
                query.GetType(),
                $"{nameof(ImdbId)}_{query.ImdbId}_{nameof(TrailersCount)}_{query.TrailersCount}"
            );
        }
    }
}
