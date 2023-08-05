namespace DocumentsApp.Shared.Dtos.Auth;

public class JwtDataDto
{
    public string AuthToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public JwtClaimsDto Claims { get; set; } = null!;
}