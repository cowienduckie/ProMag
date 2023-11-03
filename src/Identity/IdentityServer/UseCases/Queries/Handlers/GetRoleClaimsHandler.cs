using IdentityServer.Models.DbModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Promag.Protobuf.Identity.V1;

namespace IdentityServer.UseCases.Queries.Handlers;

public class GetRoleClaimsHandler : IRequestHandler<RoleClaimsRequest, RoleClaimsResponse>
{
    private readonly ILogger<GetRoleClaimsHandler> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public GetRoleClaimsHandler(RoleManager<ApplicationRole> roleManager, ILogger<GetRoleClaimsHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<RoleClaimsResponse> Handle(RoleClaimsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{HandlerName} - {RoleId} - {ClaimType} - Start", nameof(GetRoleClaimsHandler), request.RoleId, request.ClaimType);

        var role = await _roleManager.FindByIdAsync(request.RoleId);

        if (role is null)
        {
            _logger.LogError("{HandlerName} - {RoleId} - Role not found!", nameof(GetRoleClaimsHandler), request.RoleId);

            return new RoleClaimsResponse();
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        var claimValues = claims
            .Where(x => x.Type == request.ClaimType)
            .Select(y => y.Value);

        _logger.LogInformation("{HandlerName} - {RoleId} - {ClaimType} - Finish", nameof(GetRoleClaimsHandler), request.RoleId, request.ClaimType);

        var result = new RoleClaimsResponse();

        result.Permissions.AddRange(claimValues);

        return result;
    }
}