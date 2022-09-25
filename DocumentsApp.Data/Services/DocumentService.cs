using AutoMapper;
using DocumentsApp.Data.Dtos.EntityModels.DocumentModels;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Services;

public interface IDocumentService
{
    Task<GetDocumentDto> GetDocumentAsync(Guid id);
    Task<Guid> AddDocumentAsync(AddDocumentDto dto);
    Task UpdateDocumentAsync(Guid id, UpdateDocumentDto dto);
    Task DeleteDocumentAsync(Guid id);
}

public class DocumentService : IDocumentService
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

    public async Task<GetDocumentDto> GetDocumentAsync(Guid id)
    {
        var document = await SearchDocumentDbAsync(id);

        var resultDocument = _mapper.Map<GetDocumentDto>(document);
        
        return resultDocument;
    }
    
    public async Task<Guid> AddDocumentAsync(AddDocumentDto dto)
    {
        var document = _mapper.Map<Document>(dto);
        document.CreatorId = _userContextService.GetUserId().GetValueOrDefault();

        await _dbContext.AddAsync(document);
        await _dbContext.SaveChangesAsync();

        return document.Id;
    }

    public async Task UpdateDocumentAsync(Guid id, UpdateDocumentDto dto)
    {
        var document = await SearchDocumentDbAsync(id);
        _mapper.Map(dto, document);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteDocumentAsync(Guid id)
    {
        var document = await SearchDocumentDbAsync(id);
        _dbContext.Remove(document);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Document> SearchDocumentDbAsync(Guid id)
    {
        var document = await _dbContext
            .Documents
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document is null) throw new NotFoundException("Document does not exist");

        return document;
    }
}