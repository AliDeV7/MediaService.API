using MediaService.Application.Interfaces.Validation;
using MediaService.Core.Enums;
using MediaService.Core.Exceptions;
using MediaService.Infrastructure.Helpers;

namespace MediaService.Infrastructure.Validation
{
    /// <summary>
    /// Base abstract class for all media validators
    /// Contains common validation logic shared across all media types
    /// </summary>
    public abstract class BaseMediaValidator : IMediaValidator
    {
        /// <summary>
        /// Maximum allowed file size in bytes (defined by child classes)
        /// </summary>
        protected abstract long MaxFileSizeBytes { get; }

        /// <summary>
        /// File type category (Image, Video, Document, Audio)
        /// </summary>
        protected abstract FileType MediaFileType { get; }

        /// <summary>
        /// Gets allowed extensions from FileTypeHelper based on MediaFileType
        /// </summary>
        protected string[] AllowedExtensions => FileTypeHelper.GetAllowedExtensionsForType(MediaFileType);

        /// <summary>
        /// Gets allowed MIME types from FileTypeHelper based on MediaFileType
        /// </summary>
        protected string[] AllowedMimeTypes => FileTypeHelper.GetAllowedMimeTypesForType(MediaFileType);

        /// <summary>
        /// Validates file size against allowed limits
        /// </summary>
        /// <param name="fileSize">Size of the file in bytes</param>
        /// <exception cref="FileSizeLimitException">Thrown when file size exceeds the limit</exception>
        public void ValidateFileSize(long fileSize)
        {
            if (fileSize <= 0)
                throw new ArgumentException("File size must be greater than zero", nameof(fileSize));

            if (fileSize > MaxFileSizeBytes)
            {
                var maxSizeMB = MaxFileSizeBytes / (1024.0 * 1024.0);
                var actualSizeMB = fileSize / (1024.0 * 1024.0);
                throw new FileSizeLimitException(
                    $"File size ({actualSizeMB:F2} MB) exceeds the maximum allowed size ({maxSizeMB:F2} MB)");
            }
        }

        /// <summary>
        /// Validates file extension against allowed extensions
        /// </summary>
        /// <param name="fileName">Name of the file including extension</param>
        /// <exception cref="UnsupportedFileTypeException">Thrown when extension is not allowed</exception>
        public void ValidateFileExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));

            var extension = FileTypeHelper.GetExtension(fileName);

            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                throw new UnsupportedFileTypeException(
                    $"File extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", AllowedExtensions)}");
            }
        }

        /// <summary>
        /// Validates MIME type matches the file extension
        /// </summary>
        /// <param name="fileName">Name of the file including extension</param>
        /// <param name="contentType">MIME type to validate</param>
        /// <exception cref="InvalidMimeTypeException">Thrown when MIME type doesn't match extension</exception>
        public void ValidateMimeType(string fileName, string contentType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));

            if (string.IsNullOrWhiteSpace(contentType))
                throw new ArgumentException("Content type cannot be null or empty", nameof(contentType));

            var extension = FileTypeHelper.GetExtension(fileName);

            if (!FileTypeHelper.IsMimeTypeValid(extension, contentType))
            {
                var expectedMimeType = FileTypeHelper.GetMimeTypeFromExtension(extension);
                throw new InvalidMimeTypeException(
                    $"MIME type '{contentType}' does not match file extension '{extension}'. Expected: '{expectedMimeType}'");
            }
        }

        /// <summary>
        /// Validates file magic bytes (file signature) to ensure file integrity
        /// </summary>
        /// <param name="stream">File stream to validate</param>
        /// <param name="fileName">Name of the file including extension</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="InvalidFileSignatureException">Thrown when magic bytes don't match expected signature</exception>
        public async Task ValidateMagicBytesAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new ArgumentException("Stream must be readable", nameof(stream));

            if (!stream.CanSeek)
                throw new ArgumentException("Stream must be seekable", nameof(stream));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));

            var extension = FileTypeHelper.GetExtension(fileName);
            var mimeType = FileTypeHelper.GetMimeTypeFromExtension(extension);

            var isValid = await FileTypeHelper.MatchesMagicBytesAsync(stream, mimeType, cancellationToken);

            if (!isValid)
            {
                throw new InvalidFileSignatureException(
                    $"File signature does not match expected format for '{extension}' files. The file may be corrupted or have an incorrect extension.");
            }
        }
    }
}