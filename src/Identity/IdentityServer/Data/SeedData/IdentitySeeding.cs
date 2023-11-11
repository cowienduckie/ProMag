using System.Collections.Immutable;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Models.DbModel;
using Microsoft.AspNetCore.Identity;
using Shared.CustomTypes;

namespace IdentityServer.Data.SeedData;

public static class IdentitySeeding
{
    private const string SUPER_USER_NAME = "superuser";
    private const string PASSWORD = "P@55w0rD";

    private static readonly IImmutableDictionary<string, ApplicationUser> _users = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, ApplicationUser>(SUPER_USER_NAME, new ApplicationUser
        {
            UserName = SUPER_USER_NAME,
            Email = "superuser@nomail.com",
            EmailConfirmed = true
        })
    });

    private static readonly IImmutableDictionary<string, Claim[]> _userClaims = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, Claim[]>(SUPER_USER_NAME, new Claim[]
        {
            new(JwtClaimTypes.Name, "Super User"),
            new(JwtClaimTypes.GivenName, "Super"),
            new(JwtClaimTypes.FamilyName, "User"),
            new(JwtClaimTypes.Email, "superuser@nomail.com'")
        })
    });

    private static readonly IImmutableDictionary<string, string[]> _userRoles = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, string[]>(SUPER_USER_NAME, new[]
        {
            Roles.SUPER_USER_ROLE_NAME
        })
    });

    private static readonly IImmutableDictionary<string, ApplicationRole> _roles = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, ApplicationRole>(Roles.ADMIN_ROLE_NAME, new ApplicationRole
        {
            Name = Roles.ADMIN_ROLE_NAME,
            DisplayName = "Administrator",
            Description = "Administrator role"
        }),
        new KeyValuePair<string, ApplicationRole>(Roles.ONLINE_USER_ROLE_NAME, new ApplicationRole
        {
            Name = Roles.ONLINE_USER_ROLE_NAME,
            DisplayName = "Online User",
            Description = "Online User role"
        }),
        new KeyValuePair<string, ApplicationRole>(Roles.MEMBER_ROLE_NAME, new ApplicationRole
        {
            Name = Roles.MEMBER_ROLE_NAME,
            DisplayName = "Member",
            Description = "Member role"
        }),
        new KeyValuePair<string, ApplicationRole>(Roles.SUPER_USER_ROLE_NAME, new ApplicationRole
        {
            Name = Roles.SUPER_USER_ROLE_NAME,
            DisplayName = "Super User",
            Description = "Super User role"
        })
    });

    private static readonly IImmutableDictionary<string, Claim[]> _roleClaims = ImmutableDictionary.CreateRange(new[]
    {
        new KeyValuePair<string, Claim[]>(Roles.ADMIN_ROLE_NAME, new Claim[]
        {
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PROFILE_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.USER_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.CONTACT_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.ROLE_FULL)
        }),
        new KeyValuePair<string, Claim[]>(Roles.ONLINE_USER_ROLE_NAME, new Claim[]
        {
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.USER_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.CONTACT_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.ROLE_VIEW)
        }),
        new KeyValuePair<string, Claim[]>(Roles.MEMBER_ROLE_NAME, new Claim[]
        {
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PROFILE_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.USER_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.USER_CREATE),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.USER_DELETE),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_CREATE),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_DELETE),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.CONTACT_CREATE),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.CONTACT_VIEW),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.CONTACT_DELETE),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.ROLE_VIEW)
        }),
        new KeyValuePair<string, Claim[]>(Roles.SUPER_USER_ROLE_NAME, new Claim[]
        {
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PROFILE_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.USER_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.PERSON_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.CONTACT_FULL),
            new(Permissions.PERMISSION_CLAIM_TYPE, Permissions.ROLE_FULL)
        })
    });

    public static async Task SpreadUsers(UserManager<ApplicationUser> userManager)
    {
        foreach (var userName in _users.Keys)
        {
            await SeedUser(userManager, userName);
        }
    }

    public static async Task SpreadRoles(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var roleName in _roles.Keys)
        {
            await SeedRole(roleManager, roleName);
        }
    }

    private static async Task SeedUser(UserManager<ApplicationUser> userManager, string userName)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user == null)
        {
            user = _users[userName];

            var result = await userManager.CreateAsync(user, PASSWORD);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userManager.AddClaimsAsync(user, _userClaims[userName]);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            foreach (var roleName in _userRoles[userName])
            {
                result = await userManager.AddToRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }

    private static async Task SeedRole(RoleManager<ApplicationRole> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            role = _roles[roleName];

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            foreach (var claim in _roleClaims[roleName])
            {
                result = await roleManager.AddClaimAsync(role, claim);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }
}