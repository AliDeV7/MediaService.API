using MediaService.Infrastructure.Options;
using MediaService.Infrastructure.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediaService.Infrastructure.Processing;
using MediaService.Application.Interfaces;
using MediaService.Infrastructure.Storage;
using MediaService.Infrastructure.Services;
using MediaService.Application.Interfaces.Validation;
using MediaService.Core.Configuration;

namespace MediaService.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering Infrastructure layer services into the DI container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all Infrastructure layer dependencies including validators, storage services,
        /// and strongly-typed configuration options.
        /// </summary>
        /// <param name="services">The service collection to register into.</param>
        /// <param name="configuration">The application configuration used to bind options.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind FileStorageOptions to the "FileStorage" section in appsettings.json
            services.Configure<FileStorageOptions>(
                configuration.GetSection(FileStorageOptions.SectionName));

            // Configure validation options
            services.Configure<ImageValidationOptions>(
                configuration.GetSection(ImageValidationOptions.SectionName));

            // Register validators
            services.AddScoped<IImageValidator, ImageValidator>();

            // Register file-related services
            services.AddScoped<IImageProcessor, ImageProcessor>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<IMediaProcessingService, MediaProcessingService>();

            return services;
        }
    }
}
