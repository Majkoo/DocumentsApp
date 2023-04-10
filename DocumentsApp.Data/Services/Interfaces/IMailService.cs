namespace DocumentsApp.Data.Services.Interfaces;

public interface IMailService
{
    void SendMessageAsync(string to, string subject, string html, string from = null);
}