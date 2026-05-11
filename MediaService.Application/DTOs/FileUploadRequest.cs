using MediaService.Core.Constants;

namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request model for file upload using stream (multipart/form-data)
    /// This is the RECOMMENDED and most efficient method for file uploads
    /// </summary>
    public class FileUploadRequest
    {
        /// <summary>
        /// File content as stream
        /// </summary>
        public Stream FileStream { get; set; } = null!;

        /// <summary>
        /// Original file name (required to determine file type)
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
        /// Default: true
        /// </summary>
        public bool GenerateThumbnail { get; set; } = MediaServiceConstants.DefaultGenerateThumbnail;

        /// <summary>
        /// Whether to convert images to WebP format for better compression.
        /// Default: true
        /// </summary>
        public bool ConvertToWebP { get; set; } = MediaServiceConstants.DefaultConvertToWebP;

        /// <summary>
        /// Thumbnail width in pixels (height will be calculated to maintain aspect ratio).
        /// </summary>
        public int ThumbnailWidth { get; set; } = MediaServiceConstants.DefaultThumbnailWidth;

        /// <summary>
        /// WebP compression quality (1-100, higher = better quality but larger file).
        /// </summary>
        public int WebPQuality { get; set; } = MediaServiceConstants.DefaultWebPQuality;
    }
}
