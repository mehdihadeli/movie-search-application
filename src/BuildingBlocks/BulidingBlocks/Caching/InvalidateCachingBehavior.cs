using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Caching;

public class InvalidateCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IEnumerable<IInvalidateCachePolicy<TRequest, TResponse>> _invalidateCachePolicies;
    private readonly ILogger<InvalidateCachingBehavior<TRequest, TResponse>> _logger;


    public InvalidateCachingBehavior(IEasyCachingProviderFactory cachingFactory,
        ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger,
        IEnumerable<IInvalidateCachePolicy<TRequest, TResponse>> invalidateCachingPolicies)
    {
        _logger = logger;
        _cachingProvider = cachingFactory.GetCachingProvider("mem");
        _invalidateCachePolicies = invalidateCachingPolicies;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken
        cancellationToken)
    {
        var cachePolicy = _invalidateCachePolicies.FirstOrDefault();
        if (cachePolicy == null)
            // No cache policy found, so just continue through the pipeline
            return await next();

        var cacheKey = cachePolicy.GetCacheKey(request);
        var response = await next();

        await _cachingProvider.RemoveAsync(cacheKey, cancellationToken);

        _logger.LogDebug("Cache data with cache key: {CacheKey} removed", cacheKey);

        return response;
    }
}