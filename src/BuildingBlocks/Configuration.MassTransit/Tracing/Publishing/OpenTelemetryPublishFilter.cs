using System.Diagnostics;
using MassTransit;
using MassTransit.Metadata;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Publising;

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
        var tracer = _tracerProvider.GetTracer($"{StepName} {TypeMetadataCache<T>.ShortName}");

        Propagators.DefaultTextMapPropagator.Inject(tracer.CurrentSpan.Context, context.Headers, (h, k, v) => h.Set(k, v));

        if (context.MessageId.HasValue)
        {
            tracer.CurrentSpan.SetAttribute(MessageId, context.MessageId.Value.ToString());
        }

        if (context.ConversationId.HasValue)
        {
            tracer.CurrentSpan.SetAttribute(ConversationId, context.ConversationId.Value.ToString());
        }

        if (context.CorrelationId.HasValue)
        {
            tracer.CurrentSpan.SetAttribute(CorrelationId, context.CorrelationId.Value.ToString());
        }

        if (context.DestinationAddress != null)
        {
            tracer.CurrentSpan.SetAttribute(DestinationAddress, context.DestinationAddress.ToString());
        }

        if (context.RequestId.HasValue)
        {
            tracer.CurrentSpan.SetAttribute(RequestId, context.RequestId.Value.ToString());
        }

        await next.Send(context);
    }

    private static PropagationContext GetCurrentPropagationContext()
    {
        var currentActivity = Activity.Current;

        if (currentActivity == null)
        {
            return default;
        }

        var activityContext = currentActivity.Context;

        if (activityContext == default)
        {
            return default;
        }

        return new PropagationContext(activityContext, default);
    }
}