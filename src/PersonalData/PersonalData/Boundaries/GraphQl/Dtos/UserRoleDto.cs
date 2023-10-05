namespace PersonalData.Boundaries.GraphQl.Dtos;

public class UserRoleDto
{
    public string UserId { get; set; } = default!;
    public string RoleId { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}