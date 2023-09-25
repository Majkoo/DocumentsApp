using AutoMapper;
using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Api.Models;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Account;
using DocumentsApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Api.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<Account> _userManager;
    private readonly IMailHelper _mailHelper;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;

    public AccountService(
        UserManager<Account> userManager,
        IMailHelper mailHelper,
        IMailService mailService,
        IMapper mapper)
    {
        _userManager = userManager;
        _mailHelper = mailHelper;
        _mailService = mailService;
        _mapper = mapper;
    }

    public async Task<GetAccountDto> GetUserInfo(string id)
    {
        var user = await GetUserByIdAsync(id);
        return _mapper.Map<GetAccountDto>(user);
    }

    public async Task<bool> UpdateUserNameAsync(string id, UpdateUserNameDto dto)
    {
        var user = await GetUserByIdAsync(id);

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedException("Invalid login credentials");

        if (user.UserName == dto.NewUserName)
            throw new BadRequestException("UpdateUserName", "Username must be different from the current one");

        user.UserName = dto.NewUserName;

        return CheckResult(await _userManager.UpdateAsync(user), "UpdateUserName", "Failed to update username");
    }

    public async Task<bool> UpdatePasswordAsync(string id, UpdatePasswordDto dto)
    {
        var user = await GetUserByIdAsync(id);

        if (await _userManager.CheckPasswordAsync(user, dto.NewPassword))
            throw new BadRequestException("UpdatePassword", "New password must be different from the old one");
        
        return CheckResult(await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword),
            "UpdatePassword", "Failed to update password");
    }

    public async Task<bool> SubmitEmailConfirmationAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new NotFoundException("No user with such email");
        
        if (user.EmailConfirmed)
            throw new BadRequestException("ConfirmEmail", "User email is already confirmed");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var accountSecurityData = new AccountSecurityData()
        {
            AccountId = user.Id,
            Token = token
        };
        
        var message = _mailHelper.GetEmailConfirmationMessage(email, accountSecurityData);
        _mailService.SendMessageAsync(message);

        return true;
    }

    public async Task<bool> SubmitPasswordResetAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var accountSecurityData = new AccountSecurityData()
        {
            AccountId = user.Id,
            Token = token
        };
        
        var message = _mailHelper.GetPasswordResetMessage(email, accountSecurityData);
        _mailService.SendMessageAsync(message);

        return true;
    }

    public async Task<bool> ConfirmEmailAsync(AccountSecurityData accountSecurityData)
    {
        var user = await _userManager.FindByIdAsync(accountSecurityData.AccountId);

        if (user is null)
            throw new NotFoundException("No user with such id");

        if (user.EmailConfirmed)
            throw new BadRequestException("ConfirmEmail", "User email is already confirmed");

        var emailResult = await _userManager.ConfirmEmailAsync(user, accountSecurityData.Token);

        CheckResult(emailResult, "ConfirmEmail", "Failed to confirm email");
        user.LockoutEnabled = false;
        
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.AccountId);

        if (user is null)
            throw new NotFoundException("No user with such id");

        if (await _userManager.CheckPasswordAsync(user, dto.NewPassword))
            throw new BadRequestException("ResetPassword", "Password must be different from the old one");

        return CheckResult(await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword),
            "ResetPassword", "Failed to reset password");
    }
    
    #region Private methods

    private bool CheckResult(IdentityResult result, string titleOnFail, string messageOnFail)
    {
        if (result.Errors.Any())
            throw new BadRequestException(titleOnFail, result.Errors.First().Description);

        if (!result.Succeeded)
            throw new BadRequestException(titleOnFail, messageOnFail);

        return result.Succeeded;
    }

    private async Task<Account> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null) throw new NotFoundException("No user found in context");

        return user;
    }
    
    #endregion
}