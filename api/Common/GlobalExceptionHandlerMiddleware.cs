using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using api.Common.Models;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = ApiResponse.ErrorResponse(
            message: "Unknown error",
            statusCode: 500
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        return context.Response.WriteAsJsonAsync(response);
    }
}