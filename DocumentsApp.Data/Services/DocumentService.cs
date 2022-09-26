using AutoMapper;
using DocumentsApp.Data.Dtos.EntityModels.DocumentModels;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Services;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentAsync(Guid id);
    Task<IEnumerable<GetDocumentDto>> GetAllDocumentsAsync();
    Task<Guid> AddDocumentAsync(AddDocumentDto dto);
    Task UpdateDocumentAsync(Guid id, UpdateDocumentDto dto);
    Task DeleteDocumentAsync(Guid id);
}

public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IDocumentRepo _documentRepo;

    public DocumentService(IMapper mapper, IUserContextService userContextService, IDocumentRepo documentRepo)
    {
        _mapper = mapper;
        _userContextService = userContextService;
        _documentRepo = documentRepo;
    }

    public async Task<GetDocumentDto> GetDocumentAsync(Guid id)
    {
        var document = await SearchDocumentDbAsync(id);
        var resultDocument = _mapper.Map<GetDocumentDto>(document);
        
        return resultDocument;
    }

    public async Task<IEnumerable<GetDocumentDto>> GetAllDocumentsAsync()
    {
        var creatorId = _userContextService.GetUserId().GetValueOrDefault();
        var documents = await _documentRepo.GetAllDocumentsAsync(creatorId);
        
        if (!documents.Any()) throw new NotFoundException("No documents available for this user");

        var resultDocuments = _mapper.Map<IEnumerable<GetDocumentDto>>(documents);
        
        return resultDocuments;
    }

    public async Task<Guid> AddDocumentAsync(AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.CreatorId = _userContextService.GetUserId().GetValueOrDefault();
        await _documentRepo.InsertDocumentAsync(document);
        
        return document.Id;
    }

    public async Task UpdateDocumentAsync(Guid id, UpdateDocumentDto dto)
    {
        var document = await SearchDocumentDbAsync(id);
        _mapper.Map(dto, document);
        await _documentRepo.UpdateDocumentAsync(document);
    }

    public async Task DeleteDocumentAsync(Guid id)
    {
        var document = await SearchDocumentDbAsync(id);
        await _documentRepo.DeleteDocumentAsync(document);
    }

    private async Task<Document> SearchDocumentDbAsync(Guid id)
    {
        var foundDocument = await _documentRepo.GetDocumentByIdAsync(id);

        if (foundDocument is null) throw new NotFoundException("Document does not exist");

        return foundDocument;
    }
}