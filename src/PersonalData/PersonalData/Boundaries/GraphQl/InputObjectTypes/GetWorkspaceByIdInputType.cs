using HotChocolate.Types;
using PersonalData.UseCases.Queries;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class GetWorkspaceByIdInputType : InputObjectType<GetWorkspaceByIdQuery>
{
    private static readonly string _inputName = nameof(GetWorkspaceByIdInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<GetWorkspaceByIdQuery> descriptor)
    {
        descriptor.Name(_inputName);
    }
}