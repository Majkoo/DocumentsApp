using AutoMapper;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Models.EntityModels.DocumentModels;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Data.Services;

public class DocumentService
{
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly DocumentsAppDbContext _dbContext;

    public DocumentService(IMapper mapper, IUserContextService userContextService, DocumentsAppDbContext dbContext)
    {
        _mapper = mapper;
        _userContextService = userContextService;
        _dbContext = dbContext;
    }
    public async Task<Guid> AddDocument(AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.CreatorId = _userContextService.GetUserId().GetValueOrDefault();

        await _dbContext.AddAsync(document);
        await _dbContext.SaveChangesAsync();

        return document.Id;
    }
}