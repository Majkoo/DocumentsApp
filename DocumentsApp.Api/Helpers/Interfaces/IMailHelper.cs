using MimeKit;

namespace DocumentsApp.Api.Helpers.Interfaces;

public interface IMailHelper
{
    MimeMessage GetEmailConfirmationMessage(string userEmail, string encryptedCredentials);
    MimeMessage GetPasswordResetMessage(string userEmail, string encryptedCredentials);
}