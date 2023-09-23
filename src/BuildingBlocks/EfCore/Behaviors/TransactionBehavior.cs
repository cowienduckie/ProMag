using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Shared;

namespace EfCore.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly DbContext _dbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(DbContext dbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attribute = typeof(TRequest).GetCustomAttribute<TransactionAttribute>();

        if (attribute == null)
        {
            return await next();
        }

        var typeName = request.GetGenericTypeName();
        var strategy = _dbContext.Database.CreateExecutionStrategy();
        TResponse response;

        return await strategy.ExecuteAsync(async () =>
        {
            _logger.LogInformation($"Open the transaction for {nameof(request)}.");

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
            {
                _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName,
                    request);

                response = await next();

                _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                await transaction.CommitAsync(cancellationToken);
            }

            return response;
        });
    }
}