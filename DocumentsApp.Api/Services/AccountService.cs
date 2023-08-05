using System.Security.Cryptography;
using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using DocumentsApp.Shared.Enums;
using DocumentsApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Api.Services;

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
        var user = await GetUserFromContextAsync();

        var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordCheck)
            throw new UnauthorizedException("Invalid login credentials");

        user.UserName = dto.NewUserName;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDto dto)
    {
        var user = await GetUserFromContextAsync();

        return await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
    }

    public async Task<bool> SubmitEmailConfirmationAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return false;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encrypted = await EncryptCredentialsAsync(email, token, EncryptionKeyTypeEnum.EmailConfirmation);
        var message = _mailHelper.GetEmailConfirmationMessage(email, encrypted);
        _mailService.SendMessageAsync(message);

        return true;
    }

    public async Task<bool> SubmitPasswordResetAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encrypted = await EncryptCredentialsAsync(email, token, EncryptionKeyTypeEnum.PasswordReset);
        var message = _mailHelper.GetPasswordResetMessage(email, encrypted);
        _mailService.SendMessageAsync(message);

        return true;
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string encryptedString)
    {
        var decryptedCredentials =
            await DecryptCredentialsAsync(encryptedString, EncryptionKeyTypeEnum.EmailConfirmation);

        if (decryptedCredentials is null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = nameof(IdentityErrorDescriber.DefaultError),
                    Description = "Invalid operation, try confirming your email again"
                });

        var user = await _userManager.FindByEmailAsync(decryptedCredentials.Email);

        if (user is null)
            throw new NotFoundException("No user with such id");

        if (user.EmailConfirmed)
            return IdentityResult.Failed(
                new IdentityError()
                {
                    Code = nameof(IdentityErrorDescriber.DefaultError),
                    Description = "Invalid operation, your email is already confirmed"
                });

        var emailResult = await _userManager.ConfirmEmailAsync(user, decryptedCredentials.Token);

        if (emailResult != IdentityResult.Success)
            return emailResult;

        user.LockoutEnabled = false;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto, string encryptedString)
    {
        var decryptedCredentials = await DecryptCredentialsAsync(encryptedString, EncryptionKeyTypeEnum.PasswordReset);

        if (decryptedCredentials is null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = nameof(IdentityErrorDescriber.DefaultError),
                    Description = "Invalid operation, try resetting your password again"
                });

        var user = await _userManager.FindByEmailAsync(decryptedCredentials.Email);

        if (user is null)
            throw new NotFoundException("No user with such id");

        // check if the new password is same as the old one
        if (await _userManager.CheckPasswordAsync(user, dto.NewPassword))
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = nameof(IdentityErrorDescriber.DefaultError),
                    Description = "New password cannot be the same as the old one"
                });
        }

        return await _userManager.ResetPasswordAsync(user, decryptedCredentials.Token, dto.NewPassword);
    }

    private async Task<Account> GetUserFromContextAsync()
    {
        var userId = _authenticationContextProvider.GetUserId();
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) throw new NotFoundException("No user found in context");

        return user;
    }

    private async Task<string> EncryptCredentialsAsync(string email, string token, EncryptionKeyTypeEnum keyType)
    {
        var encryptedKey = await _keyService.GetEncryptionKeyByTypeAsync(keyType);
        var toEncrypt = $"{email}&{token}";

        return Convert.ToBase64String(_aesCipher.EncryptString(toEncrypt, encryptedKey.Key, encryptedKey.Vector));
    }

    private async Task<EncryptionCredentials> DecryptCredentialsAsync(string encryptedCredentials,
        EncryptionKeyTypeEnum keyType)
    {
        var encryptedKey = await _keyService.GetEncryptionKeyByTypeAsync(keyType);
        string decrypted;

        try
        {
            decrypted = _aesCipher.DecryptString(Convert.FromBase64String(encryptedCredentials), encryptedKey.Key,
                encryptedKey.Vector);
        }
        catch (CryptographicException e)
        {
            return null;
        }

        return new EncryptionCredentials
        {
            Email = decrypted.Split('&')[0],
            Token = decrypted.Split('&')[1]
        };
    }

    private class EncryptionCredentials
    {
        public string Email;
        public string Token;
    }
}