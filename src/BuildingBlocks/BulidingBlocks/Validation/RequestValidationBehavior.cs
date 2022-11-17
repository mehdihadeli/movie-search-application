using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Validation;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IValidator<TRequest> _validator;

    public RequestValidationBehavior(IServiceProvider serviceProvider,
        ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        ;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken
        cancellationToken)
    {
        _validator = _serviceProvider.GetService<IValidator<TRequest>>();
        if (_validator is null)
            return await next();

        _logger.LogInformation(
            "[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
            nameof(RequestValidationBehavior<TRequest, TResponse>), typeof(TRequest).Name,
            typeof(TResponse).Name);

        _logger.LogDebug("Handling {FullName} with content {Serialize}", typeof(TRequest).FullName,
            JsonSerializer.Serialize(request));

        await _validator.HandleValidationAsync(request);

        var response = await next();

        _logger.LogInformation("Handled {FullName}", typeof(TRequest).FullName);
        return response;
    }
}