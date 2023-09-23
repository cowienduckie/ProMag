using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using Email.MailKit;
using Promag.Shared.Email;
using Promag.Shared.Email.Implementations;
using AuthenticationException = System.Security.Authentication.AuthenticationException;

namespace Email.MailKit.Tests;

public class MailKitSenderTests
{
    private readonly MailKitOptions _mailkitOptions = new()
    {
        SmtpHost = "fakeSMTP",
        Port = 1025,
        Username = "fakeUser",
        Password = "fakePwd",
        Email = "fake@email.com"
    };

    [Fact]
    public void CanAddMultipleTo()
    {
        var sender = Mock.Of<IEmailSender>();
        var emailClient = new EmailClient(sender);

        emailClient.To("user1@nomail.com;user2@nomail.com");

        Assert.Equal(2, emailClient.Data.ToAddress.Count);
        Assert.Equal("user1@nomail.com", emailClient.Data.ToAddress[0]);
    }

    [Fact]
    public void CanAddMultipleCc()
    {
        var sender = Mock.Of<IEmailSender>();
        var emailClient = new EmailClient(sender);

        emailClient.Cc("user1@nomail.com;user2@nomail.com");

        Assert.Equal(2, emailClient.Data.CcAddress.Count);
        Assert.Equal("user1@nomail.com", emailClient.Data.CcAddress[0]);
    }

    [Fact]
    public void CanAddMultipleBcc()
    {
        var sender = Mock.Of<IEmailSender>();
        var emailClient = new EmailClient(sender);

        emailClient.Bcc("user1@nomail.com;user2@nomail.com");

        Assert.Equal(2, emailClient.Data.BccAddress.Count);
        Assert.Equal("user1@nomail.com", emailClient.Data.BccAddress[0]);
    }

    [Fact]
    public void CanSetEmailSubject()
    {
        var sender = Mock.Of<IEmailSender>();
        var emailClient = new EmailClient(sender);
        const string subject = "Test email !!!";

        emailClient.Subject(subject);

        Assert.NotNull(emailClient.Data);
        Assert.Equal(subject, emailClient.Data.Subject);
    }

    [Fact]
    public async Task Should_SendMail_Success()
    {
        var mailKitOptions = Mock.Of<IOptionsMonitor<MailKitOptions>>(f => f.CurrentValue == _mailkitOptions);
        var mailKitClient = Mock.Of<SmtpClient>();

        var emailClient = new EmailClient(new MailKitSender(mailKitClient, mailKitOptions))
            .To("user1@nomail.com")
            .Subject("Test email !!!")
            .Body("Test send email");

        await emailClient.SendAsync();

        var mailKitClientMock = Mock.Get(mailKitClient);

        mailKitClientMock.Verify(
            x => x.ConnectAsync(_mailkitOptions.SmtpHost, _mailkitOptions.Port, SecureSocketOptions.Auto, It.IsAny<CancellationToken>()), Times.Once);
        mailKitClientMock.Verify(x => x.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>(), null), Times.Once);
        mailKitClientMock.Verify(x => x.DisconnectAsync(true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowAuthenticationException_IfInvalidCredential()
    {
        var mailKitOptions = Mock.Of<IOptionsMonitor<MailKitOptions>>(f => f.CurrentValue == _mailkitOptions);
        var mailKitClient = Mock.Of<SmtpClient>();

        Mock.Get(mailKitClient).Setup(f => f.AuthenticateAsync(It.IsAny<SaslMechanismPlain>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AuthenticationException());

        var emailClient = new EmailClient(new MailKitSender(mailKitClient, mailKitOptions))
            .To("user1@nomail.com")
            .Body("Test send email");

        await Assert.ThrowsAsync<AuthenticationException>(async () => await emailClient.SendAsync());
    }
}