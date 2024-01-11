using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class AcceptWorkspaceInvitationInputType : InputObjectType<AcceptWorkspaceInvitationCommand>
{
    private static readonly string _inputName = nameof(AcceptWorkspaceInvitationInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<AcceptWorkspaceInvitationCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}