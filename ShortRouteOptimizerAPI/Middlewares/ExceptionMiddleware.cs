using ShortRouteOptimizerAPI.Models;
using System.Net;

namespace ShortRouteOptimizerAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(new ExceptionDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString());

        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionHandler(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
