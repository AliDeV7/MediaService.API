namespace MediaService.Core.Entities
{
    /// <summary>
    /// Represents a media file stored in the system
    /// Contains metadata about the saved file
    /// </summary>
    public class MediaFile
    {
        /// <summary>
        /// Unique filename (MD5 hash + extension)
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Relative path for URL generation (e.g., "/media/abc123.webp")
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// MIME type (e.g., "image/webp", "image/jpeg")
        /// </summary>
        public string FileType { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// When the file was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thumbnail path if generated (e.g., "/media/abc123_thumb.webp")
        /// </summary>
        public string? ThumbnailPath { get; set; }

        /// <summary>
        /// Thumbnail filename if generated
        /// </summary>
        public string? ThumbnailFileName { get; set; }

        /// <summary>
        /// Original file extension before conversion
        /// </summary>
        public string OriginalExtension { get; set; } = string.Empty;

        /// <summary>
        /// MD5 hash of file content
        /// </summary>
        public string Hash { get; set; } = string.Empty;
    }
}
