using MediaService.Core.Interfaces;
using MediaService.Infrastructure.Options;
using MediaService.Infrastructure.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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

            // Register file-related services
            services.AddScoped<IFileValidator, FileValidator>();

            // ImageProcessor and LocalStorageService will be added here later

            return services;
        }
    }
}
