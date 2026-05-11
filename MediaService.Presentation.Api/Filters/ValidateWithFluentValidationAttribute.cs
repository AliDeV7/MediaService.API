using Microsoft.AspNetCore.Mvc;

namespace MediaService.Presentation.Api.Filters
{
    /// <summary>
    /// Attribute to apply FluentValidation to a specific action
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateWithFluentValidationAttribute<T> : ServiceFilterAttribute where T : class
    {
        public ValidateWithFluentValidationAttribute()
            : base(typeof(FluentValidationFilter<T>))
        {
        }
    }
}
