using MediaService.Application.Interfaces;
using MediaService.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MediaService.Application
{
    /// <summary>
    /// Provides dependency injection configuration for the Application layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers Application layer services to the dependency injection container.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IImageUseCase, ImageUseCase>();

            return services;
        }
    }
}
