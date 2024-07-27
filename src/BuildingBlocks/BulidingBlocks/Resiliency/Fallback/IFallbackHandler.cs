using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.Resiliency.Fallback;

public interface IFallbackHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleFallbackAsync(TRequest request, CancellationToken cancellationToken);
}
