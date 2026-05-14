using MediaService.Application.DTOs;

namespace MediaService.Application.Interfaces.Auth
{
    /// <summary>
    /// Contract for the client authentication use case
    /// </summary>
    public interface IAuthUseCase
    {
        /// <summary>
        /// Authenticates a client and generates an access token
        /// </summary>
        /// <param name="request">Client credentials</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Token response containing access token and metadata</returns>
        Task<TokenResponseDto> ExecuteAsync(TokenRequestDto request, CancellationToken cancellationToken = default);
    }
}
