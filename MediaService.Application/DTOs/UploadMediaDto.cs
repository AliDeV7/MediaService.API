namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Generic media upload DTO (media-agnostic).
    /// Contains the file stream ready to be saved.
    /// </summary>
    public sealed class UploadMediaDto
    {
        /// <summary>
        /// File stream ready to be saved.
        /// </summary>
        public required Stream FileStream { get; init; }

        /// <summary>
        /// File name with extension.
        /// </summary>
        public required string FileName { get; init; }

        /// <summary>
        /// MIME type of the file.
        /// </summary>
        public required string MimeType { get; init; }

        /// <summary>
        /// File size in bytes.
        /// </summary>
        public required long FileSize { get; init; }

        /// <summary>
        /// Relative path where the file should be saved.
        /// Example: "media/2026/05/abc123.webp"
        /// </summary>
        public required string SavePath { get; init; }

        /// <summary>
        /// Optional thumbnail stream.
        /// </summary>
        public Stream? ThumbnailStream { get; init; }

        /// <summary>
        /// Optional thumbnail save path.
        /// Example: "media/2026/05/abc123_thumb.webp"
        /// </summary>
        public string? ThumbnailSavePath { get; init; }

        /// <summary>
        /// Content hash for deduplication.
        /// </summary>
        public required string Hash { get; init; }

        /// <summary>
        /// Original file extension before processing.
        /// Example: ".jpg"
        /// </summary>
        public required string OriginalExtension { get; init; }

        /// <summary>
        /// Optional metadata (width, height, duration, etc.).
        /// </summary>
        public Dictionary<string, object>? Metadata { get; init; }
    }

}
