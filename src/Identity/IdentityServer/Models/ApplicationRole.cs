using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class ApplicationRole : IdentityRole
{
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
}