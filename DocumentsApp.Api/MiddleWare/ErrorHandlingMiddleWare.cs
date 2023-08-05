using DocumentsApp.Shared.Exceptions;

namespace DocumentsApp.Api.MiddleWare;

public class ErrorHandlingMiddleWare : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleWare> _logger;

    public ErrorHandlingMiddleWare(ILogger<ErrorHandlingMiddleWare> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException e)
        {
            LogError(e);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(e.Message);
        }
        catch (UnauthorizedException e)
        {
            LogError(e);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(e.Message);
        }
        catch (NotFoundException e)
        {
            LogError(e);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            LogError(e);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
        
    }


    private void LogError(Exception exception)
    {
        _logger.LogError(
            "An exception occured.\n" +
            "Message: {ExceptionMessage}\n" +
            "Stack trace:{ExceptionStackTrace}", 
            exception.Message,
            exception.StackTrace
            );
    }
}