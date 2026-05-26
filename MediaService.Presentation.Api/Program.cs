using MediaService.Application;
using MediaService.Infrastructure;
using MediaService.Presentation.Api;
using MediaService.Presentation.Api.Middleware;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register Application layer services
builder.Services.AddApplicationServices();

// Register Infrastructure layer services
builder.Services.AddInfrastructure(builder.Configuration);

// Register Presentation layer services (includes JWT authentication)
builder.Services.AddPresentationServices(builder.Configuration);

// Configure Swagger/OpenAPI with JWT support
builder.Services.AddSwaggerDocumentation();

// Configure CORS (optional, useful for testing with frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Register Global Exception Handler (MUST be first) 
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Always show Swagger (useful during initial deployment testing)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Media Service API v1");
});

// Enable CORS
app.UseCors("AllowAll");

// Resolve WebRootPath safely — may be null if wwwroot folder does not exist on disk
var webRoot = builder.Environment.WebRootPath
              ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");

// Build the media directory path and create it if it does not exist
var mediaPath = Path.Combine(webRoot, "media");

if (!Directory.Exists(mediaPath))
{
    Directory.CreateDirectory(mediaPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(mediaPath),
    RequestPath = "/media"
});

// Authentication & Authorization (now enabled)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check mini-api
app.MapGet("/health", () => "OK");

app.Run();
