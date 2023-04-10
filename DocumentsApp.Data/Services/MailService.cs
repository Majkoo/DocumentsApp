using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Configurations;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DocumentsApp.Data.Services;

public class MailService : IMailService
{
    private readonly MailSettings _settings;

    public MailService(IOptions<MailSettings> settings)
    {
        _settings = settings.Value;
    }
    public void SendMessageAsync(string to, string subject, string html, string from = null)
    {
        // create email message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(string.IsNullOrEmpty(from) ? _settings.From : from));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();
        smtp.Connect(_settings.Host, _settings.Port,
            _settings.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect);
        smtp.Authenticate(_settings.UserName, _settings.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}