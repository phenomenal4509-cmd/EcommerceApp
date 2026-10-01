using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Let DbUpdateConcurrencyException fall through to the endpoint's own try/catch
        if (exception is DbUpdateConcurrencyException)
            return false;

        var traceId  = httpContext.TraceIdentifier;
        var method   = httpContext.Request.Method;
        var path     = httpContext.Request.Path;
        var query    = httpContext.Request.QueryString.HasValue
                           ? httpContext.Request.QueryString.Value
                           : string.Empty;
        var exType   = exception.GetType().Name;

        var (status, title) = exception switch
        {
            BadHttpRequestException     => (StatusCodes.Status400BadRequest,          "Bad Request"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,        "Unauthorized"),
            KeyNotFoundException        => (StatusCodes.Status404NotFound,            "Not Found"),
            InvalidOperationException   => (StatusCodes.Status400BadRequest,          "Invalid Operation"),
            _                           => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };


        if (status >= 500)
        {
            _logger.LogError(
                exception,
                "[{TraceId}] Unhandled {ExceptionType} on {Method} {Path}{Query} → {StatusCode}",
                traceId, exType, method, path, query, status);
        }
        else
        {
            _logger.LogWarning(
                "[{TraceId}] Client error {ExceptionType} on {Method} {Path}{Query} → {StatusCode}: {Message}",
                traceId, exType, method, path, query, status, exception.Message);
        }

        var problem = new ProblemDetails
        {
            Status   = status,
            Title    = title,
            Detail   = exception.Message,
            Instance = httpContext.Request.Path
        };


        problem.Extensions["traceId"] = traceId;

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
