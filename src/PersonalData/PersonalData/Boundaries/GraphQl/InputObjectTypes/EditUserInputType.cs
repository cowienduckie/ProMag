using HotChocolate.Types;
using PersonalData.UseCases.Commands;
using Shared.Common.Helpers;

namespace PersonalData.Boundaries.GraphQl.InputObjectTypes;

public class EditUserInputType : InputObjectType<EditUserCommand>
{
    private static readonly string _inputName = nameof(EditUserInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<EditUserCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}