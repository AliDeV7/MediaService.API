using Microsoft.AspNetCore.Authorization;

namespace MediaService.Presentation.Api.Attributes
{
    /// <summary>
    /// Specifies that the class or method requires a specific permission for authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequirePermissionAttribute"/> class.
        /// </summary>
        /// <param name="permission">The required permission name.</param>
        public RequirePermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
