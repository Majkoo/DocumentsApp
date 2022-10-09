using AutoMapper;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Dtos;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Data.Services;



public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentRepo _documentRepo;
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IAuthenticationContextProvider _authenticationStateProvider;

    public DocumentService(IMapper mapper, IDocumentRepo documentRepo, ISieveProcessor sieveProcessor,
        IAuthenticationContextProvider authenticationStateProvider)
    {
        _mapper = mapper;
        _documentRepo = documentRepo;
        _sieveProcessor = sieveProcessor;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<GetDocumentDto> GetDocumentByIdAsync(string id)
    {
        var document = await SearchDocumentDbAsync(id);
        var resultDocument = _mapper.Map<GetDocumentDto>(document);
        
        return resultDocument;
    }

    public async Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(SieveModel query)
    {
        var accountId = await _authenticationStateProvider.GetUserId();
        var documents = _documentRepo.GetAllUserDocumentsAsQueryable(accountId);
        
        if (!documents.Any()) throw new NotFoundException("No documents available for this account");

         var resultDocuments = await _sieveProcessor
            .Apply(query, documents)
            .Select(d => _mapper.Map<GetDocumentDto>(d))
            .ToListAsync();
         
        return new PagedResults<GetDocumentDto>(resultDocuments, documents.Count(),
            query.PageSize.GetValueOrDefault(), query.Page.GetValueOrDefault());
    }

    public async Task<string> AddDocumentAsync(AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.AccountId = await _authenticationStateProvider.GetUserId();
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