using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PaperlessREST.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    
    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid operation"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };
        
        httpContext.Response.StatusCode = statusCode;
        
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = title,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Detail = statusCode != StatusCodes.Status500InternalServerError ? exception.Message : null
            }
        });
    }
}