using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Dtos.ShareDocument;
using DocumentsApp.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace DocumentsApp.Api.Services;

public class ShareDocumentService : IShareDocumentService
{
    private readonly IMapper _mapper;
    private readonly IDocumentRepo _documentRepo;
    private readonly IAccessLevelRepo _accessLevelRepo;
    private readonly ISieveProcessor _sieveProcessor;
    private readonly IAuthenticationContextProvider _authenticationStateProvider;
    private readonly IAccountRepo _accountRepo;

    public ShareDocumentService(
        IMapper mapper,
        IDocumentRepo documentRepo,
        ISieveProcessor sieveProcessor,
        IAccessLevelRepo accessLevelRepo,
        IAuthenticationContextProvider authenticationStateProvider,
        IAccountRepo accountRepo
    )
    {
        _mapper = mapper;
        _documentRepo = documentRepo;
        _accessLevelRepo = accessLevelRepo;
        _sieveProcessor = sieveProcessor;
        _authenticationStateProvider = authenticationStateProvider;
        _accountRepo = accountRepo;
    }

    public async Task<PagedResults<GetDocumentDto>> GetAllSharedDocumentsAsync(SieveModel query)
    {
        var accountId = _authenticationStateProvider.GetUserId();
        var documents = _documentRepo.GetAllSharedDocumentsAsQueryable(accountId);
        
        if (!documents.Any()) 
            throw new NotFoundException("No documents shared for this user");
        
        var resultDocuments = await _sieveProcessor
            .Apply(query, documents)
            .Select(d => _mapper.Map<GetDocumentDto>(d))
            .ToListAsync();
        
        return new PagedResults<GetDocumentDto>(
            resultDocuments, 
            documents.Count(),
            query.PageSize.GetValueOrDefault(), 
            query.Page.GetValueOrDefault());
    }

    public async Task<ShareDocumentDto> ShareDocumentAsync(string documentId, ShareDocumentDto dto)
    {
        var shareToId = (await _accountRepo.GetAccountByUsernameAsync(dto.SharedToUserName)).Id;
        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(shareToId, documentId);
        await SearchDocumentDbAsync(documentId);

        if (string.IsNullOrEmpty(shareToId))
            throw new NotFoundException("No user with such username");
        
        if (accessLevel is not null)
            throw new BadRequestException("Document is already shared to this user");

        var newAccessLevel = new DocumentAccessLevel()
        {
            AccountId = shareToId,
            DocumentId = documentId,
            AccessLevelEnum = dto.AccessLevelEnum
        };

        await _accessLevelRepo.InsertDocumentAccessLevelAsync(newAccessLevel);

        return dto;
    }

    public async Task<ShareDocumentDto> UpdateShareAsync(string documentId, ShareDocumentDto dto)
    {
        var shareToId = (await _accountRepo.GetAccountByUsernameAsync(dto.SharedToUserName)).Id;
        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(shareToId, documentId);
        await SearchDocumentDbAsync(documentId);
        
        if (string.IsNullOrEmpty(shareToId))
            throw new NotFoundException("No user with such username");
        
        if (accessLevel is null)
            throw new NotFoundException("Document is not shared to this user");

        await _accessLevelRepo.UpdateDocumentAccessLevelAsync(accessLevel);

        return dto;
    }

    public async Task<bool> UnShareDocumentAsync(string documentId, string userName)
    {
        var shareToId = (await _accountRepo.GetAccountByUsernameAsync(userName)).Id;
        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(shareToId, documentId);
        await SearchDocumentDbAsync(documentId);

        if (string.IsNullOrEmpty(shareToId))
            throw new NotFoundException("No user with such username");
        
        if (accessLevel is null)
            throw new NotFoundException("Document is not shared to this user");

        var result = await _accessLevelRepo.RemoveDocumentAccessLevelAsync(accessLevel);

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