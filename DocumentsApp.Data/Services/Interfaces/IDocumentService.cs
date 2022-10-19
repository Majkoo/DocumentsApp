using DocumentsApp.Data.Dtos;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using Sieve.Models;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentByIdAsync(string id);
    Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(SieveModel query);
    Task<PagedResults<GetDocumentDto>> GetAllSharedDocumentsAsync(SieveModel query);
    Task<GetDocumentDto> AddDocumentAsync(AddDocumentDto dto);
    Task UpdateDocumentAsync(string id, UpdateDocumentDto dto);
    Task DeleteDocumentAsync(string id);
}