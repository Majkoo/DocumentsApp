namespace DocumentsApp.Api.Model;

public class SignInConfig
{
    public bool RequireConfirmedEmail { get; set; } = false;
    public bool RequireConfirmedAccount { get; set; } = false;
    public bool RequireConfirmedPhoneNumber { get; set; } = false;
}