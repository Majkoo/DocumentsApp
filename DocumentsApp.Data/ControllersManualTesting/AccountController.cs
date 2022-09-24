using DocumentsApp.Data.Models.AccountModels;
using DocumentsApp.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Data.ControllersManualTesting;

[Route("testing/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto dto)
    {
        await _accountService.RegisterUser(dto);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] LoginUserDto dto)
    {
        var token = await _accountService.GenerateJwt(dto);
        return Ok(token);
    }
}