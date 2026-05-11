namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration options for file storage and validation.
    /// This is the single source of truth for all file-related constraints.
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
        /// Default: 50 MB
        /// </summary>
        public long MaxFileSizeBytes { get; set; } = 50 * 1024 * 1024;

        /// <summary>
        /// Allowed file extensions (lowercase, with dot).
        /// </summary>
        public HashSet<string> AllowedExtensions { get; set; } = new()
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg",
            ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx",
            ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv",
            ".mp3", ".wav", ".ogg", ".flac", ".aac"
        };

        /// <summary>
        /// Quality for WebP image conversion (1-100).
        /// Default: 80
        /// </summary>
        public int ImageQuality { get; set; } = 80;

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
