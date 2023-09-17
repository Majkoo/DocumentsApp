using MimeKit;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IMailService
{
    void SendMessageAsync(MimeMessage message);
}