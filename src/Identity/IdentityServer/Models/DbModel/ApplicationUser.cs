using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models.DbModel;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string username) : base(username)
    {
    }
}