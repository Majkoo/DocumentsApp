namespace DocumentsApp.Shared.Dtos.AccountDtos;

public class UpdateUserPasswordDto
{
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
    public string Password { get; set; }
}