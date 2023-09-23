namespace Shared.CorrelationId;

public interface ICorrelationContextAccessor
{
    CorrelationContext? CorrelationContext { get; set; }
}