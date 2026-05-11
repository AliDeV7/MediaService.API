namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// Request model for uploading an image from Base64-encoded string.
    /// </summary>
    public class ImageUploadBase64Request
    {
        /// <summary>
        /// Base64-encoded image data.
        /// </summary>
        public string Base64Data { get; set; } = null!;

        /// <summary>
        /// File name with extension (e.g., "photo.jpg").
        /// </summary>
        public string FileName { get; set; } = null!;

        /// <summary>
        /// Whether to generate a thumbnail for the image.
        /// </summary>
        public bool? GenerateThumbnail { get; set; }

        /// <summary>
        /// Whether to convert the image to WebP format.
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
