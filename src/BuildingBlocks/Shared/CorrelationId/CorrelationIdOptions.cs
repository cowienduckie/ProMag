namespace Shared.CorrelationId;

public class CorrelationIdOptions
{
    public string Header { get; set; } = "X-Correlation-ID";
    public bool IncludeInResponse { get; set; } = true;
    public bool UpdateTraceIdentifier { get; set; } = false;
}