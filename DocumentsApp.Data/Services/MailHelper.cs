using System.Security.Cryptography;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DocumentsApp.Data.Services;

public class MailHelper : IMailHelper
{
    private readonly UserManager<Account> _userManager;
    private readonly NavigationManager _navigationManager;
    private readonly IAesCipher _aesCipher;
    private readonly IEncryptionKeyService _keyService;
    private readonly MailSettings _settings;

    public MailHelper(IOptions<MailSettings> settings, UserManager<Account> userManager,
        NavigationManager navigationManager, IAesCipher aesCipher, IEncryptionKeyService keyService)
    {
        _userManager = userManager;
        _navigationManager = navigationManager;
        _aesCipher = aesCipher;
        _keyService = keyService;
        _settings = settings.Value;
    }

    public async Task<MimeMessage> GetEmailConfirmationMessageAsync(string userEmail)
    {
        //TODO move encryption to AccountService
        var user = await _userManager.FindByEmailAsync(userEmail);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encryptedKey = await _keyService.GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum.EmailConfirmation);
        var toEncrypt = $"{userEmail}&{token}";
        
        if (user is null)
            throw new NotFoundException("No user with such id");
        
        var encrypted =
            Uri.EscapeDataString(Convert.ToBase64String(_aesCipher.EncryptString(toEncrypt, encryptedKey.Key, encryptedKey.Vector)));

        var link = _navigationManager.ToAbsoluteUri(
            $"/auth/confirmemail?encrypted={encrypted}");

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

    public async Task<MimeMessage> GetPasswordResetMessageAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        
        if (user is null)
            throw new NotFoundException("No user with such id");
        
        var token = Uri.EscapeDataString(await _userManager.GeneratePasswordResetTokenAsync(user));
        var link = _navigationManager.ToAbsoluteUri($"/auth/resetpassword?token={token}&email={userEmail}");

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