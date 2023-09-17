using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Enums;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IEncryptionKeyService
{
    Task<EncryptionKey> GetEncryptionKeyByTypeAsync(EncryptionKeyTypeEnum keyType);
}