using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.AccessLevel;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Dtos.ShareDocument;
using DocumentsApp.Shared.Enums;
using Sieve.Models;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IShareDocumentService
{
    Task<PagedResults<GetAccessLevelDto>> GetAllDocumentSharesAsync(string documentId, SieveModel query);
    Task<PagedResults<GetDocumentDto>> GetAllSharedDocumentsAsync(SieveModel query);
    Task<ShareDocumentDto> ShareDocumentAsync(string documentId, ShareDocumentDto dto);
    Task<ShareDocumentDto> UpdateShareAsync(string documentId, ShareDocumentDto dto);
    Task<bool> UnShareDocumentAsync(string documentId, string userName);
}