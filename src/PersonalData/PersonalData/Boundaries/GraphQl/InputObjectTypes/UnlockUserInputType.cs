using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class UnlockUserInputType : InputObjectType<UnlockUserCommand>
{
    private static readonly string _inputName = nameof(UnlockUserInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<UnlockUserCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}