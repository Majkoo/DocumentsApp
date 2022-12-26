using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public class DocumentRepo : IDocumentRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public DocumentRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Document> GetAllUserDocumentsAsQueryable(string accountId)
    {
        return _dbContext.Documents
            .Include(d => d.Account)
            .Include(d => d.AccessLevels)
            .Where(d => d.AccountId == accountId)
            .OrderByDescending(d => d.DateCreated)
            .AsQueryable();
    }

    public async Task<IEnumerable<Document>> GetAllUserDocumentsAsync(string accountId)
    {
        return await _dbContext.Documents
            .Include(d => d.Account)
            .Include(d => d.AccessLevels)
            .Where(d => d.AccountId == accountId)
            .OrderByDescending(d => d.DateCreated)
            .ToListAsync();
    }
    
    public IQueryable<Document> GetAllSharedDocumentsAsQueryable(string userId)
    {
        var accessLevels = _dbContext.DocumentAccessLevels
            .Include(a => a.Document)
            .Where(a => a.AccountId == userId);

        return accessLevels
            .Select(d => d.Document)
            .OrderByDescending(d => d.DateCreated)
            .AsQueryable();
    }

    public async Task<Document> GetDocumentByIdAsync(string id)
    {
        return await _dbContext
            .Documents
            .SingleOrDefaultAsync(d=>d.Id == id);
    }

    public async Task<Document> InsertDocumentAsync(Document document)
    {
        await _dbContext.Documents.AddAsync(document);
        await _dbContext.SaveChangesAsync();
        
        return await _dbContext.Documents
            .SingleOrDefaultAsync(d => d.Id == document.Id);
    }

    public async Task<Document> UpdateDocumentAsync(Document document)
    {
        _dbContext.Documents.Update(document);
        await _dbContext.SaveChangesAsync();
        
        return await _dbContext.Documents
            .SingleOrDefaultAsync(d => d.Id == document.Id);
    }

    public async Task<bool> DeleteDocumentAsync(Document document)
    {
        _dbContext.Documents.Remove(document);
        return await _dbContext.SaveChangesAsync() > 0;
    }

}