namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// Base class for image upload request view models containing common processing options.
    /// </summary>
    public class ImageUploadRequestBase
    {
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
