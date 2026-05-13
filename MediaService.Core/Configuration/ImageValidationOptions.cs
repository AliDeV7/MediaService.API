namespace MediaService.Core.Configuration
{
    /// <summary>
    /// Configuration options for image validation
    /// </summary>
    public sealed class ImageValidationOptions
    {
        public const string SectionName = "MediaValidation:Image";

        /// <summary>
        /// Maximum allowed image file size in bytes
        /// Default: 10 MB
        /// </summary>
        public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024;

        /// <summary>
        /// Maximum allowed image width in pixels
        /// </summary>
        public int MaxWidth { get; set; } = 8000;

        /// <summary>
        /// Maximum allowed image height in pixels
        /// </summary>
        public int MaxHeight { get; set; } = 8000;

        /// <summary>
        /// Minimum allowed image width in pixels
        /// </summary>
        public int MinWidth { get; set; } = 1;

        /// <summary>
        /// Minimum allowed image height in pixels
        /// </summary>
        public int MinHeight { get; set; } = 1;
    }
}
