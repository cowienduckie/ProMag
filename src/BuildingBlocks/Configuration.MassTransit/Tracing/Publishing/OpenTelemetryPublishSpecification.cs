using MassTransit;
using MassTransit.Configuration;
using OpenTelemetry.Trace;

namespace Configuration.MassTransit.Tracing.Publishing;

public class OpenTelemetryPublishSpecification<T> : IPipeSpecification<PublishContext<T>> where T : class
{
    private readonly TracerProvider _tracerProvider;

    public OpenTelemetryPublishSpecification(TracerProvider tracerProvider)
    {
        _tracerProvider = tracerProvider;
    }

    public void Apply(IPipeBuilder<PublishContext<T>> builder)
    {
        builder.AddFilter(new OpenTelemetryPublishFilter<T>(_tracerProvider));
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}