using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Extensions;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class InviteUserInputType : InputObjectType<InviteUserCommand>
{
    private static readonly string _inputName = nameof(InviteUserInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<InviteUserCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}