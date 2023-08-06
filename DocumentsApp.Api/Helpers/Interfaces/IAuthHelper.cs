using DocumentsApp.Shared.Dtos.Auth;

namespace DocumentsApp.Api.Helpers.Interfaces;

public interface IAuthHelper
{
    Task<JwtDataDto> SignIn(LoginDto loginDto);
    Task<bool> SignUp(RegisterDto registerDto);
    Task<JwtDataDto> RefreshToken(string refreshToken);
}