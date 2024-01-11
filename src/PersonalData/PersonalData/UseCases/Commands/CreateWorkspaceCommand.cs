using MediatR;
using PersonalData.UseCases.Responses;

namespace PersonalData.UseCases.Commands;

public class CreateWorkspaceCommand : IRequest<CreateWorkspaceResponse>
{
    public string Name { get; set; } = default!;
}