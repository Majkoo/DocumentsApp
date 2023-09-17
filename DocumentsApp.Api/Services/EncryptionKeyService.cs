using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Enums;
using DocumentsApp.Shared.Exceptions;

namespace DocumentsApp.Api.Services;

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