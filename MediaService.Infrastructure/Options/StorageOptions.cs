namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Strongly-typed options for local file storage configuration.
    /// Bound from appsettings.json section "Storage".
    /// </summary>
    public sealed class StorageOptions
    {
        public const string SectionName = "Storage";

        /// <summary>
        /// Absolute base path on disk where media files are stored.
        /// Example: "/var/media" or "C:\\media"
        /// </summary>
        public string BasePath { get; init; } = string.Empty;

        /// <summary>
        /// Base URL used to construct public-facing file URLs.
        /// Example: "https://cdn.example.com" or "https://localhost:5001"
        /// </summary>
        public string BaseUrl { get; init; } = string.Empty;
    }
}
