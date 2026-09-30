using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHa ndler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // DbConcurrency will handle the concurrency exception itself
        if (exception is DbUpdateConcurrencyException)
        {
            return false;
        }

        _logger.LogError(
            exception,
            "Unhandled exception on {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        var (status, title) = exception switch
        {
            BadHttpRequestException    => (StatusCodes.Status400BadRequest,  "Bad Request"),
            UnauthorizedAccessException=> (StatusCodes.Status401Unauthorized, "Unauthorized"),
            KeyNotFoundException       => (StatusCodes.Status404NotFound,    "Not Found"),
            InvalidOperationException  => (StatusCodes.Status400BadRequest,  "Invalid Operation"),
            _                          => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title  = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true; // exception handled 
    }
}
