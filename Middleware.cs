using System.Net;
using System.Text.Json;

namespace UserManagementApi.Middleware
{
    // STEP 2: Logging Middleware
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);
            logger.LogInformation("HTTP {Method} {Path} responded {StatusCode}", 
                context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }

    // STEP 3: Error-Handling Middleware
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try { await next(context); }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal server error." }));
            }
        }
    }

    // STEP 4: Authentication Middleware (Simplified Token Check)
    public class AuthMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            // For this activity, we validate a simple hardcoded token
            var token = context.Request.Headers["Authorization"].ToString();
            if (token != "Bearer techhive-secret-token")
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await next(context);
        }
    }
}
