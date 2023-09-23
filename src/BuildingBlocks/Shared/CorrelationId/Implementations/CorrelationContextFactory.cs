namespace Shared.CorrelationId.Implementations;

public class CorrelationContextFactory : ICorrelationContextFactory
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;

    public CorrelationContextFactory(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContextAccessor = correlationContextAccessor;
    }

    public CorrelationContext Create(Guid correlationId, string header)
    {
        var correlationContext = new CorrelationContext(correlationId, header);

        _correlationContextAccessor.CorrelationContext = correlationContext;

        return correlationContext;
    }

    public void Dispose()
    {
        _correlationContextAccessor.CorrelationContext = null;
    }
}