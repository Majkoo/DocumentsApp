using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using DocumentsApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services;

public class AccountService : IAccountService
{
    private readonly IAuthenticationContextProvider _authenticationContextProvider;
    private readonly UserManager<Account> _userManager;
    private readonly IMailHelper _mailHelper;
    private readonly IMailService _mailService;
    private readonly IAesCipher _aesCipher;
    private readonly IEncryptionKeyService _keyService;

    public AccountService(
        IAuthenticationContextProvider authenticationContextProvider,
        UserManager<Account> userManager,
        IMailHelper mailHelper,
        IMailService mailService,
        IAesCipher aesCipher,
        IEncryptionKeyService keyService)
    {
        _authenticationContextProvider = authenticationContextProvider;
        _userManager = userManager;
        _mailHelper = mailHelper;
        _mailService = mailService;
        _aesCipher = aesCipher;
        _keyService = keyService;
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

    public async Task<bool> SubmitEmailConfirmationAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user is null)
            return false;
        
        var message = await _mailHelper.GetEmailConfirmationMessageAsync(email);
        _mailService.SendMessageAsync(message);

        return true;
    }

    public async Task<bool> SubmitPasswordResetAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user is null)
            return false;
        
        var message = await _mailHelper.GetPasswordResetMessageAsync(email);
        _mailService.SendMessageAsync(message);

        return true;
    }

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto, string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var isSameAsOld = await _userManager.CheckPasswordAsync(user, dto.NewPassword);

        if (isSameAsOld)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = nameof(IdentityErrorDescriber.DefaultError),
                    Description = "New password cannot be the same as the old one" 
                });
        }

        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        return result;
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string encryptedString)
    {
        var encryptedKey = await _keyService.GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum.EmailConfirmation);
        
        if(encryptedKey is null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = nameof(IdentityErrorDescriber.DefaultError),
                    Description = "Invalid operation, try resetting your password again" 
                });
            
        var encrypted = Convert.FromBase64String(encryptedString);
        var decrypted = _aesCipher.DecryptString(encrypted, encryptedKey.Key, encryptedKey.Vector);

        var email = decrypted.Split('&')[0];
        var token = decrypted.Split('&')[1];

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            throw new NotFoundException("No user with such id");

        return await _userManager.ConfirmEmailAsync(user, token);
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