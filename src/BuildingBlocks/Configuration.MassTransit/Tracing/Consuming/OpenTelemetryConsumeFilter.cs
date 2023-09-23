using MassTransit;
using MassTransit.Metadata;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Consuming;

public class OpenTelemetryConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private const string MessageId = nameof(MessageId);
    private const string ConversationId = nameof(ConversationId);
    private const string CorrelationId = nameof(CorrelationId);
    private const string DestinationAddress = nameof(DestinationAddress);
    private const string InputAddress = nameof(InputAddress);
    private const string RequestId = nameof(RequestId);
    private const string MessageType = nameof(MessageType);

    private const string StepName = "MassTransit:Consumer";

    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryConsumeFilter(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("TelemetryConsumeFilter");
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var operationName = $"{StepName} {TypeMetadataCache<T>.ShortName}";
        var tracer = _tracerProvider.GetTracer(operationName);

        var propagationContext = Propagators.DefaultTextMapPropagator.Extract(
            default,
            context.Headers.ToList(),
            (r, name) => r.Where(x => x.Key == name).Select(x => x.Value.ToString()));

        var incomingSpan = tracer.StartSpan(TypeMetadataCache<T>.ShortName, SpanKind.Consumer, new SpanContext(propagationContext.ActivityContext));

        if (context.MessageId.HasValue)
        {
            incomingSpan.SetAttribute(MessageId, context.MessageId.Value.ToString());
        }

        if (context.ConversationId.HasValue)
        {
            incomingSpan.SetAttribute(ConversationId, context.ConversationId.Value.ToString());
        }

        if (context.CorrelationId.HasValue)
        {
            incomingSpan.SetAttribute(CorrelationId, context.CorrelationId.Value.ToString());
        }

        if (context.DestinationAddress != null)
        {
            incomingSpan.SetAttribute(DestinationAddress, context.DestinationAddress.ToString());
        }

        if (context.RequestId.HasValue)
        {
            incomingSpan.SetAttribute(RequestId, context.RequestId.Value.ToString());
        }

        await next.Send(context);

        incomingSpan.End();
    }
}