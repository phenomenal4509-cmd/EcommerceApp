using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.Exceptions;

/// <summary>
/// Catches every unhandled exception and returns a consistent RFC-7807
/// ProblemDetails JSON response. DbUpdateConcurrencyException is intentionally
/// skipped — endpoints own that with tailored 409 Conflict responses.
/// </summary>
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

        _logger.LogError(
            exception,
            "Unhandled exception on {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        var (status, title) = exception switch
        {
            BadHttpRequestException     => (StatusCodes.Status400BadRequest,          "Bad Request"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,        "Unauthorized"),
            KeyNotFoundException        => (StatusCodes.Status404NotFound,            "Not Found"),
            InvalidOperationException   => (StatusCodes.Status400BadRequest,          "Invalid Operation"),
            _                           => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var problem = new ProblemDetails
        {
            Status   = status,
            Title    = title,
            Detail   = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
