using MediatR;
using PersonalData.Boundaries.GraphQl.Dtos;
using PersonalData.Data.Audit;
using Shared.Common.Enums;

namespace PersonalData.UseCases.Commands;

[ActivityLog]
public class EditUserCommand : IRequest<PersonDto>
{
    public string PersonId { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = default!;

    public string? Alias { get; set; }

    public string? PhotoPath { get; set; }

    public UserStatus UserStatus { get; set; }

    public AddressDto Address { get; set; } = default!;

    public PersonInfoDto PersonInfo { get; set; } = default!;

    public string? Language { get; set; }

    public string? Timezone { get; set; }

    public string? CountryLocale { get; set; }

    public IList<string> RoleIds { get; set; } = default!;
}