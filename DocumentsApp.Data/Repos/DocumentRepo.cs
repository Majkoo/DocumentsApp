using DocumentsApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public interface IDocumentRepo
{
    IQueryable<Document> GetAllDocumentsAsQueryable(string accountId);
    Task<IEnumerable<Document>> GetAllDocumentsAsync(string accountId);
    Task<Document> GetDocumentByIdAsync(string id);
    Task<Document> InsertDocumentAsync(Document document);
    Task<Document> UpdateDocumentAsync(Document document);
    Task<bool> DeleteDocumentAsync(Document document);
}

public class DocumentRepo : IDocumentRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public DocumentRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Document> GetAllDocumentsAsQueryable(string accountId)
    {
        return _dbContext
            .Documents
            .Include(d => d.Account)
            .Where(d => d.AccountId == accountId)
            .AsQueryable();
    }

    public async Task<IEnumerable<Document>> GetAllDocumentsAsync(string accountId)
    {
        return await _dbContext
            .Documents
            .Include(d => d.Account)
            .Where(d => d.AccountId == accountId)
            .ToListAsync();
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
        return await _dbContext
            .Documents
            .SingleOrDefaultAsync(d => d.Id == document.Id);
    }

    public async Task<Document> UpdateDocumentAsync(Document document)
    {
        _dbContext.Documents.Update(document);
        await _dbContext.SaveChangesAsync();
        return await _dbContext
            .Documents
            .SingleOrDefaultAsync(d => d.Id == document.Id);
    }

    public async Task<bool> DeleteDocumentAsync(Document document)
    {
        _dbContext.Documents.Remove(document);
        return await _dbContext.SaveChangesAsync() > 0;
    }

}