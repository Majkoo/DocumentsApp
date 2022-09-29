using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DocumentsApp.Data.Services;

public interface IAccountContextService
{
    ClaimsPrincipal User { get; }
    Guid GetAccountId();
}

public class AccountContextService : IAccountContextService
{
    private readonly IHttpContextAccessor _accessor;

    public AccountContextService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public ClaimsPrincipal User => _accessor.HttpContext?.User;

    public Guid GetAccountId() =>
        User is not null ? Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value) : default;
}