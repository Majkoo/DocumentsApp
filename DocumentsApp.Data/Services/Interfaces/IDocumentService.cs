using DocumentsApp.Data.Dtos;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using Sieve.Models;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentByIdAsync(string documentId);
    Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(SieveModel query);
    Task<GetDocumentDto> AddDocumentAsync(AddDocumentDto dto);
    Task<GetDocumentDto> UpdateDocumentAsync(string documentId, UpdateDocumentDto dto);
    Task<bool> DeleteDocumentAsync(string documentId);
}