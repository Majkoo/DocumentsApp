using System.Security.Claims;
using DocumentsApp.Shared.Exceptions;

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

    public string GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new UnauthorizedException("Invalid token");
    }

    public string GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ??
               throw new UnauthorizedException("Invalid token");
    }

    public string GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name) ??
               throw new UnauthorizedException("Invalid token");
    }
}