namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Response data for file deletion operations.
    /// </summary>
    public sealed class DeleteResponseDto
    {
        /// <summary>
        /// Relative path of the deleted file.
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// Relative path of the deleted thumbnail (if existed).
        /// </summary>
        public string? ThumbnailRelativePath { get; set; }

        /// <summary>
        /// Timestamp when the file was deleted (Unix timestamp).
        /// </summary>
        public long DeletedAt { get; set; }

        /// <summary>
        /// Indicates whether a thumbnail was also deleted.
        /// </summary>
        public bool ThumbnailDeleted { get; set; }
    }
}
