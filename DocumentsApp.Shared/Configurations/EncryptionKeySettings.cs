namespace DocumentsApp.Shared.Configurations;

public class EncryptionKeySettings
{
    public TimeSpan EmailConfirmationDurationDays { get; set; }
    public TimeSpan PasswordResetDurationDays { get; set; }
}