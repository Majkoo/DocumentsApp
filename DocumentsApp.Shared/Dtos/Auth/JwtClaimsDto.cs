namespace DocumentsApp.Shared.Dtos.Auth;

public class JwtClaimsDto
{
    public string Username { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}