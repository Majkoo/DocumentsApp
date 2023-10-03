using System.Net.Mime;
using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class Auth : ControllerBase
{
    private readonly IAuthHelper _authHelper;

    public Auth(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    [HttpPost]
    [Route("SignIn")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtDataDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        return Ok(await _authHelper.SignIn(loginDto));
    }
    
    [HttpPost]
    [Route("SignUp")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> SignUp([FromBody] RegisterDto registerDto)
    {
        return Ok(await _authHelper.SignUp(registerDto));
    }
    
    [HttpPost]
    [Route("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtDataDto))]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        return Ok(await _authHelper.RefreshToken(refreshToken));
    }

}