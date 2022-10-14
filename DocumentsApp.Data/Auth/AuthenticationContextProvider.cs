using System.Security.Claims;
using DocumentsApp.Data.Auth.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DocumentsApp.Data.Auth;

public class AuthenticationContextProvider : IAuthenticationContextProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public AuthenticationContextProvider(
        ProtectedSessionStorage sessionStorage
    )
    {
        _sessionStorage = sessionStorage;
    }

    public async Task<string?> GetUserId()
    {
        var idClaim = (await GetAuthenticationStateAsync()).User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        return idClaim?.Value;
    }

    public async Task<string?> GetUserEmail()
    {
        var emailClaim = (await GetAuthenticationStateAsync()).User.FindFirst(c => c.Type == ClaimTypes.Email);
        return emailClaim?.Value;
    }

    public async Task<string?> GetUserName()
    {
        var nameClaim = (await GetAuthenticationStateAsync()).User.FindFirst(c => c.Type == ClaimTypes.Name);
        return nameClaim?.Value;
    }

    private async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

            if (userSession is null) return new AuthenticationState(_anonymous);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userSession.Id),
                new Claim(ClaimTypes.Name, userSession.Username),
                new Claim(ClaimTypes.Email, userSession.Email)
            }, "CustomAuth"));
            return new AuthenticationState(claimsPrincipal);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }

    }

}