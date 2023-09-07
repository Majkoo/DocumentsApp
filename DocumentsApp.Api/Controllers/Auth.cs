using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Api.Controllers;

[ApiController]
[Route("auth")]
public class Auth : ControllerBase
{
    private readonly IAuthHelper _authHelper;

    public Auth(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        return Ok(await _authHelper.SignIn(loginDto));
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> SignUp([FromBody] RegisterDto registerDto)
    {
        return Ok(await _authHelper.SignUp(registerDto));
    }
    
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        return Ok(await _authHelper.RefreshToken(refreshToken));
    }

}