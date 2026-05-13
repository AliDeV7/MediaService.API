using MediaService.Core.Common;
using MediaService.Core.Exceptions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using FileNotFoundException = MediaService.Core.Exceptions.FileNotFoundException;

namespace MediaService.Presentation.Api.Middleware
{
    /// <summary>
    /// Global exception handler middleware that catches all unhandled exceptions
    /// and returns standardized error responses
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions and returns appropriate HTTP responses
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, errorCode, message) = MapExceptionToResponse(exception);
            context.Response.StatusCode = (int)statusCode;

            // Generate correlation ID for tracking
            var correlationId = context.TraceIdentifier ?? Guid.NewGuid().ToString();

            // Log with correlation ID
            _logger.LogError(
                exception,
                "[{CorrelationId}] Unhandled exception: {ErrorCode} - {Message}",
                correlationId,
                errorCode,
                exception.Message
            );

            var response = ApiResponse<object>.FailureResponse(
                errorCode,
                message,
                errors: null
            );

            // Add correlation ID for production support
            if (!_environment.IsDevelopment())
            {
                response.Error!.Errors = new Dictionary<string, string[]>
                {
                    ["CorrelationId"] = new[] { correlationId }
                };
            }
            else
            {
                // Development: full details + correlation ID
                response.Error!.Errors = new Dictionary<string, string[]>
                {
                    ["CorrelationId"] = new[] { correlationId },
                    ["StackTrace"] = new[] { exception.StackTrace ?? "No stack trace available" },
                    ["InnerException"] = new[] { exception.InnerException?.Message ?? "None" },
                    ["ExceptionType"] = new[] { exception.GetType().FullName ?? "Unknown" }
                };
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment(),
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }


        /// <summary>
        /// Maps exception types to HTTP status codes and error details
        /// </summary>
        private static (HttpStatusCode StatusCode, string ErrorCode, string Message) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                // Custom domain exceptions
                FileSizeLimitException ex => (
                    HttpStatusCode.BadRequest,
                    ex.ErrorCode,
                    ex.Message
                ),

                UnsupportedFileTypeException ex => (
                    HttpStatusCode.BadRequest,
                    ex.ErrorCode,
                    ex.Message
                ),

                InvalidFileException ex => (
                    HttpStatusCode.BadRequest,
                    ex.ErrorCode,
                    ex.Message
                ),

                FileNotFoundException ex => (
                    HttpStatusCode.NotFound,
                    ex.ErrorCode,
                    ex.Message
                ),

                UnauthorizedException ex => (
                    HttpStatusCode.Unauthorized,
                    ex.ErrorCode,
                    ex.Message
                ),

                MediaServiceException ex => (
                    HttpStatusCode.BadRequest,
                    ex.ErrorCode,
                    ex.Message
                ),

                // Framework exceptions
                ArgumentNullException ex => (
                    HttpStatusCode.BadRequest,
                    "INVALID_ARGUMENT",
                    $"Required argument is missing: {ex.ParamName}"
                ),

                ArgumentException ex => (
                    HttpStatusCode.BadRequest,
                    "INVALID_ARGUMENT",
                    ex.Message
                ),

                InvalidOperationException ex => (
                    HttpStatusCode.BadRequest,
                    "INVALID_OPERATION",
                    ex.Message
                ),

                UnauthorizedAccessException => (
                    HttpStatusCode.Forbidden,
                    "FORBIDDEN",
                    "You do not have permission to access this resource"
                ),

                // Generic fallback
                _ => (
                    HttpStatusCode.InternalServerError,
                    "INTERNAL_SERVER_ERROR",
                    "An unexpected error occurred. Please try again later."
                )
            };
        }
    }
}
