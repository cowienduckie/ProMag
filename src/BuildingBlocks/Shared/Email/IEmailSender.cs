namespace Shared.Email;

public interface IEmailSender
{
    Task SendAsync(EmailData email, CancellationToken token = default);
}