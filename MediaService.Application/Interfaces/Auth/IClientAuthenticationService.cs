using MediaService.Core.Entities;

namespace MediaService.Application.Interfaces.Auth
{
    /// <summary>
    /// Service for authenticating service clients
    /// </summary>
    public interface IClientAuthenticationService
    {
        /// <summary>
        /// Authenticates a client using client credentials
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="clientSecret">Client secret</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Authenticated service client</returns>
        Task<ServiceClient> AuthenticateAsync(
            string clientId,
            string clientSecret,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates if a client exists and is active
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if client is valid and active</returns>
        Task<bool> IsClientActiveAsync(
            string clientId,
            CancellationToken cancellationToken = default);
    }
}
