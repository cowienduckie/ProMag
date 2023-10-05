using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string username) : base(username)
    {
    }
}