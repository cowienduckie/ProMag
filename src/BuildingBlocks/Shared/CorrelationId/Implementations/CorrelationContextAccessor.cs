namespace Shared.CorrelationId.Implementations;

public class CorrelationContextAccessor : ICorrelationContextAccessor
{
    private static readonly AsyncLocal<CorrelationContext> _correlationContext = new();

    public CorrelationContext CorrelationContext
    {
        get => _correlationContext.Value!;
        set => _correlationContext.Value = value;
    }
}