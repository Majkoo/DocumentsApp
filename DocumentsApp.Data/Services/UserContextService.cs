using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DocumentsApp.Data.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid GetUserId();
}

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _accessor;

    public UserContextService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public ClaimsPrincipal User => _accessor.HttpContext?.User;

    public Guid GetUserId() =>
        User is not null ? Guid.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value) : default;
}