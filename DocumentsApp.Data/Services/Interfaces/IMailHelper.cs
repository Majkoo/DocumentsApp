using MimeKit;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IMailHelper
{
    MimeMessage GetEmailConfirmationMessage(string userEmail, string encryptedCredentials);
    MimeMessage GetPasswordResetMessage(string userEmail, string encryptedCredentials);
}