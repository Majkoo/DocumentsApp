using System.Security.Claims;
using DocumentsApp.Data.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DocumentsApp.Data.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(
        ProtectedSessionStorage sessionStorage
        )
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

            if (userSession is null) return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userSession.Id),
                new Claim(ClaimTypes.Name, userSession.Username),
                new Claim(ClaimTypes.Email, userSession.Email)
            }, "CustomAuth"));
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }

    }
    public async Task UpdateAuthenticationState(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession != null)
        {
            await _sessionStorage.SetAsync("UserSession", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userSession.Id),
                new Claim(ClaimTypes.Name, userSession.Username),
                new Claim(ClaimTypes.Email, userSession.Email)
            }, "CustomAuth"));

        }
        else
        {
            await _sessionStorage.DeleteAsync("UserSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
    public async Task MarkUserAsSignedOut()
    {
        await UpdateAuthenticationState(null);
    }
    public async Task MarkUserAsSignedIn(Account account)
    {
        var userSession = new UserSession()
        {
            Id = account.Id,
            Email = account.Email,
            Username = account.UserName,
        };
        await UpdateAuthenticationState(userSession);
    }


    public async Task<string?> GetUserId()
    {
        var idClaim = GetAuthenticationStateAsync().Result.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        return await Task.FromResult(idClaim?.Value);
    }

    public async Task<string?> GetUserEmail()
    {
        var emailClaim = GetAuthenticationStateAsync().Result.User.FindFirst(c => c.Type == ClaimTypes.Email);
        return await Task.FromResult(emailClaim?.Value);
    }

    public async Task<string?> GetUserName()
    {
        var nameClaim = GetAuthenticationStateAsync().Result.User.FindFirst(c => c.Type == ClaimTypes.Name);
        return await Task.FromResult(nameClaim?.Value);
    }
}