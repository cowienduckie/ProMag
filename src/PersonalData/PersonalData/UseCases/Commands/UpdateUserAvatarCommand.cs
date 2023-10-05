using MediatR;
using PersonalData.Data.Audit;

namespace PersonalData.UseCases.Commands;

[ActivityLog]
public class UpdateUserAvatarCommand : IRequest<bool>
{
    public Guid PersonId { get; set; }
    public string AvatarFileName { get; set; } = default!;
}