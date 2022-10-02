using AutoMapper;
using DocumentsApp.Data.Dtos;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Data.Services;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentByIdAsync(Guid id);
    Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(Guid userId, SieveModel query);
    Task<Guid> AddDocumentAsync(Guid userId, AddDocumentDto dto);
    Task UpdateDocumentAsync(Guid id, UpdateDocumentDto dto);
    Task DeleteDocumentAsync(Guid id);
}

public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentRepo _documentRepo;
    private readonly ISieveProcessor _sieveProcessor;

    public DocumentService(IMapper mapper, IDocumentRepo documentRepo, ISieveProcessor sieveProcessor)
    {
        _mapper = mapper;
        _documentRepo = documentRepo;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<GetDocumentDto> GetDocumentByIdAsync(Guid id)
    {
        var document = await SearchDocumentDbAsync(id);
        var resultDocument = _mapper.Map<GetDocumentDto>(document);
        
        return resultDocument;
    }

    public async Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(Guid userId, SieveModel query)
    {
        var creatorId = userId;
        var documents = _documentRepo.GetAllDocumentsAsQueryable(creatorId);
        
        if (!documents.Any()) throw new NotFoundException("No documents available for this account");

         var resultDocuments = await _sieveProcessor
            .Apply(query, documents)
            .Select(d => _mapper.Map<GetDocumentDto>(d))
            .ToListAsync();
         
        return new PagedResults<GetDocumentDto>(resultDocuments, resultDocuments.Count,
            query.PageSize.GetValueOrDefault(), query.Page.GetValueOrDefault());
    }

    public async Task<Guid> AddDocumentAsync(Guid userId, AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.AccountId = userId;
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