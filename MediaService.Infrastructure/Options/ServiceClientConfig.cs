namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration model for a single service client
    /// </summary>
    public sealed class ServiceClientConfig
    {
        /// <summary>
        /// Unique identifier for the client
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Client secret (plain text in development, hashed in production)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the client
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Role assigned to the client (e.g., Admin, Service, User)
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// List of permissions granted to the client
        /// </summary>
        public List<string> Permissions { get; set; } = new();

        /// <summary>
        /// Indicates whether the client is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
