using System.Net.Mime;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Shared.Dtos;
using DocumentsApp.Shared.Dtos.AccessLevel;
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
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
public class Document : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IShareDocumentService _shareDocumentService;
    private readonly IAuthenticationContextProvider _authenticationContextProvider;

    public Document(
        IDocumentService documentService,
        IShareDocumentService shareDocumentService
        ,IAuthenticationContextProvider authenticationContextProvider)
    {
        _documentService = documentService;
        _shareDocumentService = shareDocumentService;
        _authenticationContextProvider = authenticationContextProvider;
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDocumentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _documentService.GetDocumentByIdAsync(userId, id));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetDocumentDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] SieveModel query)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _documentService.GetAllUserDocumentsAsync(userId, query));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetDocumentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Create([FromBody] AddDocumentDto dto)
    {
        var userId = _authenticationContextProvider.GetUserId();
        var document = await _documentService.AddDocumentAsync(userId, dto);
        return Created($"/api/restaurant/{document.Id}", document);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDocumentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateDocumentDto dto)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _documentService.UpdateDocumentAsync(userId, id, dto));
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _documentService.DeleteDocumentAsync(userId, id));
    }

    [HttpGet]
    [Route("Shared")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetDocumentDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllShared([FromQuery] SieveModel query)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _shareDocumentService.GetAllUserSharedDocumentsAsync(userId, query));
    }
    
    [HttpGet]
    [Route("{id}/Shares")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResults<GetAccessLevelDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDocumentShares([FromRoute] string id, [FromQuery] SieveModel query)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _shareDocumentService.GetAllDocumentSharesAsync(userId, id, query));
    }

    [HttpPost]
    [Route("{id}/Share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShareDocumentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Share([FromRoute] string id, [FromBody] ShareDocumentDto dto)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _shareDocumentService.ShareDocumentAsync(userId, id, dto));
    }

    [HttpPut]
    [Route("{id}/Share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShareDocumentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateShare([FromRoute] string id, [FromBody] ShareDocumentDto dto)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _shareDocumentService.UpdateShareAsync(userId, id, dto));
    }

    [HttpDelete]
    [Route("{id}/Share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Boolean))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnShare([FromRoute] string id, [FromBody] string userName)
    {
        var userId = _authenticationContextProvider.GetUserId();
        return Ok(await _shareDocumentService.UnShareDocumentAsync(userId, id, userName));
    }

}