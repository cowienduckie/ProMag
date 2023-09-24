using IdentityServer.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promag.Protobuf.Identity.V1;

namespace IdentityServer.UseCases.Queries.Handlers;

public class GetRoleByIdsHandler : IRequestHandler<GetRoleByIdsRequest, GetRolesResponse>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetRoleByIdsHandler> _logger;

    public GetRoleByIdsHandler(ApplicationDbContext dbContext, ILogger<GetRoleByIdsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<GetRolesResponse> Handle(GetRoleByIdsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - Start", nameof(GetRoleByIdsHandler));

        var query =
            from r in _dbContext.Roles
            join ur in _dbContext.UserRoles
                on r.Id equals ur.RoleId into userRoleGroup
            from role in userRoleGroup.DefaultIfEmpty()
            join u in _dbContext.Users.OrderBy(x => x.UserName)
                on role.UserId equals u.Id into userGroup
            from user in userGroup.DefaultIfEmpty()
            select new
            {
                r.Name,
                r.DisplayName,
                r.Description,
                RoleId = r.Id,
                UserId = user.Id,
                user.UserName
            };

        if (request.RoleIds is not null && request.RoleIds.Any())
        {
            query = query.Where(x => request.RoleIds.Contains(x.RoleId));
        }

        var roleWithUsers = (await query.ToListAsync(cancellationToken))
            .GroupBy(q => new { q.RoleId })
            .Distinct()
            .Select(group => new
            {
                group.Key,
                Group = group
            })
            .ToList();

        var result = new GetRolesResponse();

        roleWithUsers.ForEach(r =>
        {
            var role = new RoleDto
            {
                RoleId = r.Key.RoleId,
                Name = r.Group.FirstOrDefault()?.Name,
                DisplayName = r.Group.FirstOrDefault()?.DisplayName,
                Description = r.Group.FirstOrDefault()?.Description
            };

            r.Group
                .Where(g => g.UserId != null)
                .ToList()
                .ForEach(u => role.Users.Add(u.UserId, u.UserName));

            result.Roles.Add(role);
        });

        _logger.LogInformation("{HandlerName} - Finish", nameof(GetRoleByIdsHandler));

        return result;
    }
}