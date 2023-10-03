namespace DocumentsApp.Shared.Dtos.Account;

public class ResetPasswordDto
{
    public string AccountId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmNewPassword { get; set; } = null!;
}