using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Data.Controllers;

[ApiController]
[Route("identity")]
public class IdentityController: ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(
        IIdentityService identityService
    )
    {
        _identityService = identityService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Index(
        [FromForm] string email,
        [FromForm] string password
        )
    {
        var loginDto = new LoginAccountDto()
        {
            Email = email,
            Password = password,
        };

        var result = await _identityService.SignIn(loginDto);

        return result ? Redirect("/") : Redirect("/signin");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Index(
        [FromForm] string username,
        [FromForm] string email,
        [FromForm] string password
    )
    {
        var registerDto = new RegisterAccountDto()
        {
            Email = email,
            Password = password,
            UserName = username
        };

        try
        {
            var result = await _identityService.SignUp(registerDto);
            return result ? Redirect("/") : Redirect("/signup");
        }
        catch (Exception e)
        {
            throw;
        }


    }

    [HttpGet("signout")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _identityService.SignOut();

            return result ? Redirect("/signin") : NoContent();
        }
        catch (Exception e)
        {
            throw;
        }


    }

}
