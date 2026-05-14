namespace MediaService.Core.Entities
{
    /// <summary>
    /// Represents a registered service client for authentication
    /// </summary>
    public sealed class ServiceClient
    {
        /// <summary>
        /// Unique identifier for the client
        /// </summary>
        public required string ClientId { get; init; }

        /// <summary>
        /// Client secret for authentication
        /// </summary>
        public required string ClientSecret { get; init; }

        /// <summary>
        /// Display name of the client service
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Role assigned to the client (e.g., "Service", "Admin")
        /// </summary>
        public required string Role { get; init; }

        /// <summary>
        /// List of permissions granted to this client
        /// </summary>
        public required IReadOnlyList<string> Permissions { get; init; }

        /// <summary>
        /// Indicates whether the client is active
        /// </summary>
        public required bool IsActive { get; init; }
    }
}
