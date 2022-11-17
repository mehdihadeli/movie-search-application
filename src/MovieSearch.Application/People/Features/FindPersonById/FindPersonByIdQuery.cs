using System;
using BuildingBlocks.Caching;
using BuildingBlocks.Domain;

namespace MovieSearch.Application.People.Features.FindPersonById;

public class FindPersonByIdQuery : IQuery<FindPersonByIdQueryResult>
{
    public FindPersonByIdQuery(int personId)
    {
        PersonId = personId;
    }

    public int PersonId { get; }

    public class CachePolicy : ICachePolicy<FindPersonByIdQuery, FindPersonByIdQueryResult>
    {
        public DateTime? AbsoluteExpirationRelativeToNow => DateTime.Now.AddMinutes(15);

        public string GetCacheKey(FindPersonByIdQuery query)
        {
            return CacheKey.With(query.GetType(), query.PersonId.ToString());
        }
    }
}