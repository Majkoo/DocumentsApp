using DocumentsApp.Shared.Dtos.AccountDtos;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IAccountService
{
    Task<IdentityResult> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDto dto);

    Task<IdentityResult> ConfirmEmailAsync();
    Task<IdentityResult> ResetPasswordAsync(UpdatePasswordDto dto);
}

