namespace MediaService.Core.Constants
{
    /// <summary>
    /// Configuration constants for media service
    /// </summary>
    public static class MediaServiceConstants
    {
        /// <summary>
        /// Maximum file size in bytes (50 MB)
        /// </summary>
        public const long MaxFileSizeBytes = 50 * 1024 * 1024;

        /// <summary>
        /// Allowed image extensions
        /// </summary>
        public static readonly string[] AllowedImageExtensions =
            { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg" };

        /// <summary>
        /// Allowed video extensions
        /// </summary>
        public static readonly string[] AllowedVideoExtensions =
            { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv" };

        /// <summary>
        /// Allowed document extensions
        /// </summary>
        public static readonly string[] AllowedDocumentExtensions =
            { ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx" };

        /// <summary>
        /// Allowed audio extensions
        /// </summary>
        public static readonly string[] AllowedAudioExtensions =
            { ".mp3", ".wav", ".ogg", ".flac", ".aac" };

        /// <summary>
        /// JWT token expiration in hours
        /// </summary>
        public const int TokenExpirationHours = 24;

        /// <summary>
        /// Thumbnail default width
        /// </summary>
        public const int DefaultThumbnailWidth = 400;

        /// <summary>
        /// WebP default quality
        /// </summary>
        public const int DefaultWebPQuality = 80;
    }
}
