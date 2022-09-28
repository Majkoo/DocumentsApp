using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Data.ControllersManualTesting;

[Route("testing/document")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IUserContextService _userContextService;

    public DocumentController(IDocumentService  documentService, IUserContextService userContextService)
    {
        _documentService = documentService;
        _userContextService = userContextService;
    }

    [HttpGet("{documentId}")]
    public async Task<ActionResult<GetDocumentDto>> GetById([FromRoute] Guid documentId)
    {
        var document = await _documentService.GetDocumentByIdAsync(documentId);
        return Ok(document);
    } 
    
    [HttpGet]
    public async Task<ActionResult<GetDocumentDto>> GetAll()
    {
        var userId = _userContextService.GetUserId();
        var documents = await _documentService.GetAllDocumentsAsync(userId);
        return Ok(documents);
    } 


    [HttpPost]
    public async Task<ActionResult> Add([FromBody]AddDocumentDto dto)
    {
        var userId = _userContextService.GetUserId();
        var documentId = await _documentService.AddDocumentAsync(userId, dto);
        return Created($"{documentId}", null);
    }

    [HttpPut("{documentId}")]
    public async Task<ActionResult> UpdateById([FromRoute] Guid documentId, [FromBody] UpdateDocumentDto dto)
    {
        await _documentService.UpdateDocumentAsync(documentId, dto);
        return Ok();
    }

    [HttpDelete("{documentId}")]
    public async Task<ActionResult> DeleteById([FromRoute] Guid documentId)
    {
        await _documentService.DeleteDocumentAsync(documentId);
        return Ok();
    }
}