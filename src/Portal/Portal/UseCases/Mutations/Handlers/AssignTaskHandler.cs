using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Data;
using Portal.Services;
using Portal.UseCases.Responses;
using Shared.Common.ApiResponse;
using Shared.SecurityContext;

namespace Portal.UseCases.Mutations.Handlers;

public class AssignTaskHandler : IRequestHandler<AssignTaskCommand, AssignTaskResponse>
{
    private readonly IAccessPermissionService _accessPermissionService;
    private readonly ILogger<AssignTaskHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly ISecurityContextAccessor _securityContext;

    public AssignTaskHandler(
        ILogger<AssignTaskHandler> logger,
        PortalContext portalContext,
        ISecurityContextAccessor securityContext,
        IAccessPermissionService accessPermissionService)
    {
        _logger = logger;
        _portalContext = portalContext;
        _securityContext = securityContext;
        _accessPermissionService = accessPermissionService;
    }

    public async Task<AssignTaskResponse> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("{Handler} - Start", nameof(AssignTaskHandler));

            var task = await _portalContext.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == Guid.Parse(request.TaskId), cancellationToken);

            if (task is null)
            {
                _logger.LogError("{Handler} - TaskId={TaskId} not found", nameof(AssignTaskHandler), request.TaskId);

                return new AssignTaskResponse
                {
                    StatusCode = ResponseStatuses.NotFound.GetCode(),
                    ErrorCode = Errors.VAL_001.GetCode(),
                    ErrorMessage = Errors.VAL_001.GetMessages()
                };
            }

            // Check if user has access to project
            var hasUserAccessToTask = Guid.TryParse(_securityContext.UserId, out var userId) &&
                                      await _accessPermissionService.HasAccessToProject(
                                          userId,
                                          task.Project.WorkspaceId,
                                          task.Project.TeamId
                                      );

            if (!hasUserAccessToTask)
            {
                _logger.LogError("{Handler} - UserId={UserId} has no access to TaskId={TaskId}",
                    nameof(AssignTaskHandler),
                    _securityContext.UserId,
                    request.TaskId);

                return new AssignTaskResponse
                {
                    StatusCode = ResponseStatuses.Fail.GetCode(),
                    ErrorCode = Errors.VAL_002.GetCode(),
                    ErrorMessage = Errors.VAL_002.GetMessages("User has no access to this task")
                };
            }

            // Check if assignee has access to project
            var hasAssigneeAccessToTask = Guid.TryParse(request.AssigneeId, out var assigneeId) &&
                                          await _accessPermissionService.HasAccessToProject(
                                              assigneeId,
                                              task.Project.WorkspaceId,
                                              task.Project.TeamId
                                          );

            if (!hasAssigneeAccessToTask)
            {
                _logger.LogError("{Handler} - AssigneeId={AssigneeId} has no access to TaskId={TaskId}",
                    nameof(AssignTaskHandler),
                    request.AssigneeId,
                    request.TaskId);

                return new AssignTaskResponse
                {
                    StatusCode = ResponseStatuses.Fail.GetCode(),
                    ErrorCode = Errors.VAL_002.GetCode(),
                    ErrorMessage = Errors.VAL_002.GetMessages("Assignee has no access to this task")
                };
            }

            task.AssigneeId = assigneeId;

            await _portalContext.SaveChangesAsync(cancellationToken);

            return new AssignTaskResponse
            {
                ProjectId = task.ProjectId.ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Handler} - Exception={Message}", nameof(AssignTaskHandler), ex.Message);

            return new AssignTaskResponse
            {
                StatusCode = ResponseStatuses.FailWithException.GetCode(),
                ErrorCode = Errors.COM_000.GetCode(),
                ErrorMessage = Errors.COM_000.GetMessages()
            };
        }
    }
}