using HotChocolate;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Boundaries.GraphQl.InputObjectTypes;
using PersonalData.Boundaries.GraphQl.ObjectTypes;
using PersonalData.UseCases.Commands;
using PersonalData.UseCases.Responses;
using Shared;
using Shared.Caching;
using Shared.CustomTypes;

namespace PersonalData.Boundaries.GraphQl;

public class Mutation
{
    [GraphQLType(typeof(PersonType))]
    [Authorize(AuthorizationPolicy.ADMIN_ACCESS)]
    public async Task<PersonDto> EditUser(
        [GraphQLType(typeof(EditUserInputType))] EditUserCommand editUserInput,
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] IConfiguration configuration)
    {
        var user = await mediator.Send(editUserInput);

        if (configuration.GetOptions<CacheOptions>("Redis").Enabled)
        {
            var cacheKey = string.Format(PersonalDataCacheKeys.UserById, editUserInput.PersonId);
            await distributedCache.RemoveAsync(cacheKey);
        }

        return user;
    }

    [GraphQLType(typeof(InviteUserResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_ACCESS)]
    public async Task<InviteUserResponse> InviteUser(
        [GraphQLType(typeof(InviteUserInputType))] InviteUserCommand inviteUserInput,
        [Service] ISender mediator)
    {
        return await mediator.Send(inviteUserInput);
    }

    [GraphQLType(typeof(RegisterUserResponseType))]
    [AllowAnonymous]
    public async Task<RegisterUserResponse> RegisterUser(
        [GraphQLType(typeof(RegisterUserInputType))] RegisterUserCommand registerUserInput,
        [Service] ISender mediator)
    {
        return await mediator.Send(registerUserInput);
    }

    [Authorize(AuthorizationPolicy.ADMIN_ACCESS)]
    public async Task<bool> UnlockUser(
        [GraphQLType(typeof(UnlockUserInputType))] UnlockUserCommand unlockUserInput,
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] IConfiguration configuration)
    {
        var result = await mediator.Send(unlockUserInput);

        if (configuration.GetOptions<CacheOptions>("Redis").Enabled)
        {
            var cacheKey = string.Format(PersonalDataCacheKeys.UserById, unlockUserInput.PersonId);
            await distributedCache.RemoveAsync(cacheKey);
        }

        return result;
    }

    [Authorize(AuthorizationPolicy.ADMIN_ACCESS)]
    public async Task<bool> LockUser(
        [GraphQLType(typeof(LockUserInputType))] LockUserCommand lockUserInput,
        [Service] ISender mediator,
        [Service] IDistributedCache distributedCache,
        [Service] IConfiguration configuration)
    {
        var result = await mediator.Send(lockUserInput);

        if (configuration.GetOptions<CacheOptions>("Redis").Enabled)
        {
            var cacheKey = string.Format(PersonalDataCacheKeys.UserById, lockUserInput.PersonId);
            await distributedCache.RemoveAsync(cacheKey);
        }

        return result;
    }

    [Authorize(AuthorizationPolicy.ADMIN_ACCESS)]
    public async Task<bool> UpdateRolePermissions(UpdateRolePermissionsCommand updatePermissionsInput, [Service] ISender mediator)
    {
        return await mediator.Send(updatePermissionsInput);
    }

    [GraphQLType(typeof(CreateWorkspaceResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<CreateWorkspaceResponse> CreateWorkspace(
        [GraphQLType(typeof(CreateWorkspaceInputType))] CreateWorkspaceCommand createWorkspaceInput,
        [Service] ISender mediator)
    {
        return await mediator.Send(createWorkspaceInput);
    }

    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<bool> DeleteWorkspace(
        [GraphQLType(typeof(DeleteWorkspaceInputType))] DeleteWorkspaceCommand deleteWorkspaceInput,
        [Service] ISender mediator)
    {
        return await mediator.Send(deleteWorkspaceInput);
    }

    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<bool> InviteUserToWorkspace(
        [GraphQLType(typeof(InviteUserToWorkspaceInputType))] InviteUserToWorkspaceCommand input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }

    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<bool> AcceptWorkspaceInvitation(
        [GraphQLType(typeof(AcceptWorkspaceInvitationInputType))] AcceptWorkspaceInvitationCommand input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }
}