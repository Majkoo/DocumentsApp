using DocumentsApp.Shared.Dtos.Auth;

namespace DocumentsApp.Api.Helper;

public interface IAuthHelper
{
    Task<JwtDataDto> SignIn(LoginDto loginDto);
    Task<JwtDataDto> SignUp(RegisterDto registerDto);
}