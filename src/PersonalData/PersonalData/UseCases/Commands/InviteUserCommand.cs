using MediatR;
using PersonalData.Data.Audit;
using PersonalData.UseCases.Responses;

namespace PersonalData.UseCases.Commands;

[ActivityLog]
public class InviteUserCommand : IRequest<InviteUserResponse>
{
    public string UserName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public IList<string> RoleIds { get; set; } = default!;
}