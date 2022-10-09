using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Enums;

namespace DocumentsApp.Data.Repos.Interfaces;

public interface IAccessLevelRepo
{
    Task<IEnumerable<DocumentAccessLevel>> GetAllDocumentAccessLevelsAsync(string userId);
    Task<AccessLevelEnum> GetDocumentAccessLevelAsync(string userId, string documentId);
    Task<DocumentAccessLevel> InsertDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel);
    Task<DocumentAccessLevel> UpdateDocumentAccessLevelAsync(DocumentAccessLevel documentAccessLevel);
}