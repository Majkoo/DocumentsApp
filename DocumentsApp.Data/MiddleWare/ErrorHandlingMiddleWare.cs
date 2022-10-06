using DocumentsApp.Data.Exceptions;
using DocumentsApp.Shared.Notifications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Radzen;

namespace DocumentsApp.Data.MiddleWare;

public class ErrorHandlingMiddleWare : IMiddleware
{
    private readonly NotificationService _notificationService;
    private readonly NavigationManager _navigationManager;

    public ErrorHandlingMiddleWare(
        NotificationService notificationService,
        NavigationManager navigationManager
        )
    {
        _notificationService = notificationService;
        _navigationManager = navigationManager;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception)
        {
            _notificationService.Notify(new ErrorNotification("Error", "internal server error"));
            context.Response.StatusCode = 500;
            throw;
        }
    }
}