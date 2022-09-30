using AutoMapper;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Services;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentByIdAsync(string id);
    Task<IEnumerable<GetDocumentDto>> GetAllDocumentsAsync(string userId);
    Task<string> AddDocumentAsync(string userId, AddDocumentDto dto);
    Task UpdateDocumentAsync(string id, UpdateDocumentDto dto);
    Task DeleteDocumentAsync(string id);
}

public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentRepo _documentRepo;

    public DocumentService(IMapper mapper, IDocumentRepo documentRepo)
    {
        _mapper = mapper;
        _documentRepo = documentRepo;
    }

    public async Task<GetDocumentDto> GetDocumentByIdAsync(string id)
    {
        var document = await SearchDocumentDbAsync(id);
        var resultDocument = _mapper.Map<GetDocumentDto>(document);
        
        return resultDocument;
    }

    public async Task<IEnumerable<GetDocumentDto>> GetAllDocumentsAsync(string userId)
    {
        var creatorId = userId;
        var documents = await _documentRepo.GetAllDocumentsAsync(creatorId);
        
        if (!documents.Any()) throw new NotFoundException("No documents available for this account");

        var resultDocuments = _mapper.Map<IEnumerable<GetDocumentDto>>(documents);
        
        return resultDocuments;
    }

    public async Task<string> AddDocumentAsync(string userId, AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.AccountId = userId;
        await _documentRepo.InsertDocumentAsync(document);
        
        return document.Id;
    }

    public async Task UpdateDocumentAsync(string id, UpdateDocumentDto dto)
    {
        var document = await SearchDocumentDbAsync(id);
        _mapper.Map(dto, document);
        await _documentRepo.UpdateDocumentAsync(document);
    }

    public async Task DeleteDocumentAsync(string id)
    {
        var document = await SearchDocumentDbAsync(id);
        await _documentRepo.DeleteDocumentAsync(document);
    }

    private async Task<Document> SearchDocumentDbAsync(string id)
    {
        var foundDocument = await _documentRepo.GetDocumentByIdAsync(id);

        if (foundDocument is null) throw new NotFoundException("Document does not exist");

        return foundDocument;
    }
}