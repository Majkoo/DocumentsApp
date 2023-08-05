using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Shared.Configurations;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DocumentsApp.Api.Helpers;

public class MailHelper : IMailHelper
{
    private readonly MailSettings _settings;

    public MailHelper(IOptions<MailSettings> settings)
    {
        _settings = settings.Value;
    }

    public MimeMessage GetEmailConfirmationMessage(string userEmail, string encryptedCredentials)
    {
        var link = Path.
            Combine(_settings.BaseUrl!, $"/auth/confirmemail?encrypted={Uri.EscapeDataString(encryptedCredentials)}"
            );

        const string subject = "DocumentsApp Email Confirmation";
        var html =
            @$"<!DOCTYPE html>
            <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                </head>
                <body>
                    <h3>Welcome to DocumentsApp!</h3>
                    <p>
                        <a href=""{link}"">Click here</a>
                        to confirm your email.
                    </p>
                </body>
            </html>";

        return CreateMessage(userEmail, html, subject);
    }

    public MimeMessage GetPasswordResetMessage(string userEmail, string encryptedCredentials)
    {
        var link = Path.
            Combine(_settings.BaseUrl!, $"/auth/confirmemail?encrypted={Uri.EscapeDataString(encryptedCredentials)}"
            );
        
        const string subject = "DocumentsApp Password Reset";
        var html =
            @$"<!DOCTYPE html>
            <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                </head>
                <body>
                    <h3>DocumentsApp password reset</h3>
                    <p>
                        <a href=""{link}"">Click here</a>
                        to reset your password.
                    </p>
                    <p>If you didn't request this password reset, contact service administrators.</p>
                </body>
            </html>";

        return CreateMessage(userEmail, html, subject);
    }

    private MimeMessage CreateMessage(string userEmail, string html, string subject)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_settings.From));
        message.To.Add(MailboxAddress.Parse(userEmail));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = html };

        return message;
    }
}