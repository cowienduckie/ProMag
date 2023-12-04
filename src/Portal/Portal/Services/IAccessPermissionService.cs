namespace Portal.Services;

public interface IAccessPermissionService
{
    Task<bool> HasAccessToProject(Guid userId, Guid workspaceId, Guid teamId);
    Task<Tuple<List<Guid>, List<Guid>>> GetUserCollaboration(Guid userId);
}