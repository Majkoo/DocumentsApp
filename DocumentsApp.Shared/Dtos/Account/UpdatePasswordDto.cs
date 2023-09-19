namespace DocumentsApp.Shared.Dtos.Account;

public class UpdatePasswordDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    
    public string ConfirmNewPassword { get; set; }
}