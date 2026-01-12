using UserManagementApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

// --- PIPELINE CONFIGURATION ---

// 1. Error Handling (Catch-all for everything below)
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2. Authentication (Secure the endpoints)
app.UseMiddleware<AuthMiddleware>();

// 3. Logging (Log the final outcome of the request)
app.UseMiddleware<LoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
