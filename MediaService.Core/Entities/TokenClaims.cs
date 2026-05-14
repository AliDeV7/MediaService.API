namespace MediaService.Core.Entities
{
    /// <summary>
    /// Represents claims extracted from a validated JWT token
    /// </summary>
    public sealed class TokenClaims
    {
        /// <summary>
        /// Client identifier from the token
        /// </summary>
        public required string ClientId { get; init; }

        /// <summary>
        /// Client name from the token
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Client role from the token
        /// </summary>
        public required string Role { get; init; }

        /// <summary>
        /// List of permissions granted to the client
        /// </summary>
        public required IReadOnlyList<string> Permissions { get; init; }

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        public required DateTime ExpiresAt { get; init; }
    }
}
