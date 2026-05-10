using MediaService.Application.DTOs;

namespace MediaService.Infrastructure.Authentication
{
    /// <summary>
    /// Handles JWT token generation and client authentication
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Authenticates client credentials and issues a JWT token
        /// </summary>
        /// <param name="request">Client credentials</param>
        /// <returns>Authentication response with token</returns>
        /// <exception cref="UnauthorizedException">Thrown when credentials are invalid</exception>
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
    }
}
