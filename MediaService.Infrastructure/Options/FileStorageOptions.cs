namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Strongly-typed configuration options for file storage behavior and validation rules
    /// </summary>
    public class FileStorageOptions
    {
        /// <summary>
        /// Configuration section name in appsettings.json
        /// </summary>
        public const string SectionName = "FileStorage";

        /// <summary>
        /// Maximum allowed file size in bytes (default: 50 MB)
        /// </summary>
        public long MaxFileSizeBytes { get; set; } = 52_428_800;

        /// <summary>
        /// Root path on disk where media files are stored
        /// </summary>
        public string StorageRootPath { get; set; } = "uploads";

        /// <summary>
        /// Base URL used to build public-facing file access URLs
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Allowed file extensions including the dot prefix (e.g. ".jpg", ".mp4")
        /// </summary>
        public List<string> AllowedExtensions { get; set; } =
        [
            ".jpg", ".jpeg", ".png", ".gif", ".webp",
        ".mp4", ".mov", ".avi", ".mkv",
        ".mp3", ".wav", ".ogg",
        ".pdf"
        ];

        /// <summary>
        /// Width in pixels for generated image thumbnails
        /// </summary>
        public int ThumbnailWidth { get; set; } = 400;
    }
}
