namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Response DTO containing file stream and metadata for serving files.
    /// Framework-agnostic - uses only BCL types (Stream, string, long).
    /// </summary>
    public class MediaFileResponse
    {
        /// <summary>
        /// File content stream.
        /// </summary>
        public Stream Stream { get; set; } = null!;

        /// <summary>
        /// MIME type for Content-Type header.
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Original file name.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes.
        /// </summary>
        public long FileSize { get; set; }
    }
}
