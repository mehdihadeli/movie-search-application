using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly int defaultCacheExpirationInHours = 1;
        private readonly IEnumerable<ICachePolicy<TRequest, TResponse>> _cachePolicies;


        public CachingBehavior(IEasyCachingProviderFactory cachingFactory,
            ILogger<CachingBehavior<TRequest, TResponse>> logger,
            IEnumerable<ICachePolicy<TRequest, TResponse>> cachePolicies)
        {
            _logger = logger;
            _cachingProvider = cachingFactory.GetCachingProvider("mem");
            _cachePolicies = cachePolicies;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var cachePolicy = _cachePolicies.FirstOrDefault();
            if (cachePolicy == null)
            {
                // No cache policy found, so just continue through the pipeline
                return await next();
            }

            var cacheKey = cachePolicy.GetCacheKey(request);
            var cachedResponse = await _cachingProvider.GetAsync<TResponse>(cacheKey);
            if (cachedResponse.Value != null)
            {
                _logger.LogDebug("Response retrieved {TRequest} from cache. CacheKey: {CacheKey}",
                    typeof(TRequest).FullName, cacheKey);
                return cachedResponse.Value;
            }

            var response = await next();

            var time = cachePolicy.AbsoluteExpirationRelativeToNow ??
                       DateTime.Now.AddHours(defaultCacheExpirationInHours);
            await _cachingProvider.SetAsync(cacheKey, response, time.TimeOfDay);

            _logger.LogDebug("Caching response for {TRequest} with cache key: {CacheKey}", typeof(TRequest).FullName,
                cacheKey);

            return response;
        }
    }
}