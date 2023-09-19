using DocumentsApp.Api.Models;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Account;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IAccountService
{
    Task<GetAccountDto> GetCurrentUserInfo();
    Task<bool> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<bool> UpdatePasswordAsync(UpdatePasswordDto dto);

    Task<bool> SubmitEmailConfirmationAsync(string email);
    Task<bool> SubmitPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<bool> ConfirmEmailAsync(AccountSecurityData accountSecurityData);

}

