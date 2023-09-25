using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.Document;
using Sieve.Models;

namespace DocumentsApp.Api.Services.Interfaces;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentByIdAsync(string userId, string documentId);
    Task<PagedResults<GetDocumentDto>> GetAllUserDocumentsAsync(string userId, SieveModel query);
    Task<GetDocumentDto> AddDocumentAsync(string userId, AddDocumentDto dto);
    Task<GetDocumentDto> UpdateDocumentAsync(string userId, string documentId, UpdateDocumentDto dto);
    Task<bool> DeleteDocumentAsync(string userId, string documentId);
}