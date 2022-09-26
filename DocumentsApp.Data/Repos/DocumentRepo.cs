using DocumentsApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public interface IDocumentRepo
{
    Task<IEnumerable<Document>> GetAllDocumentsAsync(Guid userId);
    Task<Document> GetDocumentByIdAsync(Guid id);
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

    public async Task<IEnumerable<Document>> GetAllDocumentsAsync(Guid userId)
    {
        return await _dbContext
            .Documents
            .Where(d => d.CreatorId == userId)
            .ToListAsync();
    }

    public async Task<Document> GetDocumentByIdAsync(Guid id)
    {
        return await _dbContext
            .Documents
            .SingleOrDefaultAsync(d=>d.Id == id);
    }

    public async Task<Document> InsertDocumentAsync(Document document)
    {
        await _dbContext.Documents.AddAsync(document);
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