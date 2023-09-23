using MassTransit;
using MassTransit.Configuration;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Consuming;

public class OpenTelemetryConsumeSpecification<T> : IPipeSpecification<ConsumeContext<T>> where T : class
{
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryConsumeSpecification(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    public void Apply(IPipeBuilder<ConsumeContext<T>> builder)
    {
        builder.AddFilter(new OpenTelemetryConsumeFilter<T>(_tracerProvider));
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}