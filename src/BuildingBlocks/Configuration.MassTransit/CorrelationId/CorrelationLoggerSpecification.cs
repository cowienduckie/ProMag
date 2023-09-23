using MassTransit;
using MassTransit.Configuration;
using Shared.CorrelationId;

namespace Configuration.MassTransit.CorrelationId;

public class CorrelationLoggerSpecification<T> : IPipeSpecification<T> where T : class, PipeContext
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    public CorrelationLoggerSpecification(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContextAccessor = correlationContextAccessor;
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return Enumerable.Empty<ValidationResult>();
    }

    public void Apply(IPipeBuilder<T> builder)
    {
        builder.AddFilter(new CorrelationIdLoggerFilter<T>(_correlationContextAccessor));
    }
}