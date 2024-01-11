using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class CreateWorkspaceInputType : InputObjectType<CreateWorkspaceCommand>
{
    private static readonly string _inputName = nameof(CreateWorkspaceInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<CreateWorkspaceCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}