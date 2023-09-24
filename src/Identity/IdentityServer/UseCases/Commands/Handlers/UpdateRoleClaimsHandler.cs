using System.Security.Claims;
using IdentityServer.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Promag.Protobuf.Identity.V1;
using Shared.CustomTypes;

namespace IdentityServer.UseCases.Commands.Handlers;

public class UpdateRoleClaimsHandler : IRequestHandler<UpdateRoleClaimsRequest, UpdateRoleClaimsResponse>
{
    private readonly ILogger<UpdateRoleClaimsHandler> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UpdateRoleClaimsHandler(RoleManager<ApplicationRole> roleManager, ILogger<UpdateRoleClaimsHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<UpdateRoleClaimsResponse> Handle(UpdateRoleClaimsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - {RoleId} - Start", nameof(UpdateRoleClaimsHandler), request.RoleId);

        var role = await _roleManager.FindByIdAsync(request.RoleId);

        if (role is null || role.Name == Roles.SUPER_USER_ROLE_NAME)
        {
            _logger.LogError("{HandlerName} - {RoleId} - Role not found or cannot update role {CannotUpdatedRole}!",
                nameof(UpdateRoleClaimsHandler),
                request.RoleId,
                nameof(Roles.SUPER_USER_ROLE_NAME));

            return new UpdateRoleClaimsResponse
            {
                Success = false
            };
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);
        var newClaimValues = request.ClaimValues.ToArray();

        var deleteClaims = currentClaims
            .Where(x => x.Type == request.ClaimType && !newClaimValues.Contains(x.Value))
            .ToList();

        if (deleteClaims.Any())
        {
            foreach (var claim in deleteClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
        }

        var currentClaimValues = currentClaims
            .Where(x => x.Type == request.ClaimType)
            .Select(y => y.Value)
            .ToList();

        foreach (var claimValue in request.ClaimValues)
        {
            if (currentClaimValues.Contains(claimValue))
            {
                continue;
            }

            var claim = new Claim(request.ClaimType, claimValue);

            await _roleManager.AddClaimAsync(role, claim);
        }

        _logger.LogInformation("{HandlerName} - {RoleId} - Finish", nameof(UpdateRoleClaimsHandler), request.RoleId);

        return new UpdateRoleClaimsResponse
        {
            Success = true
        };
    }
}