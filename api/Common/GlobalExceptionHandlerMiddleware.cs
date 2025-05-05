using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using api.Common.Models;

namespace api.Common
{
    public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch
            {
                await HandleExceptionAsync(context);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            var response = ApiResponse.ErrorResponse(
                message: "Unknown error. Please contact admin for support.",
                statusCode: 500
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
