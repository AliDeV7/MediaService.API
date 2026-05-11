namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Response model for successful file upload.
    /// Contains only essential information needed by clients.
    /// </summary>
    public class UploadResponse
    {
        /// <summary>
        /// Complete relative path from storage root (e.g., "/media/2026/05/abc123.webp")
        /// Clients should store this to reconstruct URLs later
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// Relative path to thumbnail (e.g., "/media/2026/05/abc123_thumb.webp")
        /// Clients should store this to reconstruct thumbnail URLs later
        /// </summary>
        public string? ThumbnailRelativePath { get; set; }

        /// <summary>
        /// MIME type (e.g., "image/webp", "application/pdf")
        /// </summary>
        public string FileType { get; set; } = string.Empty;

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
        /// SHA256/MD5 hash of file content for deduplication and integrity verification
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Original filename provided by user (for display purposes)
        /// </summary>
        public string OriginalFileName { get; set; } = string.Empty;

        /// <summary>
        /// Original file extension before conversion (e.g., ".jpg", ".png")
        /// </summary>
        public string OriginalExtension { get; set; } = string.Empty;

        /// <summary>
        /// When the file was uploaded
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
