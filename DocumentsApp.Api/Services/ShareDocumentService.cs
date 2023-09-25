using AutoMapper;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.AccessLevel;
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
    private readonly IAccountRepo _accountRepo;

    public ShareDocumentService(
        IMapper mapper,
        IDocumentRepo documentRepo,
        ISieveProcessor sieveProcessor,
        IAccessLevelRepo accessLevelRepo,
        IAccountRepo accountRepo
    )
    {
        _mapper = mapper;
        _documentRepo = documentRepo;
        _accessLevelRepo = accessLevelRepo;
        _sieveProcessor = sieveProcessor;
        _accountRepo = accountRepo;
    }

    public async Task<PagedResults<GetAccessLevelDto>> GetAllDocumentSharesAsync(string userId, string documentId,
        SieveModel query)
    {
        var document = await FindDocumentAsync(documentId);
        if (document.AccountId != userId)
            throw new UnauthorizedException("Only document owner can view its shares");
        var shares = _accessLevelRepo.GetAllDocumentAccessLevelsAsync(documentId);
        var result = await _sieveProcessor
            .Apply(query, shares)
            .Select(a => _mapper.Map<GetAccessLevelDto>(a))
            .ToListAsync();

        if (result.Count == 0)
            throw new NotFoundException("Document has no shares");

        return new PagedResults<GetAccessLevelDto>(
            result,
            shares.Count(),
            query.PageSize.GetValueOrDefault(),
            query.Page.GetValueOrDefault());
    }

    public async Task<PagedResults<GetDocumentDto>> GetAllUserSharedDocumentsAsync(string userId, SieveModel query)
    {
        var documents = _documentRepo.GetAllSharedDocumentsAsQueryable(userId);

        var resultDocuments = await _sieveProcessor
            .Apply(query, documents)
            .Select(d => _mapper.Map<GetDocumentDto>(d))
            .ToListAsync();

        if (resultDocuments.Count == 0)
            throw new NotFoundException("No documents shared for this user");

        return new PagedResults<GetDocumentDto>(
            resultDocuments,
            documents.Count(),
            query.PageSize.GetValueOrDefault(),
            query.Page.GetValueOrDefault());
    }

    public async Task<ShareDocumentDto> ShareDocumentAsync(string ownerId, string documentId, ShareDocumentDto dto)
    {
        await CheckIfUserAuthorizedAsync(ownerId, documentId, "User is not authorized to share this document");
        var document = await FindDocumentAsync(documentId);
        var shareToUser = await FindShareToUserAsync(dto.ShareToNameOrEmail);

        if (document.AccountId == shareToUser.Id)
            throw new BadRequestException("ShareDocument", "Cannot share document to owner");

        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(shareToUser.Id, documentId);
        if (accessLevel is not null)
            throw new BadRequestException("ShareDocument", "Document is already shared to this user");

        var newAccessLevel = new DocumentAccessLevel()
        {
            AccountId = shareToUser.Id,
            DocumentId = documentId,
            AccessLevelEnum = dto.AccessLevelEnum
        };

        await _accessLevelRepo.InsertDocumentAccessLevelAsync(newAccessLevel);

        return dto;
    }

    public async Task<ShareDocumentDto> UpdateShareAsync(string ownerId, string documentId, ShareDocumentDto dto)
    {
        await CheckIfUserAuthorizedAsync(ownerId, documentId, "User is not authorized to edit sharing of this document");
        var shareToUser = await FindShareToUserAsync(dto.ShareToNameOrEmail);

        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(shareToUser.Id, documentId);
        if (accessLevel is null || accessLevel.AccessLevelEnum == dto.AccessLevelEnum)
            throw new BadRequestException("UpdateShare", "Document is not shared to this user");

        accessLevel.AccessLevelEnum = dto.AccessLevelEnum;

        await _accessLevelRepo.UpdateDocumentAccessLevelAsync(accessLevel);

        return dto;
    }

    public async Task<bool> UnShareDocumentAsync(string ownerId, string documentId, string userName)
    {
        await CheckIfUserAuthorizedAsync(ownerId, documentId, "User is not authorized to unshare this document");
        var shareToUser = await FindShareToUserAsync(userName);

        var accessLevel = await _accessLevelRepo.GetDocumentAccessLevelAsync(shareToUser.Id, documentId);
        if (accessLevel is null)
            throw new NotFoundException("Document is not shared to this user");

        return await _accessLevelRepo.RemoveDocumentAccessLevelAsync(accessLevel);
    }

    #region Private methods

    private async Task<Document> FindDocumentAsync(string documentId)
    {
        var foundDocument = await _documentRepo.GetDocumentByIdAsync(documentId);
        if (foundDocument is null)
            throw new NotFoundException("Document does not exist");

        return foundDocument;
    }

    private async Task CheckIfUserAuthorizedAsync(string userId, string documentId, string message)
    {
        var document = await FindDocumentAsync(documentId);
        if (document.AccountId != userId)
            throw new UnauthorizedException(message);
    }

    private async Task<Account> FindShareToUserAsync(string userName)
    {
        var shareToUser = await _accountRepo.GetAccountByUsernameAsync(userName) ??
                          await _accountRepo.GetAccountByEmailAsync(userName);

        if (shareToUser is null)
            throw new NotFoundException("No user with such username");

        return shareToUser;
    }

    #endregion
}