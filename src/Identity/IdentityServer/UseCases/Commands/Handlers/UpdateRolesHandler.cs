using IdentityServer.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Promag.Protobuf.Identity.V1;
using Shared.CustomTypes;

namespace IdentityServer.UseCases.Commands.Handlers;

public class UpdateRolesHandler : IRequestHandler<UpdateRolesRequest, UpdateRolesResponse>
{
    private readonly ILogger<UpdateRolesHandler> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateRolesHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<UpdateRolesHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<UpdateRolesResponse> Handle(UpdateRolesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - UserId: {UserId} - Start", nameof(UpdateRolesHandler), request.UserId);

        var appUser = await _userManager.FindByIdAsync(request.UserId);

        if (appUser is null)
        {
            _logger.LogError("{HandlerName} - UserId: {UserId} - User not found!", nameof(UpdateRolesHandler), request.UserId);

            return new UpdateRolesResponse
            {
                Result = false
            };
        }

        var currentRoleNames = await _userManager.GetRolesAsync(appUser);

        if (!request.RoleIds.Any() || currentRoleNames.Contains(Roles.SUPER_USER_ROLE_NAME))
        {
            _logger.LogError("{HandlerName} - UserId: {UserId} - User with no role or contain role {CannotUpdatedRole}!",
                nameof(UpdateRolesHandler),
                request.UserId,
                nameof(Roles.SUPER_USER_ROLE_NAME));

            return new UpdateRolesResponse
            {
                Result = false
            };
        }

        var currentRoles = _roleManager.Roles
            .Where(x => currentRoleNames.Contains<string?>(x.Name))
            .ToList();

        await _userManager.RemoveFromRolesAsync(appUser, currentRoles.Select(x => x.NormalizedName)!);

        var newRoles = _roleManager.Roles
            .Where(x => request.RoleIds.Contains(x.Id))
            .ToList();

        foreach (var role in newRoles)
        {
            if (role.NormalizedName is null)
            {
                continue;
            }

            await _userManager.AddToRoleAsync(appUser, role.NormalizedName);
        }

        _logger.LogInformation("{HandlerName} - UserId: {UserId} - Finish", nameof(UpdateRolesHandler), request.UserId);

        var result = new UpdateRolesResponse
        {
            Result = true
        };

        return result;
    }
}