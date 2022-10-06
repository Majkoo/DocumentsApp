using DocumentsApp.Data.Dtos.AccountDtos;
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
    public async Task<ActionResult> RegisterUser([FromBody] RegisterAccountDto dto)
    {
        await _accountService.RegisterAccountAsync(dto);
        return Ok();
    }

    [HttpPut("updateUserName")]
    public async Task<ActionResult> UpdateUserName([FromBody] UpdateUserNameDto dto)
    {
        await _accountService.UpdateUserNameAsync(dto);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] LoginAccountDto dto)
    {
        var token = await _accountService.GenerateJwtAsync(dto);
        return Ok(token);
    }
}