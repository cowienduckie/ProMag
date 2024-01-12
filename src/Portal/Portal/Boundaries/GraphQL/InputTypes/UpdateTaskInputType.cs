using HotChocolate.Types;
using Portal.UseCases.Mutations;
using Shared.Common.Helpers;

namespace Portal.Boundaries.GraphQL.InputTypes;

public class UpdateTaskInputType : InputObjectType<UpdateTaskCommand>
{
    private static readonly string _inputName = nameof(UpdateTaskInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<UpdateTaskCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}