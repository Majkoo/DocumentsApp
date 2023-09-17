using System.Net.Mime;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Dtos.ShareDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace DocumentsApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class Document : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IShareDocumentService _shareDocumentService;

    public Document(
        IDocumentService documentService,
        IShareDocumentService shareDocumentService)
    {
        _documentService = documentService;
        _shareDocumentService = shareDocumentService;
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDocumentDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        return Ok(await _documentService.GetDocumentByIdAsync(id));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetDocumentDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var query = new SieveModel()
        {
            Page = page,
            PageSize = pageSize
        };

        return Ok(await _documentService.GetAllDocumentsAsync(query));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDocumentDto))]
    public async Task<IActionResult> Create([FromBody] AddDocumentDto dto)
    {
        var document = await _documentService.AddDocumentAsync(dto);
        return Created($"/api/restaurant/{document.Id}", document);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDocumentDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateDocumentDto dto)
    {
        return Ok(await _documentService.UpdateDocumentAsync(id, dto));
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        return Ok(await _documentService.DeleteDocumentAsync(id));
    }

    [HttpGet]
    [Route("shared")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetDocumentDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllShared([FromQuery] int page, [FromBody] int pageSize)
    {
        var query = new SieveModel()
        {
            Page = page,
            PageSize = pageSize
        };

        return Ok(await _shareDocumentService.GetAllSharedDocumentsAsync(query));
    }

    [HttpPost]
    [Route("{id}/share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetDocumentDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Share([FromRoute] string id, [FromBody] ShareDocumentDto dto)
    {
        return Ok(await _shareDocumentService.ShareDocumentAsync(id, dto));
    }

    [HttpPut]
    [Route("{id}/share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetDocumentDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateShare([FromRoute] string id, [FromBody] ShareDocumentDto dto)
    {
        return Ok(await _shareDocumentService.UpdateShareAsync(id, dto));
    }

    [HttpDelete]
    [Route("{id}/share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnShare([FromRoute] string id, [FromBody] string userName)
    {
        return Ok(await _shareDocumentService.UnShareDocumentAsync(id, userName));
    }

}