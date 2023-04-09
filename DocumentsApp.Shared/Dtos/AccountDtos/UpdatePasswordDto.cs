namespace DocumentsApp.Shared.Dtos.AccountDtos;

public class UpdatePasswordDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}