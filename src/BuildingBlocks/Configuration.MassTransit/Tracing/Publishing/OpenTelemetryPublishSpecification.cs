using MassTransit;
using MassTransit.Configuration;

namespace Configuration.MassTransit.Tracing.Publishing;

public class OpenTelemetryPublishSpecification<T> : IPipeSpecification<PublishContext<T>> where T : class
{
    public void Apply(IPipeBuilder<PublishContext<T>> builder)
    {
        builder.AddFilter(new OpenTelemetryPublishFilter<T>());
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}