namespace DocumentsApp.Api.Models;

public class JwtConfig
{
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationMinutes { get; set; } = 15;
    public int RefreshExpirationHours { get; set; } = 72;
}