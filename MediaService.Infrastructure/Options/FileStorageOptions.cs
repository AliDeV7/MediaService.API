namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration options for file storage.
    /// Defines the root storage path and base URL for constructing public file URLs.
    /// </summary>
    public sealed class FileStorageOptions
    {
        /// <summary>
        /// Configuration section name in appsettings.json.
        /// </summary>
        public const string SectionName = "FileStorage";

        /// <summary>
        /// Root directory path where all media files are stored.
        /// Example: "C:/MyApp/Storage" or "/var/www/storage"
        /// </summary>
        public string StorageRootPath { get; set; } = string.Empty;

        /// <summary>
        /// Base URL for constructing public file URLs.
        /// Example: "https://example.com/media" or "https://cdn.example.com"
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;
    }
}
