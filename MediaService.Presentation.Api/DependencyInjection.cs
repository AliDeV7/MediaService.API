using FluentValidation;
using MediaService.Presentation.Api.ViewModels.Validators;
using MediaService.Presentation.Api.ViewModels;

namespace MediaService.Presentation.Api
{
    /// <summary>
    /// Dependency injection configuration for the Presentation layer
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers Presentation layer services including validators
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            // Register FluentValidation validators
            services.AddScoped<IValidator<MediaUploadRequest>, MediaUploadRequestValidator>();

            return services;
        }
    }
}
