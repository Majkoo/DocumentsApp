using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.Document;
using Sieve.Models;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentByIdAsync(string documentId);
    Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(SieveModel query);
    Task<GetDocumentDto> AddDocumentAsync(AddDocumentDto dto);
    Task<GetDocumentDto> UpdateDocumentAsync(string documentId, UpdateDocumentDto dto);
    Task<bool> DeleteDocumentAsync(string documentId);
}