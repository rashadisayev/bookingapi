using System.Net;
using BookingApi.Models;
using BookingApp.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;

namespace BookingApi.Internal.Middlewares;


public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
    {
        var response = new ApiResponse<Unit>(false, Unit.Value);
        var statusCode = HttpStatusCode.InternalServerError;

        if (ex is ApplicationError appEx)
        {
            response.Message = appEx.Message;
            statusCode = ParseStatusCode(appEx.ErrorType);
        }

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/json";

        // Log to ILogger
        logger.LogError(ex, "An error occurred while processing the request.");

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }

    private static HttpStatusCode ParseStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.NotFound => HttpStatusCode.NotFound,
        ErrorType.BadRequest => HttpStatusCode.BadRequest,
        ErrorType.Forbidden => HttpStatusCode.Forbidden,
        _ => HttpStatusCode.InternalServerError,
    };
}