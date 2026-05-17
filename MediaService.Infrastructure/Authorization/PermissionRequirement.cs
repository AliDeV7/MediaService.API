using Microsoft.AspNetCore.Authorization;

namespace MediaService.Infrastructure.Authorization
{
    /// <summary>
    /// Represents an authorization requirement that checks for a specific permission claim.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets the required permission name.
        /// </summary>
        public string Permission { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
        /// </summary>
        /// <param name="permission">The permission name required for authorization.</param>
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
