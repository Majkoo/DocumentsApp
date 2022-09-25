using AutoMapper;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Models.EntityModels.DocumentModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<Guid> CreateDocument(CreateDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.CreatorId = _userContextService.GetUserId().GetValueOrDefault();

        _dbContext.Add(document);
        await _dbContext.SaveChangesAsync();

        return document.Id;
    }

    public async Task UpdateDocumentById(UpdateDocumentDto dto)
    {
        
    }

    public async Task DeleteDocumentById(Guid id)
    {
        var document = await _dbContext
            .Documents
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document is null) throw new NotFoundException("Document does not exist");

        _dbContext.Remove(document);
        await _dbContext.SaveChangesAsync();
    }
}