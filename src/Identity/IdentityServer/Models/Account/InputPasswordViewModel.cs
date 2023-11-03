using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account;

public class InputPasswordViewModel
{
    public string Token { get; set; } = default!;

    public string UserId { get; set; } = default!;

    public string? UserName { get; set; }

    [Required]
    public string Password { get; set; } = default!;

    [Compare("Password")]
    public string ConfirmPassword { get; set; } = default!;

    public string? TokenPurpose { get; set; }
}