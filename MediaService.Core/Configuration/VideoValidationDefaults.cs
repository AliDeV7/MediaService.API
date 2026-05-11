namespace MediaService.Core.Configuration
{
    /// <summary>
    /// Default validation settings for video files.
    /// </summary>
    public static class VideoValidationDefaults
    {
        /// <summary>
        /// Maximum file size for videos in bytes (100 MB).
        /// </summary>
        public const long MaxFileSizeBytes = 100 * 1024 * 1024;

        /// <summary>
        /// Maximum video duration in seconds.
        /// </summary>
        public const int MaxDurationSeconds = 600; // 10 minutes

        /// <summary>
        /// Allowed video extensions.
        /// </summary>
        public static readonly string[] AllowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
    }
}
