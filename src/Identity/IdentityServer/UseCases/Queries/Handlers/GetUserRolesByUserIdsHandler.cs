using IdentityServer.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promag.Protobuf.Identity.V1;

namespace IdentityServer.UseCases.Queries.Handlers;

public class GetUserRolesByUserIdsHandler : IRequestHandler<GetUserRolesByUserIdsRequest, List<UserRoleDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetUserRolesByUserIdsHandler> _logger;

    public GetUserRolesByUserIdsHandler(ApplicationDbContext dbContext, ILogger<GetUserRolesByUserIdsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<UserRoleDto>> Handle(GetUserRolesByUserIdsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetUserRolesByUserIdsHandler));

        var query = _dbContext.Roles
            .Join(_dbContext.UserRoles, role => role.Id, ur => ur.RoleId, (role, ur) => new { role, ur })
            .OrderBy(table => table.role.DisplayName)
            .Where(table => request.UserIds.Contains(table.ur.UserId))
            .Select(table => new UserRoleDto
            {
                RoleId = table.ur.RoleId, RoleName = table.role.Name, UserId = table.ur.UserId
            });

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetUserRolesByUserIdsHandler));

        return await query.ToListAsync(cancellationToken);
    }
}