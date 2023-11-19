using MediatR;
using Portal.UseCases.Responses;

namespace Portal.UseCases.Mutations;

public class CreateProjectCommand : IRequest<CreateProjectResponse>
{
    public string Name { get; set; } = default!;
    public string? Notes { get; set; }
    public string? Color { get; set; }
    public DateTime? DueDate { get; set; }
}