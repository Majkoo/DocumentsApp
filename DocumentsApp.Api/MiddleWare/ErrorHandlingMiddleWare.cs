using DocumentsApp.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DocumentsApp.Api.MiddleWare;

public class ErrorHandlingMiddleWare : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleWare> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ErrorHandlingMiddleWare(ILogger<ErrorHandlingMiddleWare> logger, ProblemDetailsFactory problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
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
            var result = GetResult(context, StatusCodes.Status400BadRequest, e.Message);
            var actionContext = new ActionContext(context, context.GetRouteData(), new ActionDescriptor());
            await result.ExecuteResultAsync(actionContext);
        }
        catch (UnauthorizedException e)
        {
            LogError(e);
            var result = GetResult(context, StatusCodes.Status401Unauthorized, e.Message);
            var actionContext = new ActionContext(context, context.GetRouteData(), new ActionDescriptor());
            await result.ExecuteResultAsync(actionContext);
        }
        catch (NotFoundException e)
        {
            LogError(e);
            var result = GetResult(context, StatusCodes.Status404NotFound, e.Message);
            var actionContext = new ActionContext(context, context.GetRouteData(), new ActionDescriptor());
            await result.ExecuteResultAsync(actionContext);
        }
        catch (Exception e)
        {
            LogError(e);
            var result = GetResult(context, StatusCodes.Status500InternalServerError, "Something went wrong");
            var actionContext = new ActionContext(context, context.GetRouteData(), new ActionDescriptor());
            await result.ExecuteResultAsync(actionContext);
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

    private ObjectResult GetResult(HttpContext context, int statusCode, string message)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(context, statusCode, detail: message);
        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}