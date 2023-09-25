using System.Security.Claims;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DocumentsApp.Api.Hubs;

[Authorize]
public class SharedDocumentHub : Hub
{
    private readonly IDocumentService _documentService;

    public SharedDocumentHub(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public async Task Subscribe(string documentId)
    {
        await _documentService.GetDocumentByIdAsync(GetUserId(), documentId);
        await Groups.AddToGroupAsync(Context.ConnectionId, documentId);
    }

    public async Task Broadcast(string documentId, string content)
    {
        var updated = new UpdateDocumentDto()
        {
            Content = content
        };

        await _documentService.UpdateDocumentAsync(GetUserId(), documentId, updated);
        await Clients.OthersInGroup(documentId).SendAsync("GetUpdate", content);
    }

    #region private methods

    private string GetUserId()
    {
        return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new UnauthorizedException("Invalid token");
    }

    #endregion
}