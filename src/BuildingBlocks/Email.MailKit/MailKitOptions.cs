namespace Email.MailKit;

public class MailKitOptions
{
    public string SmtpHost { get; set; } = default!;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
}