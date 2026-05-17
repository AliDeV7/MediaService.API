namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Response model for successful media upload.
    /// Contains essential information for all media types with optional type-specific metadata.
    /// </summary>
    public class UploadResponseDto
    {
        /// <summary>
        /// Complete relative path from storage root (e.g., "/media/2026/05/abc123.webp")
        /// Clients should store this to reconstruct URLs later
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// Relative path to thumbnail (e.g., "/media/2026/05/abc123_thumb.webp")
        /// Clients should store this to reconstruct thumbnail URLs later
        /// Available for images and video previews
        /// </summary>
        public string? ThumbnailRelativePath { get; set; }

        /// <summary>
        /// MIME type (e.g., "image/webp", "video/mp4", "application/pdf")
        /// </summary>
        public string MimeType { get; set; } = string.Empty;

        /// <summary>
        /// Media category (Image, Video, Audio, Document, Unknown)
        /// </summary>
        public string FileType { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Image/video width in pixels (null for non-visual media)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Image/video height in pixels (null for non-visual media)
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Video/audio duration in seconds (null for non-media files)
        /// </summary>
        public double? DurationInSeconds { get; set; }

        /// <summary>
        /// Document page count (null for non-documents)
        /// </summary>
        public int? PageCount { get; set; }

        /// <summary>
        /// SHA256/MD5 hash of file content for deduplication and integrity verification
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Original filename provided by user (for display purposes)
        /// </summary>
        public string OriginalFileName { get; set; } = string.Empty;

        /// <summary>
        /// Original file extension before conversion (e.g., ".jpg", ".png", ".mp4")
        /// </summary>
        public string OriginalExtension { get; set; } = string.Empty;

        /// <summary>
        /// When the file was uploaded (Unix milliseconds)
        /// </summary>
        public long CreatedAt { get; set; }
    }

}
