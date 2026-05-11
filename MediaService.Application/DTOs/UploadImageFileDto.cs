namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request model for file upload using stream (multipart/form-data).
    /// This is the RECOMMENDED and most efficient method for file uploads.
    /// Default values are applied during mapping from ViewModel.
    /// </summary>
    public class UploadImageFileDto
    {
        /// <summary>
        /// File content as stream
        /// </summary>
        public Stream FileStream { get; set; } = null!;

        /// <summary>
        /// Original file name (required to determine file type).
        /// Example: "image.png", "document.pdf"
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// MIME type of the file (e.g., "image/png", "application/pdf")
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Whether to generate a thumbnail for images.
        /// Default is applied during mapping if not provided.
        /// </summary>
        public bool GenerateThumbnail { get; set; }

        /// <summary>
        /// Whether to convert images to WebP format for better compression.
        /// Default is applied during mapping if not provided.
        /// </summary>
        public bool ConvertToWebP { get; set; }

        /// <summary>
        /// Thumbnail width in pixels (height will be calculated to maintain aspect ratio).
        /// Default is applied during mapping if not provided.
        /// </summary>
        public int ThumbnailWidth { get; set; }

        /// <summary>
        /// WebP compression quality (1-100, higher = better quality but larger file).
        /// Default is applied during mapping if not provided.
        /// </summary>
        public int WebPQuality { get; set; }
    }

}
