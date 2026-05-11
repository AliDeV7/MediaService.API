using FluentValidation;
using MediaService.Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MediaService.Presentation.Api.Filters
{
    /// <summary>
    /// Action filter that validates requests using FluentValidation
    /// and returns standardized ApiResponse format for validation errors
    /// </summary>
    public class FluentValidationFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public FluentValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Executes validation before the action method
        /// </summary>
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Find the argument of type T
            var argument = context.ActionArguments.Values
                .OfType<T>()
                .FirstOrDefault();

            if (argument == null)
            {
                await next();
                return;
            }

            // Validate using FluentValidation
            var validationResult = await _validator.ValidateAsync(argument);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var errorResponse = ApiResponse<object>.FailureResponse(
                    "VALIDATION_ERROR",
                    "One or more validation errors occurred.",
                    errors
                );

                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}
