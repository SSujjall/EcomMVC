using System.Security.Claims;

namespace EcomSiteMVC.Utilities.CustomMiddlewares
{
    public class AdminRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only check on the home page
            if (context.Request.Path == "/" || context.Request.Path == "/Home" || context.Request.Path == "/Home/Index")
            {
                // Check if user is authenticated
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    // Check if user is admin or superadmin
                    var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    if (role == "Admin" || role == "Superadmin")
                    {
                        // Redirect to dashboard
                        context.Response.Redirect("/Dashboard");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

    // Extension method to make it easier to add the middleware
    public static class AdminRedirectMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminRedirect(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminRedirectMiddleware>();
        }
    }
}
