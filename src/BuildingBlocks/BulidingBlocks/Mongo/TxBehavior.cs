using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Mongo;

// transaction not work on mongo standalone
public class TxBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly IMongoDbContext _dbContext;
    private readonly ILogger<TxBehavior<TRequest, TResponse>> _logger;

    public TxBehavior(IMongoDbContext dbContext, ILogger<TxBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbContext = dbContext;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken
        cancellationToken)
    {
        if (request is not ITxRequest) return await next();

        _logger.LogInformation("{Prefix} Handled command {MediatRRequest}", nameof(TxBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName);
        _logger.LogDebug("{Prefix} Handled command {MediatRRequest} with content {RequestContent}",
            nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName, JsonSerializer.Serialize(request));
        _logger.LogInformation("{Prefix} Open the transaction for {MediatRRequest}",
            nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName);

        try
        {
            // Achieving atomicity
            await _dbContext.BeginTransactionAsync();

            var response = await next();
            _logger.LogInformation("{Prefix} Executed the {MediatRRequest} request",
                nameof(TxBehavior<TRequest, TResponse>), typeof(TRequest).FullName);

            await _dbContext.CommitTransactionAsync();

            return response;
        }
        catch (System.Exception)
        {
            await _dbContext.RollbackTransactionAsync();
            throw;
        }
    }
}