using DocumentsApp.Data.Entities;
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

    public IQueryable<DocumentAccessLevel> GetAllDocumentAccessLevelsAsync(string documentId)
    {
        return _dbContext.DocumentAccessLevels
            .Include(a => a.Account)
            .Where(a => a.DocumentId == documentId)
            .AsQueryable();
    }

    public async Task<DocumentAccessLevel> GetDocumentAccessLevelAsync(string userId, string documentId)
    {
        var documentAccessLevel = await _dbContext.DocumentAccessLevels
            .SingleOrDefaultAsync(a => a.AccountId == userId && a.DocumentId == documentId);

        return documentAccessLevel;
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
        _dbContext.DocumentAccessLevels.Update(documentAccessLevel);
        await _dbContext.SaveChangesAsync();

        return await _dbContext.DocumentAccessLevels
            .SingleOrDefaultAsync(a => a.Id == documentAccessLevel.Id);
    }

    public async Task<bool> RemoveDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel)
    {
        _dbContext.DocumentAccessLevels.Remove(documentAccessLevel);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveAllDocumentAccessLevelsAsync(string documentId)
    {
        var toRemove = _dbContext.DocumentAccessLevels.Where(a => a.DocumentId == documentId);
        _dbContext.RemoveRange(toRemove);
        
        return await _dbContext.SaveChangesAsync() > 0;
    }
}