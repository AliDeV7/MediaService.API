using MediaService.Application.Interfaces.Auth;
using MediaService.Core.Entities;
using MediaService.Core.Exceptions;
using MediaService.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MediaService.Infrastructure.Services.Auth
{
    /// <summary>
    /// Service for authenticating service clients using in-memory client storage
    /// </summary>
    public sealed class ClientAuthenticationService : IClientAuthenticationService
    {
        private readonly IReadOnlyList<ServiceClient> _clients;
        private readonly ILogger<ClientAuthenticationService> _logger;

        public ClientAuthenticationService(
            IOptions<List<ServiceClientConfig>> clientsOptions,
            ILogger<ClientAuthenticationService> logger)
        {
            // Map ServiceClientConfig to ServiceClient domain entity
            _clients = clientsOptions.Value
                .Select(config => new ServiceClient
                {
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    Name = config.Name,
                    Role = config.Role,
                    Permissions = config.Permissions,
                    IsActive = config.IsActive
                })
                .ToList()
                .AsReadOnly();

            _logger = logger;
        }

        /// <summary>
        /// Authenticates a client using client credentials
        /// </summary>
        public Task<ServiceClient> AuthenticateAsync(
            string clientId,
            string clientSecret,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new UnauthorizedException("Client ID is required");
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new UnauthorizedException("Client secret is required");
            }

            var client = _clients.FirstOrDefault(c => c.ClientId == clientId);

            if (client == null)
            {
                _logger.LogWarning("Authentication failed: Client {ClientId} not found", clientId);
                throw new UnauthorizedException("Invalid client credentials");
            }

            if (!client.IsActive)
            {
                _logger.LogWarning("Authentication failed: Client {ClientId} is inactive", clientId);
                throw new UnauthorizedException("Client is inactive");
            }

            if (!VerifyClientSecret(clientSecret, client.ClientSecret))
            {
                _logger.LogWarning("Authentication failed: Invalid secret for client {ClientId}", clientId);
                throw new UnauthorizedException("Invalid client credentials");
            }

            _logger.LogInformation("Client {ClientId} authenticated successfully", clientId);
            return Task.FromResult(client);
        }

        /// <summary>
        /// Validates if a client exists and is active
        /// </summary>
        public Task<bool> IsClientActiveAsync(
            string clientId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Task.FromResult(false);
            }

            var isActive = _clients.Any(c => c.ClientId == clientId && c.IsActive);
            return Task.FromResult(isActive);
        }


        /// <summary>
        /// Verifies the provided client secret against the stored hashed secret using SHA-256.
        /// Uses constant-time comparison to prevent timing attacks.
        /// </summary>
        /// <param name="providedSecret">The plain text secret provided by the client</param>
        /// <param name="storedHashedSecret">The SHA-256 hashed secret stored in configuration</param>
        /// <returns>True if the secrets match, false otherwise</returns>
        private static bool VerifyClientSecret(string providedSecret, string storedHashedSecret)
        {
            // Hash the provided secret using SHA-256
            using var sha256 = SHA256.Create();
            var providedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(providedSecret));
            var providedHashString = Convert.ToHexString(providedHash).ToLowerInvariant();

            // Compare hashes using constant-time comparison to prevent timing attacks
            var providedBytes = Encoding.UTF8.GetBytes(providedHashString);
            var storedBytes = Encoding.UTF8.GetBytes(storedHashedSecret);

            return CryptographicOperations.FixedTimeEquals(providedBytes, storedBytes);
        }
    }
}
