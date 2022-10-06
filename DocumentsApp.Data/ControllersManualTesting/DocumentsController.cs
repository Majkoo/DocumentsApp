using DocumentsApp.Data.Auth;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace DocumentsApp.Data.ControllersManualTesting;

[Route("testing/document")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;

    public DocumentController(IDocumentService  documentService, CustomAuthenticationStateProvider authenticationStateProvider)
    {
        _documentService = documentService;
        _authenticationStateProvider = authenticationStateProvider;
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
        var userId = await _authenticationStateProvider.GetUserId();
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