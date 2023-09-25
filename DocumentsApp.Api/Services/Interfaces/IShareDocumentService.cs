using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.AccessLevel;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Dtos.ShareDocument;
using DocumentsApp.Shared.Enums;
using Sieve.Models;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IShareDocumentService
{
    Task<PagedResults<GetAccessLevelDto>> GetAllDocumentSharesAsync(string userId, string documentId, SieveModel query);
    Task<PagedResults<GetDocumentDto>> GetAllUserSharedDocumentsAsync(string userId, SieveModel query);
    Task<ShareDocumentDto> ShareDocumentAsync(string ownerId, string documentId, ShareDocumentDto dto);
    Task<ShareDocumentDto> UpdateShareAsync(string ownerId, string documentId, ShareDocumentDto dto);
    Task<bool> UnShareDocumentAsync(string ownerId, string documentId, string userName);
}