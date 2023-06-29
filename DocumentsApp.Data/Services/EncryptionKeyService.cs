using System.Security.Cryptography;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Enums;
using Microsoft.Extensions.Options;

namespace DocumentsApp.Data.Services;

public class EncryptionKeyService : IEncryptionKeyService
{
    private readonly IEncryptionKeyRepo _keyRepo;
    private readonly EncryptionKeySettings _encryptionKeySettings;

    public EncryptionKeyService(IEncryptionKeyRepo keyRepo, IOptions<EncryptionKeySettings> encryptedKeySettings)
    {
        _keyRepo = keyRepo;
        _encryptionKeySettings = encryptedKeySettings.Value;
    }

    public async Task<EncryptionKey> GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum keyType)
    {
        var key =
            (await _keyRepo.GetUserEncryptedKeysAsync()).SingleOrDefault(e => e.EncryptionKeyType == keyType);

        // TODO add event scheduler
        if (key is null)
            return await CreateEncryptionKeyByType(keyType, false);

        if(key.DateExpired < DateTime.Now)
            return await CreateEncryptionKeyByType(keyType, true);
        return key;
    }

    private async Task<EncryptionKey> CreateEncryptionKeyByType(EncryptionKeyTypeEnum keyType, bool isRecreated)
    {
        if (isRecreated)
        {
            var oldKey =
                (await _keyRepo.GetUserEncryptedKeysAsync()).SingleOrDefault(e => e.EncryptionKeyType == keyType);

            await _keyRepo.DeleteEncryptedKeyAsync(oldKey);
        }

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

        return await _keyRepo.InsertEncryptedKeyAsync(newKey);
    }
}