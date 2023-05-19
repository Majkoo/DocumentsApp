using DocumentsApp.Shared.Enums;
using DocumentsApp.Data.Entities;
namespace DocumentsApp.Data.Services.Interfaces;

public interface IEncryptionKeyService
{
    Task<EncryptionKey> GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum keyType);
}