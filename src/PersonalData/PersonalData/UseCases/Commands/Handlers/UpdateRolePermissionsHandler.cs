using MediatR;
using Microsoft.Extensions.Logging;
using PersonalData.Common;
using Promag.Protobuf.Identity.V1;
using Shared.CustomTypes;

namespace PersonalData.UseCases.Commands.Handlers;

public class UpdateRolePermissionsHandler : IRequestHandler<UpdateRolePermissionsCommand, bool>
{
    private readonly IdentityApi.IdentityApiClient _identityApiClient;
    private readonly ILogger<UpdateRolePermissionsHandler> _logger;

    public UpdateRolePermissionsHandler(IdentityApi.IdentityApiClient identityApiClient, ILogger<UpdateRolePermissionsHandler> logger)
    {
        _identityApiClient = identityApiClient;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{Handler} - Start", nameof(UpdateRolePermissionsHandler));
        _logger.LogInformation("{Handler} - Send gRPC request to Identity Server", nameof(UpdateRolePermissionsHandler));

        var permissionClaimsRequest = new UpdateRoleClaimsRequest
        {
            RoleId = request.RoleId.ToString(),
            ClaimType = Claims.Permission
        };

        permissionClaimsRequest.ClaimValues.AddRange(request.Permissions);

        var result = await _identityApiClient.UpdateRoleClaimsAsync(permissionClaimsRequest, cancellationToken: cancellationToken);

        if (!result.Success)
        {
            _logger.LogInformation("{Handler} - Unexpected error(s) occured from Identity Server", nameof(UpdateRolePermissionsHandler));

            throw result.Errors.ToValidationException();
        }

        _logger.LogInformation("{Handler} - Finish", nameof(UpdateRolePermissionsHandler));

        return true;
    }
}