using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Shared.Dtos.AccessLevel;
using DocumentsApp.Shared.Dtos.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DocumentsApp.Api.Hubs;

[Authorize]
public class SharedDocumentHub : Hub
{
    private readonly IDocumentService _documentService;
    private readonly IAuthenticationContextProvider _contextProvider;

    public SharedDocumentHub(IDocumentService documentService, IAuthenticationContextProvider contextProvider)
    {
        _documentService = documentService;
        
        _contextProvider = contextProvider;
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var documentId = Context.GetHttpContext()?.Request.Query["id"];
        //TODO save to db at disconnect
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, documentId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task Subscribe(string documentId)
    {
        // check document and access
        await _documentService.GetDocumentByIdAsync(documentId);
        await Groups.AddToGroupAsync(Context.ConnectionId, documentId);
    }

    public async Task UpdateDocument(string documentId, string content)
    {
        await Clients.OthersInGroup(documentId).SendAsync("GetUpdate", content);

        var updated = new UpdateDocumentDto()
        {
            Content = content
        };
        
        await _documentService.UpdateDocumentAsync(documentId, updated);
    }
}