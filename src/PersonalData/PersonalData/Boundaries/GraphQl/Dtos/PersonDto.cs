using PersonalData.Common.Enums;

namespace PersonalData.Boundaries.GraphQl.Dtos;

public class PersonDto
{
    public string Id { get; set; } = default!;
    public string ActorId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = default!;
    public string? Alias { get; set; }
    public string? PhotoPath { get; set; }
    public string Email { get; set; } = default!;
    public string? Language { get; set; }
    public string? Timezone { get; set; }
    public string? CountryLocale { get; set; }
    public UserStatus UserStatus { get; set; }
    public AddressDto Address { get; set; } = default!;
    public PersonInfoDto PersonInfo { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string LastModifiedBy { get; set; } = default!;
}