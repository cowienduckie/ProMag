using HotChocolate;
using HotChocolate.Types;
using MediatR;
using Portal.UseCases.Responses;

namespace Portal.UseCases.Mutations;

public class UpdateKanbanProjectCommand : IRequest<UpdateKanbanProjectResponse>
{
    public UpdateKanbanProjectCommand(string projectId, List<string> columnOrder, IDictionary<string, object> tasks)
    {
        ProjectId = projectId;
        ColumnOrder = columnOrder;
        Tasks = tasks;
    }

    public string ProjectId { get; }
    public List<string> ColumnOrder { get; }

    [GraphQLType(typeof(AnyType))]
    public IDictionary<string, object> Tasks { get; }
}