using CodeJudex.Content.Domain.Exceptions;
using System.Net;

namespace CodeJudex.Content.Api.Middleware;

/// <summary>
/// Provides centralized exception handling for the API.
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private record ErrorResponse(int StatusCode, string Message);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is BaseException baseEx)
        {
            context.Response.StatusCode = baseEx.StatusCode;
            return context.Response.WriteAsJsonAsync(new ErrorResponse(baseEx.StatusCode, baseEx.Message));
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsJsonAsync(new ErrorResponse(500, "Internal Server Error!"));
    }
}