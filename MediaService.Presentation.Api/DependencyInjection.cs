using FluentValidation;
using MediaService.Presentation.Api.ViewModels.Validators;
using MediaService.Presentation.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MediaService.Presentation.Api.Filters;

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

            // Register the filter as scoped
            services.AddScoped(typeof(FluentValidationFilter<>));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Disable automatic model validation
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
