using DocumentsApp.Api.Models;
using MimeKit;

namespace DocumentsApp.Api.Helpers.Interfaces;

public interface IMailHelper
{
    MimeMessage GetEmailConfirmationMessage(string userEmail, AccountSecurityData accountSecurityData);
    MimeMessage GetPasswordResetMessage(string userEmail, AccountSecurityData accountSecurityData);
}