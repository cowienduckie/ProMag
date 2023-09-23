namespace Shared.CorrelationId;

public interface ICorrelationContextFactory
{
    CorrelationContext? Create(Guid correlationId, string header);
    void Dispose();
}