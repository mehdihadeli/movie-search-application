using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;

namespace BuildingBlocks.Resiliency.Fallback
{
    /// <summary>
    /// MediatR Fallback Pipeline Behavior
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class FallbackBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IFallbackHandler<TRequest, TResponse>> _fallbackHandlers;
        private readonly ILogger<FallbackBehavior<TRequest, TResponse>> _logger;

        public FallbackBehavior(IEnumerable<IFallbackHandler<TRequest, TResponse>> fallbackHandlers,
            ILogger<FallbackBehavior<TRequest, TResponse>> logger)
        {
            _fallbackHandlers = fallbackHandlers;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var fallbackHandler = _fallbackHandlers.FirstOrDefault();
            if (fallbackHandler == null)
            {
                // No fallback handler found, continue through pipeline
                return await next();
            }

            var fallbackPolicy = Policy<TResponse>
                .Handle<System.Exception>()
                .FallbackAsync(async (cancellationToken) =>
                {
                    _logger.LogDebug(
                        $"Initial handler failed. Falling back to `{fallbackHandler.GetType().FullName}@HandleFallback`");
                    return await fallbackHandler.HandleFallbackAsync(request, cancellationToken)
                        .ConfigureAwait(false);
                });

            var response = await fallbackPolicy.ExecuteAsync(async () => await next());

            return response;
        }
    }
}