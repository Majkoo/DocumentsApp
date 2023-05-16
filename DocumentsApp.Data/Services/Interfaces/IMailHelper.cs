using MimeKit;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IMailHelper
{
    Task<MimeMessage> GetEmailConfirmationMessageAsync(string userEmail);
    Task<MimeMessage> GetPasswordResetMessageAsync(string userEmail);
}