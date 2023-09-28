namespace EfCore.Auditing;

public interface IAuditService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}