using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public class EncryptionKeyRepo : IEncryptionKeyRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public EncryptionKeyRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<EncryptionKey>> GetUserEncryptedKeysAsync()
    {
        return await _dbContext
            .EncryptionKeys
            .ToListAsync();
    }

    public async Task<EncryptionKey> InsertEncryptedKeyAsync(EncryptionKey encryptionKey)
    {
        await _dbContext.EncryptionKeys.AddAsync(encryptionKey);
        await _dbContext.SaveChangesAsync();

        return await _dbContext
            .EncryptionKeys
            .SingleOrDefaultAsync(e => e.Id == encryptionKey.Id);
    }

    public async Task<bool> DeleteEncryptedKeyAsync(EncryptionKey encryptionKey)
    {
        _dbContext.EncryptionKeys.Remove(encryptionKey);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}