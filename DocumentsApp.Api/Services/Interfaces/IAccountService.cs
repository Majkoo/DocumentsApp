using DocumentsApp.Shared.Dtos.Account;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IAccountService
{
    Task<IdentityResult> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDto dto);

    Task<bool> SubmitEmailConfirmationAsync(string email);
    Task<bool> SubmitPasswordResetAsync(string email);
    Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto, string encryptedString);
    Task<IdentityResult> ConfirmEmailAsync(string encryptedString);

}

