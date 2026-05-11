namespace MediaService.Core.Configuration
{
    /// <summary>
    /// Default validation rules and constraints for image uploads.
    /// These define acceptable file sizes, dimensions, and formats.
    /// </summary>
    public static class ImageValidationDefaults
    {
        /// <summary>
        /// Configuration section name in appsettings.json.
        /// </summary>
        public const string SectionName = "ImageValidation";

        /// <summary>
        /// Maximum allowed file size for image uploads (in bytes).
        /// Default: 10 MB
        /// </summary>
        public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        /// <summary>
        /// Minimum allowed image width (in pixels).
        /// </summary>
        public const int MinWidth = 50;

        /// <summary>
        /// Maximum allowed image width (in pixels).
        /// </summary>
        public const int MaxWidth = 10000;

        /// <summary>
        /// Minimum allowed image height (in pixels).
        /// </summary>
        public const int MinHeight = 50;

        /// <summary>
        /// Maximum allowed image height (in pixels).
        /// </summary>
        public const int MaxHeight = 10000;

        /// <summary>
        /// Allowed MIME types for image uploads.
        /// </summary>
        public static readonly string[] AllowedMimeTypes =
        {
        "image/jpeg",
        "image/jpg",
        "image/png",
        "image/webp",
        "image/gif",
        "image/bmp",
        "image/tiff"
    };

        /// <summary>
        /// Allowed file extensions for image uploads (without dot).
        /// </summary>
        public static readonly string[] AllowedExtensions =
        {
        "jpg",
        "jpeg",
        "png",
        "webp",
        "gif",
        "bmp",
        "tiff",
        "tif"
    };

        /// <summary>
        /// Maximum allowed aspect ratio (width/height).
        /// Default: 10.0 (e.g., 1000x100 is allowed)
        /// </summary>
        public const double MaxAspectRatio = 10.0;

        /// <summary>
        /// Minimum allowed aspect ratio (width/height).
        /// Default: 0.1 (e.g., 100x1000 is allowed)
        /// </summary>
        public const double MinAspectRatio = 0.1;
    }
}
