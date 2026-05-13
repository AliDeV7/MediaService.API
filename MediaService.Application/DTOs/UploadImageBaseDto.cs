namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Base class for image upload requests containing common processing options.
    /// </summary>
    public class UploadImageBaseDto
    {
        /// <summary>
        /// Original file name (required to determine file type).
        /// Example: "image.png", "document.pdf"
        /// </summary>
        public string? FileName { get; set; } = string.Empty;

        /// <summary>
        /// Whether to generate a thumbnail for images.
        /// Default is applied during mapping if not provided.
        /// </summary>
        public bool? GenerateThumbnail { get; set; }

        /// <summary>
        /// Whether to convert images to WebP format for better compression.
        /// Default is applied during mapping if not provided.
        /// </summary>
        public bool? ConvertToWebP { get; set; }

        /// <summary>
        /// Thumbnail width in pixels (height will be calculated to maintain aspect ratio).
        /// Default is applied during mapping if not provided.
        /// </summary>
        public int? ThumbnailWidth { get; set; }

        /// <summary>
        /// WebP compression quality (1-100, higher = better quality but larger file).
        /// Default is applied during mapping if not provided.
        /// </summary>
        public int? WebPQuality { get; set; }
    }
}
