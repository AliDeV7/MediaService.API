namespace MediaService.Infrastructure.Options
{

    /// <summary>
    /// Strongly-typed options for allowed service clients.
    /// Bound from appsettings.json section "Clients".
    /// </summary>
    public sealed class ClientCredentialOptions
    {
        public const string SectionName = "Clients";

        /// <summary>List of registered service clients allowed to authenticate.</summary>
        public List<ClientCredential> AllowedClients { get; init; } = [];
    }

    /// <summary>
    /// Represents a single allowed service client credential.
    /// </summary>
    public sealed class ClientCredential
    {
        /// <summary>Unique client identifier.</summary>
        public string ClientId { get; init; } = string.Empty;

        /// <summary>Client secret (store hashed in production).</summary>
        public string ClientSecret { get; init; } = string.Empty;

        /// <summary>Human-readable name for logging/auditing.</summary>
        public string ClientName { get; init; } = string.Empty;
    }
}
