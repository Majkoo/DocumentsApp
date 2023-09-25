using DocumentsApp.Api.Models;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Account;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IAccountService
{
    Task<GetAccountDto> GetUserInfo(string id);
    Task<bool> UpdateUserNameAsync(string id, UpdateUserNameDto dto);
    Task<bool> UpdatePasswordAsync(string id, UpdatePasswordDto dto);

    Task<bool> SubmitEmailConfirmationAsync(string email);
    Task<bool> SubmitPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<bool> ConfirmEmailAsync(AccountSecurityData accountSecurityData);

}

