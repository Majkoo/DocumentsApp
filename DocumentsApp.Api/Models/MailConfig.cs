namespace DocumentsApp.Api.Models;

public class MailConfig
{
    public string? DisplayName { get; set; }
    public string? From { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
    public bool UseSsl { get; set; } = true;
    public bool UseStartTls { get; set; } = true;
    public string? BaseUrl { get; set; }
    public string? ConfirmEmailUrl { get; set; }
    public string? ResetPasswordUrl { get; set; }
}