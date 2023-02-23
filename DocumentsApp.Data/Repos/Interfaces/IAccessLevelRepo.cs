using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.Repos.Interfaces;

public interface IAccessLevelRepo
{
    Task<IEnumerable<DocumentAccessLevel>> GetAllDocumentAccessLevelsAsync(string userId);
    Task<DocumentAccessLevel> GetDocumentAccessLevelAsync(string userId, string documentId);
    Task<DocumentAccessLevel> InsertDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel);
    Task<DocumentAccessLevel> UpdateDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel);
    Task<bool> RemoveDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel);
    Task<bool> RemoveAllDocumentAccessLevelsAsync(string documentId);
}