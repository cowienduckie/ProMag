using HotChocolate.Types;
using Portal.UseCases.Queries;
using Shared.Common.Helpers;

namespace Portal.Boundaries.GraphQL.InputTypes;

public class GetKanbanProjectByIdInputType : InputObjectType<GetKanbanProjectByIdQuery>
{
    private static readonly string _inputName = nameof(GetKanbanProjectByIdInputType).RemoveSuffix("Type");

    protected override void Configure(IInputObjectTypeDescriptor<GetKanbanProjectByIdQuery> descriptor)
    {
        descriptor.Name(_inputName);
    }
}