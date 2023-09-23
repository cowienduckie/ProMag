using MassTransit;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Publishing;

public class OpenTelemetryPublishFilter<T> : IFilter<PublishContext<T>> where T : class
{
    private const string MessageId = nameof(MessageId);
    private const string ConversationId = nameof(ConversationId);
    private const string CorrelationId = nameof(CorrelationId);
    private const string DestinationAddress = nameof(DestinationAddress);
    private const string RequestId = nameof(RequestId);
    private const string MessageType = nameof(MessageType);

    private const string StepName = "MassTransit:Publish";

    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryPublishFilter(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("TelemetrySendFilter");
    }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        // TODO: Investigate why there is no need to Start a new span
        // var operationName = $"{StepName} {TypeMetadataCache<T>.ShortName}";
        // var tracer = _tracerProvider.GetTracer(operationName);

        var propagationContext = new PropagationContext(Tracer.CurrentSpan.Context, Baggage.Current);

        Propagators.DefaultTextMapPropagator.Inject(propagationContext, context.Headers, (h, k, v) => h.Set(k, v));

        if (context.MessageId.HasValue)
        {
            Tracer.CurrentSpan.SetAttribute(MessageId, context.MessageId.Value.ToString());
        }

        if (context.ConversationId.HasValue)
        {
            Tracer.CurrentSpan.SetAttribute(ConversationId, context.ConversationId.Value.ToString());
        }

        if (context.CorrelationId.HasValue)
        {
            Tracer.CurrentSpan.SetAttribute(CorrelationId, context.CorrelationId.Value.ToString());
        }

        if (context.DestinationAddress != null)
        {
            Tracer.CurrentSpan.SetAttribute(DestinationAddress, context.DestinationAddress.ToString());
        }

        if (context.RequestId.HasValue)
        {
            Tracer.CurrentSpan.SetAttribute(RequestId, context.RequestId.Value.ToString());
        }

        await next.Send(context);
    }
}