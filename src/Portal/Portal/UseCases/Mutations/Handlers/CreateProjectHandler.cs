using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Portal.Common.Constants;
using Portal.Data;
using Portal.Domain;
using Portal.Services;
using Portal.UseCases.Responses;
using Shared.Common.ApiResponse;
using Shared.SecurityContext;

namespace Portal.UseCases.Mutations.Handlers;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
{
    private readonly IAccessPermissionService _accessPermissionService;
    private readonly ILogger<CreateProjectHandler> _logger;
    private readonly PortalContext _portalContext;
    private readonly ISecurityContextAccessor _securityContext;
    private readonly IValidator<CreateProjectCommand> _validator;

    public CreateProjectHandler(
        ILogger<CreateProjectHandler> logger,
        PortalContext portalContext,
        ISecurityContextAccessor securityContext,
        IValidator<CreateProjectCommand> validator,
        IAccessPermissionService accessPermissionService)
    {
        _logger = logger;
        _portalContext = portalContext;
        _securityContext = securityContext;
        _validator = validator;
        _accessPermissionService = accessPermissionService;
    }

    public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(CreateProjectHandler));

        if (!Guid.TryParse(_securityContext.UserId, out var userId))
        {
            _logger.LogError("{Handler} - UserId={UserId} is not a valid Guid", nameof(CreateProjectHandler), _securityContext.UserId);

            return new CreateProjectResponse
            {
                StatusCode = ResponseStatuses.BadRequest.GetCode(),
                ErrorCode = Errors.VAL_000.GetCode(),
                ErrorMessage = Errors.VAL_000.GetMessages()
            };
        }

        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var (workspaces, teams) = await _accessPermissionService.GetUserCollaboration(userId);

            if (request.WorkspaceId is null ||
                !Guid.TryParse(request.WorkspaceId, out var workspaceId) ||
                !workspaces.Contains(workspaceId))
            {
                workspaceId = workspaces.FirstOrDefault(); // TODO: Find default workspace
            }

            if (request.TeamId is null ||
                !Guid.TryParse(request.TeamId, out var teamId) ||
                !teams.Contains(teamId))
            {
                teamId = teams.FirstOrDefault(); // TODO: Find default team
            }

            var newProject = new Project
            {
                Name = request.Name,
                Notes = request.Notes,
                Color = request.Color ?? ColorHexCode.Default,
                DueDate = request.DueDate?.ToUniversalTime(),
                Archived = false,
                OwnerId = userId,
                TeamId = teamId,
                WorkspaceId = workspaceId,
                Sections = new List<Section>
                {
                    new(DefaultName.ToDoColumn, 1),
                    new(DefaultName.InProgressColumn, 2),
                    new(DefaultName.DoneColumn, 3)
                }
            };

            await _portalContext.Projects.AddAsync(newProject, cancellationToken);
            await _portalContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("{Handler} - Finish", nameof(CreateProjectHandler));

            return new CreateProjectResponse
            {
                StatusCode = ResponseStatuses.Success.GetCode(),
                ProjectId = Guid.NewGuid().ToString()
            };
        }
        catch (ValidationException ex)
        {
            _logger.LogError("{Handler} - Validation failed: {Message}", nameof(CreateProjectHandler), ex.Message);

            return new CreateProjectResponse
            {
                StatusCode = ResponseStatuses.BadRequest.GetCode(),
                ErrorCode = Errors.VAL_001.GetCode(),
                ErrorMessage = Errors.VAL_001.GetMessages(ex.Message)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Handler} - Unexpected errors: {Message}", nameof(CreateProjectHandler), ex.Message);

            return new CreateProjectResponse
            {
                StatusCode = ResponseStatuses.FailWithException.GetCode(),
                ErrorCode = Errors.COM_000.GetCode(),
                ErrorMessage = Errors.COM_000.GetMessages()
            };
        }
    }
}