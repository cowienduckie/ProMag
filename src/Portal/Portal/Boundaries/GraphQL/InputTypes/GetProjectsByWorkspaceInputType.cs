using HotChocolate.Types;
using Portal.UseCases.Queries;
using Shared.Common.Helpers;

namespace Portal.Boundaries.GraphQL.InputTypes;

public class GetProjectsByWorkspaceInputType : InputObjectType<GetProjectsByWorkspaceQuery>
{
    private static readonly string _inputName = nameof(GetProjectsByWorkspaceInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<GetProjectsByWorkspaceQuery> descriptor)
    {
        descriptor.Name(_inputName);
    }
}