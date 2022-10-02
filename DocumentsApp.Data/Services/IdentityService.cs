using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly IAccountRepo _accountRepo;

    public IdentityService(
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        IAccountRepo accountRepo
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _accountRepo = accountRepo;
    }

    public async Task<bool> SignIn(LoginAccountDto loginDto)
    {
        var user = await _accountRepo.GetAccountByEmailAsync(loginDto.Email) ??
                   await _accountRepo.GetAccountByUsernameAsync(loginDto.Email);

        if (user == null) return false;

        var canSignIn = await _signInManager.CanSignInAsync(user);

        if (!canSignIn) return false;

        var auth = await _signInManager
            .PasswordSignInAsync(user, loginDto.Password, true, false);

        return auth.Succeeded && !auth.IsNotAllowed && !auth.IsLockedOut;
    }

    public async Task<bool> SignUp(RegisterAccountDto registerAccountDto)
    {
        var newAccount = new Account()
        {
            UserName = registerAccountDto.UserName,
            Email = registerAccountDto.Email
        };

        var result = await _userManager.CreateAsync(newAccount, registerAccountDto.Password);

        // result.errors

        return result.Succeeded;
    }

    public async Task<bool> SignOut()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}