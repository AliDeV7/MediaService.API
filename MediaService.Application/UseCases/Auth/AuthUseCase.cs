using MediaService.Application.DTOs;
using MediaService.Application.Interfaces.Auth;

namespace MediaService.Application.UseCases.Auth
{
    /// <summary>
    /// Use case for handling client authentication and token generation
    /// </summary>
    public sealed class AuthUseCase : IAuthUseCase
    {
        private readonly IClientAuthenticationService _clientAuthService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of AuthUseCase
        /// </summary>
        public AuthUseCase(
            IClientAuthenticationService clientAuthService,
            ITokenService tokenService)
        {
            _clientAuthService = clientAuthService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authenticates a client and generates an access token
        /// </summary>
        /// <param name="request">Client credentials</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Token response containing access token and metadata</returns>
        /// <exception cref="InvalidCredentialsException">Thrown when credentials are invalid</exception>
        public async Task<TokenResponseDto> ExecuteAsync(
            TokenRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var client = await _clientAuthService.AuthenticateAsync(
                request.ClientId,
                request.ClientSecret,
                cancellationToken);

            var token = _tokenService.GenerateToken(client);

            return new TokenResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = _tokenService.GetExpirationSeconds(),
                IssuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }
}
