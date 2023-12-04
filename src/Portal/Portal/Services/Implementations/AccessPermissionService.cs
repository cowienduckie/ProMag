using Promag.Protobuf.PersonalData.V1;

namespace Portal.Services.Implementations;

public class AccessPermissionService : IAccessPermissionService
{
    private readonly PersonalApi.PersonalApiClient _personalApiClient;

    public AccessPermissionService(PersonalApi.PersonalApiClient personalApiClient)
    {
        _personalApiClient = personalApiClient;
    }

    public async Task<bool> HasAccessToProject(Guid userId, Guid workspaceId, Guid teamId)
    {
        var (workspaces, teams) = await GetUserCollaboration(userId);

        if (!workspaces.Contains(workspaceId) && !teams.Contains(teamId))
        {
            return false;
        }

        return true;
    }

    public async Task<Tuple<List<Guid>, List<Guid>>> GetUserCollaboration(Guid userId)
    {
        var response = await _personalApiClient.GetUserCollaborationAsync(new GetUserCollaborationRequest
        {
            UserId = userId.ToString()
        });

        var workspaces = response.Workspaces.Select(Guid.Parse).ToList();
        var teams = response.Teams.Select(Guid.Parse).ToList();

        return Tuple.Create(workspaces, teams);
    }
}