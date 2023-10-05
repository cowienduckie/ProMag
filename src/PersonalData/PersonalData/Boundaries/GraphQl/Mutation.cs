using HotChocolate;
using HotChocolate.Authorization;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Boundaries.GraphQl.InputObjectTypes;
using PersonalData.Boundaries.GraphQl.ObjectTypes;
using PersonalData.Common.Constants;
using PersonalData.UseCases.Commands;
using PersonalData.UseCases.Responses;
using Shared;
using Shared.Caching;

namespace PersonalData.Boundaries.GraphQl;

public class Mutation
{
    [GraphQLType(typeof(PersonType))]
    [Authorize(AuthorizationPolicy.CAN_EDIT_ROLE)]
    public async Task<PersonDto> EditUser(
        [GraphQLType(typeof(EditUserInputType))]
        EditUserCommand editUserInput,
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
    [Authorize(AuthorizationPolicy.CAN_EDIT_ROLE)]
    public async Task<InviteUserResponse> InviteUser(
        [GraphQLType(typeof(InviteUserInputType))]
        InviteUserCommand inviteUserInput,
        [Service] ISender mediator)
    {
        return await mediator.Send(inviteUserInput);
    }

    [Authorize(AuthorizationPolicy.CAN_EDIT_ROLE)]
    public async Task<bool> UnlockUser(
        [GraphQLType(typeof(UnlockUserInputType))]
        UnlockUserCommand unlockUserInput,
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

    [Authorize(AuthorizationPolicy.CAN_EDIT_ROLE)]
    public async Task<bool> LockUser(
        [GraphQLType(typeof(LockUserInputType))]
        LockUserCommand lockUserInput,
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

    [Authorize(AuthorizationPolicy.CAN_EDIT_ROLE)]
    public async Task<bool> UpdateRolePermissions(UpdateRolePermissionsCommand updatePermissionsInput, [Service] ISender mediator)
    {
        return await mediator.Send(updatePermissionsInput);
    }
}