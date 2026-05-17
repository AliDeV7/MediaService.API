using MediaService.Application.DTOs;
using MediaService.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaService.Presentation.Api.Controllers
{
    /// <summary>
    /// Handles authentication and token management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ILogger<AuthController> logger, IAuthUseCase authUseCase)
        {
            _logger = logger;
            _authUseCase = authUseCase;
        }

        /// <summary>
        /// Authenticates a service client and returns a JWT token
        /// </summary>
        /// <param name="request">Client credentials</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JWT token and expiration details</returns>
        /// <response code="200">Authentication successful</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponseDto>> GetToken(
            [FromBody] TokenRequestDto request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Token request received for client: {ClientId}", request.ClientId);

            var response = await _authUseCase.ExecuteAsync(
                request,
                cancellationToken);

            _logger.LogInformation("Token generated successfully for client: {ClientId}", request.ClientId);

            return Ok(response);
        }

        /// <summary>
        /// Validates the current token (requires authentication)
        /// </summary>
        /// <returns>Token validation status</returns>
        /// <response code="200">Token is valid</response>
        /// <response code="401">Token is invalid or expired</response>
        [HttpGet("validate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ValidateToken()
        {
            // If the request reaches here, the token is valid (middleware validated it)
            return Ok(new { message = "Token is valid", clientId = User.Identity?.Name });
        }
    }
}
