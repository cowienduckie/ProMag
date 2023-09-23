namespace Shared.CorrelationId;

public class CorrelationContext
{
    internal CorrelationContext(Guid correlationId, string header)
    {
        Guard.NotNullOrEmpty(correlationId);
        Guard.NotNullOrEmpty(header);

        CorrelationId = correlationId;
        Header = header;
    }

    public Guid CorrelationId { get; set; }
    public string Header { get; set; }
}