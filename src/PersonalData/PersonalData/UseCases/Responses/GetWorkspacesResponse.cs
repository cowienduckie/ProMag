using PersonalData.Boundaries.GraphQl.Dtos;

namespace PersonalData.UseCases.Responses;

public class GetWorkspacesResponse
{
    public IEnumerable<WorkspaceDto> OwnedWorkspaces { get; set; } = default!;
    public IEnumerable<WorkspaceDto> MemberWorkspaces { get; set; } = default!;
    public IEnumerable<WorkspaceDto> PendingWorkspaces { get; set; } = default!;
}