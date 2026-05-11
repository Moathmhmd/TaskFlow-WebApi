using System.Net;
using System.Text.Json;
using TaskFlow.Application.Common;
using TaskFlow.Application.Common.Exceptions;

namespace TaskFlow.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case BadRequestException:
                context.Response.StatusCode =
                    (int)HttpStatusCode.BadRequest;

                response.StatusCode = context.Response.StatusCode;
                response.Message = exception.Message;
                break;

            case NotFoundException:
                context.Response.StatusCode =
                    (int)HttpStatusCode.NotFound;

                response.StatusCode = context.Response.StatusCode;
                response.Message = exception.Message;
                break;

            case UnauthorizedException:
                context.Response.StatusCode =
                    (int)HttpStatusCode.Unauthorized;

                response.StatusCode = context.Response.StatusCode;
                response.Message = exception.Message;
                break;

            default:
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                response.StatusCode = context.Response.StatusCode;
                response.Message =
                    "An unexpected error occurred";
                break;
        }

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}