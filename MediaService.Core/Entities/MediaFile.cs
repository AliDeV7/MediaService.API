namespace MediaService.Core.Entities
{
    /// <summary>
    /// Represents a media file stored in the system
    /// Contains metadata about the saved file
    /// </summary>
    public class MediaFile
    {
        /// <summary>
        /// Original filename provided by user
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Relative path for storage (e.g., "media/2026/05/abc123.webp")
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// MIME type (e.g., "image/webp", "application/pdf")
        /// </summary>
        public string MimeType { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Image width in pixels (null for non-images)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Image height in pixels (null for non-images)
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// When the file was uploaded
        /// </summary>
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Thumbnail relative path if generated (e.g., "media/2026/05/abc123_thumb.webp")
        /// </summary>
        public string? ThumbnailPath { get; set; }

        /// <summary>
        /// Full public URL to access the file
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Full public URL to access thumbnail if generated
        /// </summary>
        public string? ThumbnailUrl { get; set; }

        /// <summary>
        /// MD5 hash of file content for deduplication
        /// </summary>
        public string? Hash { get; set; }

        /// <summary>
        /// Original file extension before conversion (e.g., ".jpg", ".png")
        /// </summary>
        public string? OriginalExtension { get; set; }

        /// <summary>
        /// Optional title/description
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Optional alt text for accessibility
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// Optional sorting order
        /// </summary>
        public int? SortingOrder { get; set; }
    }
}
