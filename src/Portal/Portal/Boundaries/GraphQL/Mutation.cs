using System.Diagnostics.CodeAnalysis;
using HotChocolate;
using HotChocolate.Authorization;
using MediatR;
using Portal.Boundaries.GraphQL.InputTypes;
using Portal.Boundaries.GraphQL.ResponseTypes;
using Portal.UseCases.Mutations;
using Portal.UseCases.Responses;
using Shared.CustomTypes;

namespace Portal.Boundaries.GraphQL;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Mutation
{
    [GraphQLType(typeof(CreateProjectResponseType))]
    [Authorize(AuthorizationPolicy.ADMIN_ACCESS)]
    public async Task<CreateProjectResponse> CreateProject(
        [GraphQLType(typeof(CreateProjectInputType))]
        CreateProjectCommand createProjectInput,
        [Service] ISender mediator)
    {
        return await mediator.Send(createProjectInput);
    }
}