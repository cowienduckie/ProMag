using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using IdentityServer.UseCases.Queries;
using MediatR;
using Promag.Protobuf.Commons.V1;
using Promag.Protobuf.Identity.V1;

namespace IdentityServer.Boundaries.Grpc;

public class IdentityService : IdentityApi.IdentityApiBase
{
    private readonly IMediator _mediator;

    public IdentityService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
    {
        return await _mediator.Send(new PingQuery());
    }

    public override async Task<CreateLogInUserResponse> CreateLoginAccount(CreateLogInUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }

    public override async Task<GetUserRolesByUserIdsResponse> GetUserRolesByUserIds(GetUserRolesByUserIdsRequest request, ServerCallContext context)
    {
        var userRoleDtos = await _mediator.Send(request);
        var result = new GetUserRolesByUserIdsResponse();

        result.UserRoles.AddRange(userRoleDtos);

        return result;
    }

    public override async Task<GetRolesResponse> GetRoles(GetRoleByIdsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }

    public override async Task<UpdateRolesResponse> UpdateUserRoles(UpdateRolesRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }

    public override async Task<AccountResponse> UnlockUserAccount(AccountRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }

    public override async Task<LockUserResponse> LockUserAccount(LockUserRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }

    public override async Task<UpdateRoleClaimsResponse> UpdateRoleClaims(UpdateRoleClaimsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }

    public override async Task<RoleClaimsResponse> GetRolesClaims(RoleClaimsRequest request, ServerCallContext context)
    {
        return await _mediator.Send(request);
    }
}