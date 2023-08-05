using DocumentsApp.Api.Helper;
using DocumentsApp.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Api.Controllers;

[ApiController]
[Route("Auth")]
public class Auth : ControllerBase
{
    private readonly IAuthHelper _authHelper;

    public Auth(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        return Ok(await _authHelper.SignIn(loginDto));
    }
    
    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] RegisterDto registerDto)
    {
        return Ok(await _authHelper.SignUp(registerDto));
    }
    
    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        throw new NotImplementedException();
    }
    

}