using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.SecurityContext.Implementations;

public class SecurityContextAccessor : ISecurityContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SecurityContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("sub");

            return claim?.Value;
        }
    }

    public string? Username
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email);

            return claim?.Value;
        }
    }

    public IEnumerable<string> Roles
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var roles = user?.FindAll(ClaimTypes.Role).Select<Claim, string>(x => x.Value);

            return roles ?? Enumerable.Empty<string>();
        }
    }

    public IEnumerable<string> Permissions
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var permissions = user?.FindAll(CustomTypes.Permissions.PERMISSION_CLAIM_TYPE).Select<Claim, string>(x => x.Value);

            return permissions ?? Enumerable.Empty<string>();
        }
    }

    public string? IpAddressClient
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();

            return ipAddress;
        }
    }
}