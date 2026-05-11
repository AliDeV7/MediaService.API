namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// View model for media file upload from HTTP multipart/form-data request.
    /// Contains framework-specific IFormFile type.
    /// </summary>
    public class ImageUploadFileRequest
    {
        /// <summary>
        /// File from multipart/form-data request.
        /// </summary>
        public IFormFile File { get; set; } = null!;

        /// <summary>
        /// Whether to generate a thumbnail for images.
        /// </summary>
        public bool? GenerateThumbnail { get; set; }

        /// <summary>
        /// Whether to convert images to WebP format.
        /// </summary>
        public bool? ConvertToWebP { get; set; }

        /// <summary>
        /// Thumbnail width in pixels.
        /// </summary>
        public int? ThumbnailWidth { get; set; }

        /// <summary>
        /// WebP compression quality (1-100).
        /// </summary>
        public int? WebPQuality { get; set; }
    }
}
