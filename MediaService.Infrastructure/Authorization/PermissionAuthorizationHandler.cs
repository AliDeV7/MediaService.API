using Microsoft.AspNetCore.Authorization;

namespace MediaService.Infrastructure.Authorization
{
    /// <summary>
    /// Authorization handler that validates if the user has the required permission claim.
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// Handles the authorization requirement by checking if the user has the required permission claim.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The permission requirement to validate.</param>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Check if the user has the required permission claim
            if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
