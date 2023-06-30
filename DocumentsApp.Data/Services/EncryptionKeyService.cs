using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Enums;
using Microsoft.Extensions.Options;

namespace DocumentsApp.Data.Services;

public class EncryptionKeyService : IEncryptionKeyService
{
    private readonly IEncryptionKeyRepo _keyRepo;

    public EncryptionKeyService(IEncryptionKeyRepo keyRepo)
    {
        _keyRepo = keyRepo;
    }

    public async Task<EncryptionKey> GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum keyType)
    {
        var key =
            (await _keyRepo.GetUserEncryptedKeysAsync()).SingleOrDefault(e => e.EncryptionKeyType == keyType);

        if (key is null)
            throw new NotFoundException("No encryption key found");

        return key;
    }
}