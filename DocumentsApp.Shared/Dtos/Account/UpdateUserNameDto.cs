namespace DocumentsApp.Shared.Dtos.Account;

public class UpdateUserNameDto
{
    public string NewUserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}