using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalData.Data;
using PersonalData.Domain;
using PersonalData.UseCases.Responses;
using Shared.SecurityContext;

namespace PersonalData.UseCases.Commands.Handlers;

public class CreateWorkspaceHandler : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResponse>
{
    private readonly PersonalContext _dbContext;
    private readonly ILogger<CreateWorkspaceHandler> _logger;
    private readonly ISecurityContextAccessor _securityContext;

    public CreateWorkspaceHandler(PersonalContext dbContext, ILogger<CreateWorkspaceHandler> logger, ISecurityContextAccessor securityContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _securityContext = securityContext;
    }

    public async Task<CreateWorkspaceResponse> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(CreateWorkspaceHandler));

        var owner = await _dbContext.People.FirstOrDefaultAsync(p => p.Id == Guid.Parse(_securityContext.UserId!), cancellationToken);

        var workspace = new Workspace
        {
            Name = request.Name,
            Members = new List<Person>
            {
                owner!
            }
        };

        _logger.LogInformation("{Handler} - Save changes to DB", nameof(CreateWorkspaceHandler));

        await _dbContext.Workspaces.AddAsync(workspace, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("{Handler} - End", nameof(CreateWorkspaceHandler));

        return new CreateWorkspaceResponse
        {
            WorkspaceId = workspace.Id.ToString(),
            UserId = workspace.CreatedBy.ToString()
        };
    }
}