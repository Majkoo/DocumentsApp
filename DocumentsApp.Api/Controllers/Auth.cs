using DocumentsApp.Api.Helper;
using DocumentsApp.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class Auth : ControllerBase
{
    private readonly IAuthHelper _authHelper;

    public Auth(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    [HttpPost(Name = "SignIn")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        return Ok(await _authHelper.SignIn(loginDto));
    }
    
    [HttpPost(Name = "SignUp")]
    public async Task<IActionResult> SignUp([FromBody] RegisterDto registerDto)
    {
        return Ok(await _authHelper.SignUp(registerDto));
    }
    
    [HttpPost(Name = "RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        throw new NotImplementedException();
    }
    

}