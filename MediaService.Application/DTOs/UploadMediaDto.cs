namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Generic Media file upload request DTO (media-agnostic).
    /// Used across all media types (images, videos, documents).
    /// </summary>
    public sealed class UploadMediaDto
    {
        public required Stream FileStream { get; init; }
        public required string FileName { get; init; }
        public required string ContentType { get; init; }
        public required long FileSize { get; init; }
    }
}
