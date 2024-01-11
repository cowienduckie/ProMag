using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Data;
using Shared.SecurityContext;

namespace PersonalData.UseCases.Commands.Handlers;

public class DeleteWorkspaceHandler : IRequestHandler<DeleteWorkspaceCommand, bool>
{
    private readonly PersonalContext _dbContext;
    private readonly ILogger<DeleteWorkspaceHandler> _logger;
    private readonly ISecurityContextAccessor _securityContext;

    public DeleteWorkspaceHandler(PersonalContext dbContext, ILogger<DeleteWorkspaceHandler> logger, ISecurityContextAccessor securityContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _securityContext = securityContext;
    }

    public async Task<bool> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(DeleteWorkspaceHandler));

        var workspace =
            await _dbContext.Workspaces.FirstOrDefaultAsync(w => w.Id == request.WorkspaceId && w.CreatedBy == Guid.Parse(_securityContext.UserId!),
                cancellationToken);

        if (workspace == null)
        {
            _logger.LogInformation("{Handler} - Workspace not found", nameof(DeleteWorkspaceHandler));
            return false;
        }

        workspace.DeletedOn = DateTime.UtcNow;
        workspace.DeletedBy = Guid.Parse(_securityContext.UserId!);

        _dbContext.Workspaces.Update(workspace);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - End", nameof(DeleteWorkspaceHandler));

        return true;
    }
}