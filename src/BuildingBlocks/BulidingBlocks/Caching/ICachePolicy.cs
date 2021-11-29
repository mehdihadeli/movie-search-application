using System;
using System.Linq;
using MediatR;

namespace BuildingBlocks.Caching
{
    public interface ICachePolicy<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        DateTime? AbsoluteExpirationRelativeToNow { get; }

        string GetCacheKey(TRequest request)
        {
            var r = new {request};
            var props = r.request.GetType().GetProperties().Select(pi => $"{pi.Name}:{pi.GetValue(r.request, null)}");
            return $"{typeof(TRequest).FullName}{{{String.Join(",", props)}}}";
        }
    }
}