using DocumentsApp.Shared.Dtos.AccountDtos;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IAccountService
{
    Task<IdentityResult> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDto dto);

    Task<bool> SubmitEmailConfirmationAsync(string email);
    Task<bool> SubmitPasswordResetAsync(string email);
    
    Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto, string token, string email);
    Task<IdentityResult> ConfirmEmailAsync(string token, string email);

}

