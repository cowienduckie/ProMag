using OpenTracing.Util;
using Serilog.Core;
using Serilog.Events;

namespace Shared.Logging.Enrichers;

public class OpenTracingContextLogEventEnricher : ILogEventEnricher
{
    private const string TracIdPropertyName = "TraceId";
    private const string SpanIdPropertyName = "SpanId";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var tracer = GlobalTracer.Instance;

        if (tracer?.ActiveSpan == null)
        {
            return;
        }

        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(TracIdPropertyName, tracer.ActiveSpan.Context.TraceId));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(SpanIdPropertyName, tracer.ActiveSpan.Context.SpanId));
    }
}