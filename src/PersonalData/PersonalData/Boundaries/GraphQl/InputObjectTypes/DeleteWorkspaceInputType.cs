using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class DeleteWorkspaceInputType : InputObjectType<DeleteWorkspaceCommand>
{
    private static readonly string _inputName = nameof(DeleteWorkspaceInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<DeleteWorkspaceCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}