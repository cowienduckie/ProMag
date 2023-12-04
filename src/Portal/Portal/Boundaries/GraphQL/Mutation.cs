using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Authorization;
using MediatR;
using Portal.Boundaries.GraphQL.ResponseTypes;
using Portal.UseCases.Mutations;
using Portal.UseCases.Responses;
using Shared.CustomTypes;

namespace Portal.Boundaries.GraphQL;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Mutation
{
    [GraphQLType(typeof(CreateProjectResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<CreateProjectResponse> CreateProject(
        CreateProjectCommand input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }

    [GraphQLType(typeof(UpdateKanbanProjectResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<UpdateKanbanProjectResponse> UpdateKanbanProject(
        UpdateKanbanProjectCommand input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }

    [GraphQLType(typeof(CreateTaskResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<CreateTaskResponse> CreateTask(
        CreateTaskCommand input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }

    [GraphQLType(typeof(AssignTaskResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<AssignTaskResponse> AssignTask(
        AssignTaskCommand input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }
}