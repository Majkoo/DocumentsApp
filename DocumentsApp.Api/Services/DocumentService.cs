using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Enums;
using DocumentsApp.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Api.Services;

public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentRepo _documentRepo;
    private readonly IAccessLevelRepo _accessLevelRepo;
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IAuthenticationContextProvider _authenticationContextProvider;

    public DocumentService(
        IMapper mapper,
        IDocumentRepo documentRepo,
        ISieveProcessor sieveProcessor,
        IAccessLevelRepo accessLevelRepo,
        IAuthenticationContextProvider authenticationContextProvider
    )
    {
        _mapper = mapper;
        _documentRepo = documentRepo;
        _accessLevelRepo = accessLevelRepo;
        _sieveProcessor = sieveProcessor;
        _authenticationContextProvider = authenticationContextProvider;
    }

    public async Task<GetDocumentDto> GetDocumentByIdAsync(string documentId)
    {
        var document = await SearchDocumentDbAsync(documentId);
        var accountId = _authenticationContextProvider.GetUserId();
        var documentAccessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(accountId, document.Id);

        if (documentAccessLevel is null && document.AccountId != accountId)
            throw new UnauthorizedException("User does not have access to this file");
                
        var resultDocument = _mapper.Map<GetDocumentDto>(document);
        return resultDocument;
    }

    public async Task<PagedResults<GetDocumentDto>> GetAllDocumentsAsync(SieveModel query)
    {
        var accountId = _authenticationContextProvider.GetUserId();
        var documents = _documentRepo.GetAllUserDocumentsAsQueryable(accountId);

        var resultDocuments = await _sieveProcessor
            .Apply(query, documents)
            .Select(d => _mapper.Map<GetDocumentDto>(d))
            .ToListAsync();
         
        if (resultDocuments.Count == 0) 
            throw new NotFoundException("No documents available for this user");
        
        return new PagedResults<GetDocumentDto>(
            resultDocuments,
            documents.Count(),
            query.PageSize.GetValueOrDefault(),
            query.Page.GetValueOrDefault());
    }

    public async Task<GetDocumentDto> AddDocumentAsync(AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.AccountId = _authenticationContextProvider.GetUserId();
        
        var result = await _documentRepo.InsertDocumentAsync(document);
        
        return _mapper.Map<GetDocumentDto>(result);
    }

    public async Task<GetDocumentDto> UpdateDocumentAsync(string documentId, UpdateDocumentDto dto)
    {
        var document = await SearchDocumentDbAsync(documentId);
        var userId = _authenticationContextProvider.GetUserId();
        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(userId, documentId);

        if (document.AccountId != userId && accessLevel?.AccessLevelEnum != AccessLevelEnum.Write)
            throw new UnauthorizedException("User is not authorized to edit this document");

        _mapper.Map(dto, document);

        var result = await _documentRepo.UpdateDocumentAsync(document);
        
        return _mapper.Map<GetDocumentDto>(result);
    }

    public async Task<bool> DeleteDocumentAsync(string documentId)
    {
        var document = await SearchDocumentDbAsync(documentId);
        var userId = _authenticationContextProvider.GetUserId();
        
        if (document.AccountId != userId)
            throw new UnauthorizedException("User is not authorized to delete this document");

        await _accessLevelRepo.RemoveAllDocumentAccessLevelsAsync(documentId);
        var result = await _documentRepo.DeleteDocumentAsync(document);
        
        return result;
    }

    private async Task<Document> SearchDocumentDbAsync(string documentId)
    {
        var foundDocument = await _documentRepo.GetDocumentByIdAsync(documentId);
        if (foundDocument is null) 
            throw new NotFoundException("Document does not exist");

        return foundDocument;
    }
}