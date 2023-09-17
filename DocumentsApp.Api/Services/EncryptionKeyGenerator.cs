using System.Security.Cryptography;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Enums;
using Microsoft.Extensions.Options;

namespace DocumentsApp.Api.Services;

public class EncryptionKeyGenerator : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EncryptionKeySettings _encryptionKeySettings;

    public EncryptionKeyGenerator(IOptions<EncryptionKeySettings> encryptionKeySettings,
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _encryptionKeySettings = encryptionKeySettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var keyService = scope.ServiceProvider.GetRequiredService<IEncryptionKeyService>();
        
        await CreateEncryptionKeyByType(EncryptionKeyTypeEnum.PasswordReset);
        await CreateEncryptionKeyByType(EncryptionKeyTypeEnum.EmailConfirmation);

        while (!stoppingToken.IsCancellationRequested)
        {
            var passwordResetKey = await keyService.GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum.PasswordReset);
            var emailConfirmationKey = await keyService.GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum.EmailConfirmation);
        
            if (passwordResetKey.DateExpired < DateTime.Now)
                await CreateEncryptionKeyByType(EncryptionKeyTypeEnum.PasswordReset);

            if (emailConfirmationKey.DateExpired < DateTime.Now)
                await CreateEncryptionKeyByType(EncryptionKeyTypeEnum.EmailConfirmation);

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task<EncryptionKey> CreateEncryptionKeyByType(EncryptionKeyTypeEnum keyType)
    {
        using var scope = _scopeFactory.CreateScope();
        var keyRepo = scope.ServiceProvider.GetRequiredService<IEncryptionKeyRepo>();

        var oldKeys =
            (await keyRepo.GetUserEncryptedKeysAsync()).Where(e => e.EncryptionKeyType == keyType);

        foreach (var key in oldKeys)
            await keyRepo.DeleteEncryptedKeyAsync(key);

        var aes = Aes.Create();

        var newKey = new EncryptionKey
        {
            EncryptionKeyType = keyType,
            Key = aes.Key,
            Vector = aes.IV,
            DateExpired = keyType == EncryptionKeyTypeEnum.EmailConfirmation
                ? DateTime.Now + _encryptionKeySettings.EmailConfirmationDurationDays
                : DateTime.Now + _encryptionKeySettings.PasswordResetDurationDays
        };

        return await keyRepo.InsertEncryptedKeyAsync(newKey);
    }
}