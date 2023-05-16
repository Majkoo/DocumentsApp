using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DocumentsApp.Data.Services;

public class MailService : IMailService
{
    private readonly MailSettings _settings;

    public MailService(IOptions<MailSettings> settings)
    {
        _settings = settings.Value;
    }
    public void SendMessageAsync(MimeMessage message)
    {
        using var smtp = new SmtpClient();
        smtp.Connect(_settings.Host, _settings.Port,
            _settings.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect);
        smtp.Authenticate(_settings.UserName, _settings.Password);
        smtp.Send(message);
        smtp.Disconnect(true);
    }
}