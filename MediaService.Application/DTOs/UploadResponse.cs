namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Response model for successful file upload
    /// </summary>
    public class UploadResponse
    {
        /// <summary>
        /// Unique filename (MD5 hash + extension)
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Relative file path for URL generation
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// MIME type
        /// </summary>
        public string FileType { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Thumbnail path if generated
        /// </summary>
        public string? ThumbnailPath { get; set; }

        /// <summary>
        /// Thumbnail filename if generated
        /// </summary>
        public string? ThumbnailFileName { get; set; }

        /// <summary>
        /// Full URL to access the file (base URL + FilePath)
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// Full URL to access thumbnail if generated
        /// </summary>
        public string? ThumbnailUrl { get; set; }

        /// <summary>
        /// MD5 hash of file content
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Original extension before conversion
        /// </summary>
        public string OriginalExtension { get; set; } = string.Empty;

        /// <summary>
        /// When the file was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
