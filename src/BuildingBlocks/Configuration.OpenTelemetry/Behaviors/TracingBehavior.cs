using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Shared;

namespace Configuration.OpenTelemetry.Behaviors;

public class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TracingBehavior<TRequest, TResponse>> _logger;
    private readonly Tracer _tracer;

    public TracingBehavior(TracerProvider tracerProvider, ILogger<TracingBehavior<TRequest, TResponse>> logger)
    {
        _tracer = tracerProvider.GetTracer("MediatR behavior");
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using (_tracer.StartActiveSpan(request.GetGenericTypeName()))
        {
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);

            var response = await next();

            _logger.LogInformation("----- Command {CommandName} handled", request.GetGenericTypeName());

            return response;
        }
    }
}