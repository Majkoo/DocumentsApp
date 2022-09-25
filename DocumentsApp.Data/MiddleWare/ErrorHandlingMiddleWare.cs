using DocumentsApp.Data.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DocumentsApp.Data.MiddleWare;

public class ErrorHandlingMiddleWare : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(badRequestException.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal server error");
            throw;
        }
    }
}