using MediaService.Application;
using MediaService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register Application layer services
builder.Services.AddApplicationServices();

// Register Infrastructure layer services
builder.Services.AddInfrastructure(builder.Configuration);

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Media Service API",
        Version = "v1",
        Description = "API for managing media file uploads and storage"
    });

    // Enable XML comments if you have them
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // options.IncludeXmlComments(xmlPath);
});

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

// Authentication/Authorization (skipped for now)
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
