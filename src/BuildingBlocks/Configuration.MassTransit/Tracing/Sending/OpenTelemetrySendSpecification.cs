using MassTransit;
using MassTransit.Configuration;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Sending;

public class OpenTelemetrySendSpecification<T> : IPipeSpecification<SendContext<T>>
    where T : class
{
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetrySendSpecification(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    public void Apply(IPipeBuilder<SendContext<T>> builder)
    {
        builder.AddFilter(new OpenTelemetrySendFilter<T>(_tracerProvider));
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}