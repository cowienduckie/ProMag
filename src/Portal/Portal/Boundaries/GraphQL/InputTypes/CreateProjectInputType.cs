using HotChocolate.Types;
using Portal.UseCases.Mutations;
using Shared.Common.Helpers;

namespace Portal.Boundaries.GraphQL.InputTypes;

public class CreateProjectInputType : InputObjectType<CreateProjectCommand>
{
    private static readonly string _inputName = nameof(CreateProjectInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<CreateProjectCommand> descriptor)
    {
        descriptor.Name(_inputName);
    }
}