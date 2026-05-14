using MediaService.Core.Entities;

namespace MediaService.Application.Interfaces.Auth
{
    /// <summary>
    /// Service for generating and validating JWT tokens
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT access token for an authenticated service client
        /// </summary>
        /// <param name="client">The authenticated service client</param>
        /// <returns>JWT token string</returns>
        string GenerateToken(ServiceClient client);

        /// <summary>
        /// Validates a JWT token and extracts the claims
        /// </summary>
        /// <param name="token">JWT token string to validate</param>
        /// <returns>Token claims if valid</returns>
        /// <exception cref="Core.Exceptions.InvalidTokenException">Thrown when token is invalid or expired</exception>
        TokenClaims ValidateToken(string token);

        /// <summary>
        /// Gets the token expiration duration in seconds
        /// </summary>
        /// <returns>Expiration time in seconds</returns>
        int GetExpirationSeconds();
    }
}
