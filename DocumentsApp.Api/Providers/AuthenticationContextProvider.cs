using System.Security.Claims;

namespace DocumentsApp.Api.Providers;

public class AuthenticationContextProvider : IAuthenticationContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationContextProvider(
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    
    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
    }

    public string? GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

    }

}