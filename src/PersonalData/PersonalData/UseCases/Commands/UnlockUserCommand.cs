using MediatR;
using PersonalData.Data.Audit;

namespace PersonalData.UseCases.Commands;

[ActivityLog]
public class UnlockUserCommand : IRequest<bool>
{
    public string PersonId { get; set; } = default!;
}