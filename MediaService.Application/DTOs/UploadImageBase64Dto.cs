namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request model for base64 file upload (JSON payload)
    /// Use this when client cannot send multipart/form-data (e.g., some AJAX scenarios)
    /// Note: Base64 encoding increases payload size by ~33%, use FileUploadRequest when possible
    /// </summary>
    public class UploadImageBase64Dto
    {
        /// <summary>
        /// Base64-encoded file data
        /// Can include data URI prefix (e.g., "data:image/png;base64,iVBORw0KG...")
        /// or just the base64 string
        /// </summary>
        public string Base64Data { get; set; } = string.Empty;

        /// <summary>
        /// Original file name (required for base64 uploads to determine file type)
        /// Example: "image.png", "document.pdf"
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Optional title/description for the media file
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Optional alt text for accessibility (images)
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// Whether to generate a thumbnail for images
        /// Default: true
        /// </summary>
        public bool GenerateThumbnail { get; set; } = true;

        /// <summary>
        /// Whether to convert images to WebP format for better compression
        /// Default: true
        /// </summary>
        public bool ConvertToWebP { get; set; } = true;

        /// <summary>
        /// Thumbnail width in pixels (height will be calculated to maintain aspect ratio)
        /// Default: 400px
        /// </summary>
        public int ThumbnailWidth { get; set; } = 400;

        /// <summary>
        /// WebP compression quality (1-100, higher = better quality but larger file)
        /// Default: 80
        /// </summary>
        public int WebPQuality { get; set; } = 80;

        /// <summary>
        /// Optional sorting order for display purposes
        /// </summary>
        public int? SortingOrder { get; set; }
    }
}
