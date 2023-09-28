using Shared.Domain;

namespace MasterData.Domain;

public class ActivityLog : AuditableEntity
{
    public string? IpAddress { get; set; }
    public string? Service { get; set; }
    public string? Action { get; set; }
    public long? Duration { get; set; }
    public string? Parameters { get; set; }
    public string? Username { get; set; }
}