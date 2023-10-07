using MediatR;
using Microsoft.Extensions.Logging;
using Promag.Protobuf.Identity.V1;
using Shared;
using Shared.CustomTypes;

namespace PersonalData.UseCases.Queries.Handlers;

public class GetRolePermissionsHandler : IRequestHandler<GetRolePermissionsQuery, IEnumerable<string>>
{
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly ILogger<GetRolePermissionsHandler> _logger;

    public GetRolePermissionsHandler(IdentityApi.IdentityApiClient identityApiClient, ILogger<GetRolePermissionsHandler> logger)
    {
        _identityApiClient = identityApiClient;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        Guard.NotNull(request);

        _logger.LogInformation("{Handler} - Start", nameof(GetRolePermissionsHandler));

        var permissionClaimsRequest = new RoleClaimsRequest
        {
            RoleId = request.RoleId.ToString(),
            ClaimType = Permissions.PERMISSION_CLAIM_TYPE
        };

        var result = await _identityApiClient.GetRolesClaimsAsync(permissionClaimsRequest, cancellationToken: cancellationToken);

        _logger.LogInformation("{Handler} - Start", nameof(GetRolePermissionsHandler));

        return result.Permissions;
    }
}