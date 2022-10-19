using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.Repos.Interfaces;

public interface IDocumentRepo
{
    IQueryable<Document> GetAllUserDocumentsAsQueryable(string accountId);
    IQueryable<Document> GetAllSharedDocumentsAsQueryable(string userId);
    Task<IEnumerable<Document>> GetAllUserDocumentsAsync(string accountId);
    Task<Document> GetDocumentByIdAsync(string id);
    Task<Document> InsertDocumentAsync(Document document);
    Task<Document> UpdateDocumentAsync(Document document);
    Task<bool> DeleteDocumentAsync(Document document);
    

}