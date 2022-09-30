using DocumentsApp.Data.Dtos.AccountDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocumentsApp.View.Areas.Identity.Pages.Account;

public class SignUp : PageModel
{
    private readonly SignInManager<Data.Entities.Account> _signInManager;
    private readonly UserManager<Data.Entities.Account> _userManager;

    public SignUp(
        SignInManager<Data.Entities.Account> signInManager,
        UserManager<Data.Entities.Account> userManager
        )
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public RegisterAccountDto RegisterAccountDto { get; set; }
    public string ReturnUrl { get; set; }

    public void OnGet()
    {
        ReturnUrl = Url.Content("~/");
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var identity = new Data.Entities.Account()
            {
                UserName = RegisterAccountDto.UserName,
                Email = RegisterAccountDto.Email
            };
            var result = await _userManager.CreateAsync(identity, RegisterAccountDto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(identity, isPersistent: false);
                return LocalRedirect(ReturnUrl);
            }
        }
        return Page();
    }
}