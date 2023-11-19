using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;
using Portal.Boundaries.GraphQL.Dtos;
using Portal.Boundaries.GraphQL.Filters;
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
    [UseOffsetPaging(typeof(SimplifiedProjectType))]
    [UseFiltering(typeof(SimplifiedProjectFilter))]
    [Authorize(AuthorizationPolicy.ADMIN_MEMBER_ACCESS)]
    public async Task<IQueryable<SimplifiedProjectDto>> GetProjects([Service] ISender mediator)
    {
        return await mediator.Send(new GetProjectsQuery());
    }
}