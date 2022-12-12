using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static int GetHttpStatusCode(Exception exception)
    {
        if (exception is not ProblemDetailsException problemDetailsException)
        {
            return StatusCodes.Status500InternalServerError;
        }
        
        return problemDetailsException.Details.Status switch
        {
            400 => StatusCodes.Status400BadRequest,
            401 => StatusCodes.Status401Unauthorized,
            404 => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is ProblemDetailsException)
            throw exception;
    
        context.Response.StatusCode = GetHttpStatusCode(exception);
        var isInternalServerError = context.Response.StatusCode >= (int)HttpStatusCode.InternalServerError;
    
        _logger.Log(
            isInternalServerError ? LogLevel.Error : LogLevel.Warning,
            exception.ToString());

        context.Response.ContentType = MediaTypeNames.Application.Json;
        var details = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Detail = isInternalServerError ? "Internal server error" : exception.Message,
        };
        var serializedResponse = JsonSerializer.Serialize(details);
        await context.Response.WriteAsync(serializedResponse);
    }
}