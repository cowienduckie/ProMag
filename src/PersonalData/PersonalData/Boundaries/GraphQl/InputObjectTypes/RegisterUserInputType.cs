using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class RegisterUserInputType : InputObjectType<RegisterUserCommand>
{
    private static readonly string _inputName = nameof(RegisterUserInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<RegisterUserCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}