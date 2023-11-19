using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class LockUserInputType : InputObjectType<LockUserCommand>
{
    private static readonly string _inputName = nameof(LockUserInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<LockUserCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}