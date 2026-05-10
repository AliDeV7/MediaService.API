namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration options for file storage.
    /// </summary>
    public sealed class FileStorageOptions
    {
        /// <summary>
        /// Configuration section name in appsettings.json.
        /// </summary>
        public const string SectionName = "FileStorage";

        /// <summary>
        /// Root directory path for storing uploaded files.
        /// Default: "wwwroot/uploads"
        /// </summary>
        public string StorageRootPath { get; set; } = Path.Combine("wwwroot", "uploads");

        /// <summary>
        /// Base URL for constructing public file URLs.
        /// Example: "https://yourdomain.com/uploads"
        /// </summary>
        public string BaseUrl { get; set; } = "/uploads";

        /// <summary>
        /// Maximum allowed file size in bytes.
        /// Default: 10 MB
        /// </summary>
        public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024;

        /// <summary>
        /// Allowed file extensions (lowercase, with dot).
        /// </summary>
        public HashSet<string> AllowedExtensions { get; set; } = new()
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp",
            ".pdf", ".doc", ".docx", ".txt",
            ".mp4", ".avi", ".mov", ".mkv"
        };

        /// <summary>
        /// Quality for WebP image conversion (1-100).
        /// Default: 85
        /// </summary>
        public int ImageQuality { get; set; } = 85;

        /// <summary>
        /// Maximum dimension (width/height) for thumbnail generation.
        /// Default: 400
        /// </summary>
        public int ThumbnailMaxSize { get; set; } = 400;

        /// <summary>
        /// Quality for thumbnail generation (1-100).
        /// Default: 80
        /// </summary>
        public int ThumbnailQuality { get; set; } = 80;
    }
}
