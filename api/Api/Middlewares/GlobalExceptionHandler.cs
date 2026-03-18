using Microsoft.AspNetCore.Diagnostics;
using Api.Wrappers;

namespace Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, $"Unhandled exception occurred while processing the request: {exception.Message}");

        var statusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var errors = new List<string> { exception.Message };
#if DEBUG
        if (exception.StackTrace != null)
            errors.Add(exception.StackTrace);
#endif

        var response = new ApiResponse<object>(
            message: "An unexpected error occurred. Please try again later.",
            errors: errors
        );

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}