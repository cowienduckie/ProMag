using PersonalData.Common.Enums;
using Shared.Common.Enums;
using Shared.Domain;

namespace PersonalData.Domain;

public class Person : AuditableEntity
{
    public Guid ActorId { get; set; }
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = default!;
    public string? Alias { get; set; }
    public string? PhotoPath { get; set; }
    public UserStatus UserStatus { get; set; }
    public UserType UserType { get; set; }
    public string Email { get; set; } = default!;
    public string? Fax { get; set; }
    public string? LandLineNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Website { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? CountryLocale { get; set; }
    public string? Language { get; set; }
    public string? Timezone { get; set; }

    public ICollection<Team> Teams { get; set; } = default!;
    public ICollection<Workspace> Workspaces { get; set; } = default!;
}