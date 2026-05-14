using MediaService.Application;
using MediaService.Infrastructure;
using MediaService.Presentation.Api;
using MediaService.Presentation.Api.Middleware;

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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Media Service API v1");
    });
}

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Serve static files from wwwroot/media (so uploaded images are accessible)
app.UseStaticFiles();

// Authentication & Authorization (now enabled)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
