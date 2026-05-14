using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MediaService.Application.Interfaces.Auth;
using MediaService.Core.Exceptions;
using MediaService.Infrastructure.Options;
using MediaService.Core.Entities;

namespace MediaService.Infrastructure.Services.Auth
{
    /// <summary>
    /// Implementation of JWT token generation and validation service
    /// </summary>
    public sealed class JwtTokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _validationParameters;

        /// <summary>
        /// Initializes a new instance of JwtTokenService
        /// </summary>
        /// <param name="jwtSettings">JWT configuration settings</param>
        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _validationParameters = CreateValidationParameters();
        }

        /// <summary>
        /// Generates a JWT access token for an authenticated service client
        /// </summary>
        /// <param name="client">The authenticated service client</param>
        /// <returns>JWT token string</returns>
        public string GenerateToken(ServiceClient client)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            );
            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, client.ClientId),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, client.Name),
                new(ClaimTypes.Role, client.Role)
            };

            // Add permissions as individual claims
            foreach (var permission in client.Permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Validates a JWT token and extracts the claims
        /// </summary>
        /// <param name="token">JWT token string to validate</param>
        /// <returns>Token claims if valid</returns>
        /// <exception cref="InvalidTokenException">Thrown when token is invalid or expired</exception>
        public TokenClaims ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(
                    token,
                    _validationParameters,
                    out var validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;

                return new TokenClaims
                {
                    ClientId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty,
                    Name = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                    Role = principal.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    Permissions = principal.FindAll("permission")
                        .Select(c => c.Value)
                        .ToList(),
                    ExpiresAt = jwtToken.ValidTo
                };
            }
            catch (SecurityTokenExpiredException)
            {
                throw new InvalidTokenException("Token has expired");
            }
            catch (SecurityTokenException ex)
            {
                throw new InvalidTokenException($"Invalid token: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException($"Token validation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the token expiration duration in seconds
        /// </summary>
        /// <returns>Expiration time in seconds</returns>
        public int GetExpirationSeconds()
        {
            return _jwtSettings.ExpirationMinutes * 60;
        }

        /// <summary>
        /// Creates token validation parameters from JWT settings
        /// </summary>
        /// <returns>Token validation parameters</returns>
        private TokenValidationParameters CreateValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
                ),
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
