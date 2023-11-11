using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account;

public class LoginInputModel
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    public bool RememberLogin { get; set; }

    public string ReturnUrl { get; set; } = default!;
}