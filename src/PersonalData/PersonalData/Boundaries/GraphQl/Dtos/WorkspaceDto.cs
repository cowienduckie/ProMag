namespace PersonalData.Boundaries.GraphQl.Dtos;

public class WorkspaceDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string OwnerId { get; set; } = default!;
    public IEnumerable<PersonDto> Members { get; set; } = default!;
    public IEnumerable<PersonDto> Invitations { get; set; } = default!;
}