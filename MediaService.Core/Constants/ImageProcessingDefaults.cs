namespace MediaService.Core.Constants
{
    /// <summary>
    /// Default values and constraints for image processing operations.
    /// These are used when optional parameters are not provided in requests.
    /// </summary>
    public static class ImageProcessingDefaults
    {
        /// <summary>
        /// Default width for thumbnail generation (in pixels).
        /// </summary>
        public const int ThumbnailWidth = 400;

        /// <summary>
        /// Default quality for WebP conversion (1-100).
        /// </summary>
        public const int WebPQuality = 80;

        /// <summary>
        /// Default value for thumbnail generation.
        /// </summary>
        public const bool GenerateThumbnail = false;

        /// <summary>
        /// Default value for WebP conversion.
        /// </summary>
        public const bool ConvertToWebP = false;

        /// <summary>
        /// Minimum allowed thumbnail width (in pixels).
        /// </summary>
        public const int MinThumbnailWidth = 50;

        /// <summary>
        /// Maximum allowed thumbnail width (in pixels).
        /// </summary>
        public const int MaxThumbnailWidth = 2000;

        /// <summary>
        /// Minimum allowed quality value (1-100).
        /// </summary>
        public const int MinQuality = 1;

        /// <summary>
        /// Maximum allowed quality value (1-100).
        /// </summary>
        public const int MaxQuality = 100;
    }

}
