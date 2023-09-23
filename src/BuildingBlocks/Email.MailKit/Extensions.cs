using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promag.Shared.Email;
using Promag.Shared.Email.Implementations;

namespace Email.MailKit;

public static class Extensions
{
    public static IServiceCollection AddMailKit(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailKitOptions>(configuration.GetSection("MailKit"));
        services.AddSingleton(new SmtpClient());
        services.AddSingleton<IEmailSender, MailKitSender>();
        services.AddTransient<IEmailClient, EmailClient>();

        return services;
    }
}