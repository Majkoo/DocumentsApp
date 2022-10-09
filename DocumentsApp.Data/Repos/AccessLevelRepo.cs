using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Enums;
using DocumentsApp.Data.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public class AccessLevelRepo : IAccessLevelRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public AccessLevelRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DocumentAccessLevel>> GetAllDocumentAccessLevelsAsync(string userId)
    {
        return await _dbContext.DocumentAccessLevels
            .Where(a => a.AccountId == userId)
            .ToListAsync();
    }

    public async Task<AccessLevelEnum> GetDocumentAccessLevelAsync(string userId, string documentId)
    {
        var documentAccessLevel = await _dbContext.DocumentAccessLevels
            .SingleOrDefaultAsync(a => a.AccountId == userId && a.DocumentId == documentId);

        return documentAccessLevel?.AccessLevelEnum ?? AccessLevelEnum.None;
    }

    public async Task<DocumentAccessLevel> InsertDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel)
    {
        await _dbContext.DocumentAccessLevels.AddAsync(documentAccessLevel);
        await _dbContext.SaveChangesAsync();

        return await _dbContext.DocumentAccessLevels
            .SingleOrDefaultAsync(a => a.Id == documentAccessLevel.Id);
    }

    public async Task<DocumentAccessLevel> UpdateDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel)
    {
        if (documentAccessLevel.AccessLevelEnum == AccessLevelEnum.None)
            _dbContext.DocumentAccessLevels.Remove(documentAccessLevel);
        else
            _dbContext.DocumentAccessLevels.Update(documentAccessLevel);
        
        await _dbContext.SaveChangesAsync();

        return await _dbContext.DocumentAccessLevels
            .SingleOrDefaultAsync(a => a.Id == documentAccessLevel.Id);
    }
    
}