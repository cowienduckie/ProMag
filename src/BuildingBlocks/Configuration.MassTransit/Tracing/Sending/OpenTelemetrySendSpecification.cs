using MassTransit;
using MassTransit.Configuration;

namespace Configuration.MassTransit.Tracing.Sending;

public class OpenTelemetrySendSpecification<T> : IPipeSpecification<SendContext<T>>
    where T : class
{
    public void Apply(IPipeBuilder<SendContext<T>> builder)
    {
        builder.AddFilter(new OpenTelemetrySendFilter<T>());
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }
}