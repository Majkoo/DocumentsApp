using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Api.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DocumentsApp.Api.Helpers;

public class MailHelper : IMailHelper
{
    private readonly MailConfig _config;

    public MailHelper(IOptions<MailConfig> settings)
    {
        _config = settings.Value;
    }

    public MimeMessage GetEmailConfirmationMessage(string userEmail, AccountSecurityData accountSecurityData)
    {
        var uriBuilder = new UriBuilder()
        {
            Host = _config.BaseUrl,
            Path = _config.ConfirmEmailUrl,
            Query = $"id={accountSecurityData.AccountId}&token={accountSecurityData.Token}"
        };

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
                        <a href=""{uriBuilder.Uri}"">Click here</a>
                        to confirm your email.
                    </p>
                </body>
            </html>";

        return CreateMessage(userEmail, html, subject);
    }

    public MimeMessage GetPasswordResetMessage(string userEmail, AccountSecurityData accountSecurityData)
    {
        var uriBuilder = new UriBuilder()
        {
            Host = _config.BaseUrl,
            Path = _config.ConfirmEmailUrl,
            Query = $"id={accountSecurityData.AccountId}&token={accountSecurityData.Token}"
        };

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
                        <a href=""{uriBuilder.Uri}"">Click here</a>
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
        message.From.Add(MailboxAddress.Parse(_config.From));
        message.To.Add(MailboxAddress.Parse(userEmail));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = html };

        return message;
    }
}