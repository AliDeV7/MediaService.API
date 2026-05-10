namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// View model for media file upload from HTTP multipart/form-data request.
    /// Contains framework-specific IFormFile type.
    /// </summary>
    public class MediaUploadRequest
    {
        /// <summary>
        /// File from multipart/form-data request.
        /// </summary>
        public IFormFile File { get; set; } = null!;

        /// <summary>
        /// Optional title/description for the media file.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Optional alt text for accessibility (images).
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// Whether to generate a thumbnail for images.
        /// </summary>
        public bool GenerateThumbnail { get; set; } = true;

        /// <summary>
        /// Whether to convert images to WebP format.
        /// </summary>
        public bool ConvertToWebP { get; set; } = true;

        /// <summary>
        /// Thumbnail width in pixels.
        /// </summary>
        public int ThumbnailWidth { get; set; } = 400;

        /// <summary>
        /// WebP compression quality (1-100).
        /// </summary>
        public int WebPQuality { get; set; } = 80;

        /// <summary>
        /// Optional sorting order.
        /// </summary>
        public int? SortingOrder { get; set; }
    }
}
