using MediaService.Application.Interfaces;
using MediaService.Application.Interfaces.Auth;
using MediaService.Application.UseCases;
using MediaService.Application.UseCases.Auth;
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
            services.AddScoped<IAuthUseCase, AuthUseCase>();

            return services;
        }
    }
}
