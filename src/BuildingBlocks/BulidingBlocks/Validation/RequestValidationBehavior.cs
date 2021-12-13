using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace BuildingBlocks.Validation
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private IValidator<TRequest> _validator;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger;

        public RequestValidationBehavior(IServiceProvider serviceProvider,
            ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _validator = _serviceProvider.GetService<IValidator<TRequest>>();
            if (_validator is null)
               return await next();

            _logger.LogInformation(
                "[{Prefix}] Handle request={X-RequestData} and response={X-ResponseData}",
                nameof(RequestValidationBehavior<TRequest, TResponse>), typeof(TRequest).Name,
                typeof(TResponse).Name);

            _logger.LogDebug($"Handling {typeof(TRequest).FullName} with content {JsonSerializer.Serialize(request)}");

            await _validator.HandleValidationAsync(request);

            var response = await next();

            _logger.LogInformation($"Handled {typeof(TRequest).FullName}");
            return response;
        }
    }
}