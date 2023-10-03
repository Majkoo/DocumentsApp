namespace DocumentsApp.Shared.Dtos.Account;

public class GetAccountDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
}