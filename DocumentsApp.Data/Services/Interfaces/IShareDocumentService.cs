using DocumentsApp.Data.Dtos;
using DocumentsApp.Data.Dtos.ShareDocumentDtos;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using Sieve.Models;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IShareDocumentService
{
    Task<PagedResults<GetDocumentDto>> GetAllSharedDocumentsAsync(SieveModel query);
    Task<ShareDocumentDto> ShareDocumentAsync(string documentId, ShareDocumentDto dto);
    Task<ShareDocumentDto> UpdateShareAsync(string documentId, ShareDocumentDto dto);
    Task<bool> UnShareDocumentAsync(string documentId, string userName);
}