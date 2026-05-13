namespace MediaService.Application.Interfaces.Validation
{
    /// <summary>
    /// Base interface for all media validators
    /// </summary>
    public interface IMediaValidator
    {
        /// <summary>
        /// Validates file size against allowed limits
        /// </summary>
        /// <param name="fileSize">Size of the file in bytes</param>
        /// <exception cref="FileSizeLimitException">Thrown when file size exceeds the limit</exception>
        void ValidateFileSize(long fileSize);

        /// <summary>
        /// Validates file extension against allowed extensions
        /// </summary>
        /// <param name="fileName">Name of the file including extension</param>
        /// <exception cref="UnsupportedFileTypeException">Thrown when extension is not allowed</exception>
        void ValidateFileExtension(string fileName);

        /// <summary>
        /// Validates MIME type matches the file extension
        /// </summary>
        /// <param name="fileName">Name of the file including extension</param>
        /// <param name="contentType">MIME type to validate</param>
        /// <exception cref="InvalidMimeTypeException">Thrown when MIME type doesn't match extension</exception>
        void ValidateMimeType(string fileName, string contentType);

        /// <summary>
        /// Validates file magic bytes (file signature) to ensure file integrity
        /// </summary>
        /// <param name="stream">File stream to validate</param>
        /// <param name="fileName">Name of the file including extension</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="InvalidFileSignatureException">Thrown when magic bytes don't match expected signature</exception>
        Task ValidateMagicBytesAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
    }
}
