using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace DocumentsApp.Data.ControllersManualTesting;

[Route("testing/document")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService  documentService)
    {
        _documentService = documentService;
    }

    [HttpGet("{documentId}")]
    public async Task<ActionResult<GetDocumentDto>> GetById([FromRoute] string documentId)
    {
        var document = await _documentService.GetDocumentByIdAsync(documentId);
        return Ok(document);
    } 
    
    [HttpGet]
    public async Task<ActionResult<GetDocumentDto>> GetAll([FromBody] SieveModel query)
    {
        var documents = await _documentService.GetAllDocumentsAsync(query);
        return Ok(documents);
    } 

    [HttpPost]
    public async Task<ActionResult> Add([FromBody]AddDocumentDto dto)
    {
        var documentId = await _documentService.AddDocumentAsync(dto);
        return Created($"{documentId}", null);
    }

    [HttpPut("{documentId}")]
    public async Task<ActionResult> UpdateById([FromRoute] string documentId, [FromBody] UpdateDocumentDto dto)
    {
        await _documentService.UpdateDocumentAsync(documentId, dto);
        return Ok();
    }

    [HttpDelete("{documentId}")]
    public async Task<ActionResult> DeleteById([FromRoute] string documentId)
    {
        await _documentService.DeleteDocumentAsync(documentId);
        return Ok();
    }
}