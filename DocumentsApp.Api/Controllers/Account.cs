using DocumentsApp.Api.Models;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Shared.Dtos.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class Account : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly UserManager<Data.Entities.Account> _userManager;
    private readonly IAuthenticationContextProvider _contextProvider;

    public Account(IAccountService accountService, UserManager<Data.Entities.Account> userManager,
        IAuthenticationContextProvider contextProvider)
    {
        _accountService = accountService;
        _userManager = userManager;
        _contextProvider = contextProvider;
    }

    [HttpGet]
    [Route("Me")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountDto))]
    public async Task<IActionResult> GetLoggedInUser()
    {
        var userId = _contextProvider.GetUserId();
        return Ok(await _accountService.GetUserInfo(userId));
    }

    [HttpPut]
    [Route("UpdateUserName")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IdentityResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameDto dto)
    {
        var userId = _contextProvider.GetUserId();
        return Ok(await _accountService.UpdateUserNameAsync(userId, dto));
    }

    [HttpPut]
    [Route("UpdatePassword")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IdentityResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> UpdateUserName([FromBody] UpdatePasswordDto dto)
    {
        var userId = _contextProvider.GetUserId();
        return Ok(await _accountService.UpdatePasswordAsync(userId, dto));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("SubmitEmailConfirmation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> SubmitEmailConfirmation([FromBody] string email)
    {
        return Ok(await _accountService.SubmitEmailConfirmationAsync(email));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("ConfirmEmail")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> ConfirmEmail([FromQuery] AccountSecurityData data)
    {
        return Ok(await _accountService.ConfirmEmailAsync(data));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("SubmitPasswordReset")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> SubmitPasswordReset([FromBody] string email)
    {
        return Ok(await _accountService.SubmitPasswordResetAsync(email));
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("ResetPassword")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        return Ok(await _accountService.ResetPasswordAsync(dto));
    }
}