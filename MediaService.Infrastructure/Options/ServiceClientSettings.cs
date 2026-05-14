namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration settings for authorized service clients
    /// </summary>
    public sealed class ServiceClientSettings
    {
        /// <summary>
        /// Configuration section name in appsettings.json
        /// </summary>
        public const string SectionName = "ServiceClients";

        /// <summary>
        /// List of authorized service clients
        /// </summary>
        public List<ServiceClientConfig> Clients { get; set; } = new();
    }
}
