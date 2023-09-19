using DocumentsApp.Api.Models;
using DocumentsApp.Api.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DocumentsApp.Api.Services;

public class MailService : IMailService
{
    private readonly MailConfig _config;

    public MailService(IOptions<MailConfig> settings)
    {
        _config = settings.Value;
    }

    public void SendMessageAsync(MimeMessage message)
    {
        using var smtp = new SmtpClient();
        if (_config.UseSsl)
        {
            try
            {
                smtp.Connect(_config.Host, _config.Port,
                    _config.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect);
            }
            catch (NotSupportedException)
            {
                smtp.Connect(_config.Host, _config.Port, SecureSocketOptions.SslOnConnect);
                throw;
            }
        }
        else
        {
            smtp.Connect(_config.Host, _config.Port, SecureSocketOptions.None);
        }

        smtp.Authenticate(_config.UserName, _config.Password);
        smtp.Send(message);
        smtp.Disconnect(true);
    }
}