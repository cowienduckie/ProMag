using MediatR;
using PersonalData.UseCases.Responses;

namespace PersonalData.UseCases.Commands;

public class RegisterUserCommand : IRequest<RegisterUserResponse>
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}