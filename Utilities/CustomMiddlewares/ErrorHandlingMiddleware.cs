
using Newtonsoft.Json;

namespace EcomSiteMVC.Utilities.CustomMiddlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set the response status code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message // Optionally, log this or send more detailed info
            };

            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            return context.Response.WriteAsync(jsonResponse);
        }
    }

    // Extension method to make it easier to add the middleware
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
