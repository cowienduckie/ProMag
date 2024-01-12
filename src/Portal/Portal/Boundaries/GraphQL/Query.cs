using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;
using Portal.Boundaries.GraphQL.Dtos.Projects;
using Portal.Boundaries.GraphQL.Dtos.Tasks;
using Portal.Boundaries.GraphQL.Filters;
using Portal.Boundaries.GraphQL.InputTypes;
using Portal.Boundaries.GraphQL.ObjectTypes;
using Portal.UseCases.Queries;
using Promag.Protobuf.Commons.V1;
using Shared.CustomTypes;

namespace Portal.Boundaries.GraphQL;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Query
{
    [GraphQLName("PortalPing")]
    public async Task<PongReply> Ping([Service] ISender mediator)
    {
        return await mediator.Send(new PingQuery());
    }

    [GraphQLName("Projects")]
    [UseOffsetPaging(typeof(SimplifiedProjectType), IncludeTotalCount = true, MaxPageSize = 9, DefaultPageSize = 9)]
    [UseFiltering(typeof(SimplifiedProjectFilter))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<IQueryable<SimplifiedProjectDto>> GetProjects([Service] ISender mediator)
    {
        return await mediator.Send(new GetProjectsQuery());
    }

    [GraphQLName("GetProjectsByWorkspace")]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<IEnumerable<SimplifiedProjectDto>> GetProjectsByWorkspace(
        [GraphQLType(typeof(GetProjectsByWorkspaceInputType))] GetProjectsByWorkspaceQuery query,
        [Service] ISender mediator)
    {
        return await mediator.Send(query);
    }

    [GraphQLName("KanbanProject")]
    [GraphQLType(typeof(KanbanProjectType))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<KanbanProjectDto?> GetKanbanProjectById(
        GetKanbanProjectByIdQuery input,
        [Service] ISender mediator)
    {
        return await mediator.Send(input);
    }

    [GraphQLName("MyTasks")]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<IEnumerable<SimplifiedTaskDto>> GetKanbanProjectById(
        [Service] ISender mediator)
    {
        return await mediator.Send(new GetMyTaskQuery());
    }
}