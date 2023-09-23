namespace Shared.Email;

public interface IEmailClient
{
    EmailData Data { get; }

    IEmailClient To(string emailAddress);

    IEmailClient From(string emailAddress);

    IEmailClient Cc(string emailAddress);

    IEmailClient Bcc(string emailAddress);

    IEmailClient Subject(string subject);

    IEmailClient Body(string body, bool isHtml = false);

    Task SendAsync(CancellationToken token = default);
}