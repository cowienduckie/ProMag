using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class InviteUserToWorkspaceInputType : InputObjectType<InviteUserToWorkspaceCommand>
{
    private static readonly string _inputName = nameof(InviteUserToWorkspaceInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<InviteUserToWorkspaceCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}