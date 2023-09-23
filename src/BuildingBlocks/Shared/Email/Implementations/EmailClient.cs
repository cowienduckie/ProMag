namespace Shared.Email.Implementations;

public class EmailClient : IEmailClient
{
    private readonly IEmailSender _sender;

    public EmailClient(IEmailSender sender)
    {
        Data = new EmailData();
        _sender = sender;
    }

    public EmailData Data { get; }

    public IEmailClient Body(string body, bool isHtml = false)
    {
        Guard.NotNullOrEmpty(body);

        Data.Body = body;
        Data.IsHtml = isHtml;

        return this;
    }

    public IEmailClient From(string emailAddress)
    {
        Guard.NotNullOrEmpty(emailAddress);

        Data.FromAddress = emailAddress;

        return this;
    }

    public IEmailClient Subject(string subject)
    {
        Guard.NotNullOrEmpty(subject);

        Data.Subject = subject;

        return this;
    }

    public IEmailClient To(string emailAddress)
    {
        Guard.NotNullOrEmpty(emailAddress);

        if (emailAddress.Contains(EmailConstants.MultipleEmailAddressSeparator))
        {
            foreach (var address in emailAddress.Split(EmailConstants.MultipleEmailAddressSeparator))
            {
                Data.ToAddress.Add(address);
            }
        }
        else
        {
            Data.ToAddress.Add(emailAddress);
        }

        return this;
    }

    public IEmailClient Bcc(string emailAddress)
    {
        Guard.NotNullOrEmpty(emailAddress);

        if (emailAddress.Contains(EmailConstants.MultipleEmailAddressSeparator))
        {
            foreach (var address in emailAddress.Split(EmailConstants.MultipleEmailAddressSeparator))
            {
                Data.BccAddress.Add(address);
            }
        }
        else
        {
            Data.BccAddress.Add(emailAddress);
        }

        return this;
    }

    public IEmailClient Cc(string emailAddress)
    {
        Guard.NotNullOrEmpty(emailAddress);

        if (emailAddress.Contains(EmailConstants.MultipleEmailAddressSeparator))
        {
            foreach (var address in emailAddress.Split(EmailConstants.MultipleEmailAddressSeparator))
            {
                Data.CcAddress.Add(address);
            }
        }
        else
        {
            Data.CcAddress.Add(emailAddress);
        }

        return this;
    }

    public async Task SendAsync(CancellationToken token = default)
    {
        await _sender.SendAsync(Data, token);
    }
}