using MimeKit;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IMailService
{
    void SendMessageAsync(MimeMessage message);
}