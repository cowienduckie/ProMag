using MediatR;
using PersonalData.Data.Audit;

namespace PersonalData.UseCases.Commands;

[ActivityLog]
public class LockUserCommand : IRequest<bool>
{
    public Guid PersonId { get; set; }
}