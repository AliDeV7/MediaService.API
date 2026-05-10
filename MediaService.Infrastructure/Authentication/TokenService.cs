using MediaService.Application.DTOs;
using MediaService.Core.Entities;
using MediaService.Core.Exceptions;
using MediaService.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediaService.Infrastructure.Authentication
{
    /// <summary>
    /// JWT token service implementation for service-to-service authentication
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ClientOptions _clientOptions;

        public TokenService(
            IOptions<JwtOptions> jwtOptions,
            IOptions<ClientOptions> clientOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _clientOptions = clientOptions.Value;
        }

        /// <summary>
        /// Authenticates client credentials and issues a JWT token
        /// </summary>
        public Task<AuthResponse> AuthenticateAsync(AuthRequest request)
        {
            var client = ValidateCredentials(request.ClientId, request.ClientSecret);

            var token = GenerateToken(client);
            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes).ToUnixTimeSeconds();

            var response = new AuthResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresAt = expiresAt,
                ClientName = client.ClientName
            };

            return Task.FromResult(response);
        }

        /// <summary>
        /// Validates client credentials against registered clients
        /// </summary>
        private Client ValidateCredentials(string clientId, string clientSecret)
        {
            var client = _clientOptions.RegisteredClients
                .FirstOrDefault(c => c.ClientId == clientId && c.IsActive);

            if (client == null)
            {
                throw new UnauthorizedException("Invalid client credentials");
            }

            // In production, use BCrypt or similar for hashed comparison
            if (client.ClientSecret != clientSecret)
            {
                throw new UnauthorizedException("Invalid client credentials");
            }

            return client;
        }

        /// <summary>
        /// Generates JWT token with client claims
        /// </summary>
        private string GenerateToken(Client client)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, client.ClientId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("client_id", client.ClientId),
            new("client_name", client.ClientName)
        };

            // Add role claims
            foreach (var role in client.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
