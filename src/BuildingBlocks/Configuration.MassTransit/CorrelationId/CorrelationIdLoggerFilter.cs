using System.Diagnostics.CodeAnalysis;
using MassTransit;
using Shared.CorrelationId;
using LogContext = Serilog.Context.LogContext;

namespace Configuration.MassTransit.CorrelationId;

public class CorrelationIdLoggerFilter<T> : IFilter<T> where T : class, PipeContext
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    public CorrelationIdLoggerFilter(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContextAccessor = correlationContextAccessor;
    }

    public void Probe(ProbeContext context)
    {
    }

    [SuppressMessage("ReSharper", "InconsistentContextLogPropertyNaming")]
    public async Task Send(T context, IPipe<T> next)
    {
        switch (context)
        {
            case ConsumeContext consumeContext:
                using (LogContext.PushProperty("X-Correlation-ID", consumeContext.CorrelationId?.ToString()))
                {
                    await next.Send(context);
                }

                break;
            case SendContext sendContext:
                sendContext.CorrelationId = _correlationContextAccessor.CorrelationContext?.CorrelationId;

                await next.Send(context);
                break;
            default:
                await next.Send(context);
                break;
        }
    }
}