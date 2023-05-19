using DocumentsApp.Shared.Enums;
using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.Repos.Interfaces;

public interface IEncryptionKeyRepo
{
    Task<IEnumerable<EncryptionKey>> GetUserEncryptedKeysAsync();
    Task<EncryptionKey> InsertEncryptedKeyAsync(EncryptionKey encryptionKey);
    Task<bool> DeleteEncryptedKeyAsync(EncryptionKey encryptionKey);
}