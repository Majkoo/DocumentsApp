using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services;

public class AccountService : IAccountService
{
    private readonly IAuthenticationContextProvider _authenticationContextProvider;
    private readonly UserManager<Account> _userManager;

    public AccountService(
        IAuthenticationContextProvider authenticationContextProvider, 
        UserManager<Account> userManager)
    {
        _authenticationContextProvider = authenticationContextProvider;
        _userManager = userManager;
    }

    public async Task<IdentityResult> UpdateUserNameAsync(UpdateUserNameDto dto)
    {
        var user = await GetUserFromContext();

        var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordCheck)
            throw new NotAuthorizedException("Wrong password for this user");

        user.UserName = dto.NewUserName;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDto dto)
    {
        var user = await GetUserFromContext();

        return await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
    }

    public Task<IdentityResult> ConfirmEmailAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> ResetPasswordAsync(UpdatePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    private async Task<Account> GetUserFromContext()
    {
        var userId = await _authenticationContextProvider.GetUserId();
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new NotFoundException("No user found in context");

        return user;
    }

}