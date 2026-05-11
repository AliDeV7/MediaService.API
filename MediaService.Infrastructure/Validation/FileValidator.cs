using MediaService.Core.Enums;
using MediaService.Core.Exceptions;
using MediaService.Application.Interfaces;
using MediaService.Infrastructure.Helpers;
using MediaService.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MediaService.Infrastructure.Validation
{
    /// <summary>
    /// Validates uploaded files based on configured rules for size, extension, MIME type, and magic bytes
    /// </summary>
    public class FileValidator : IFileValidator
    {
        private readonly FileStorageOptions _options;

        public FileValidator(IOptions<FileStorageOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Validates file size against the configured maximum limit
        /// </summary>
        /// <exception cref="FileSizeLimitException">Thrown when file exceeds the maximum allowed size</exception>
        public void ValidateFileSize(long fileSize)
        {
            //if (fileSize > _options.MaxFileSizeBytes)
                throw new FileSizeLimitException("File size exceeds the allowed limit.");
        }

        /// <summary>
        /// Validates that the file extension is in the allowed list
        /// </summary>
        /// <exception cref="UnsupportedFileTypeException">Thrown when the extension is not allowed</exception>
        public void ValidateFileExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName);

            //if (string.IsNullOrWhiteSpace(ext) ||
                //!_options.AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                throw new UnsupportedFileTypeException(ext ?? string.Empty);
        }

        /// <summary>
        /// Resolves the FileType enum from the file's extension
        /// </summary>
        public FileType GetFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName);

            return !string.IsNullOrWhiteSpace(ext) &&
                   FileTypeHelper.ExtensionTypeMap.TryGetValue(ext, out var fileType)
                ? fileType
                : FileType.Unknown;
        }

        /// <summary>
        /// Validates MIME type consistency with the file extension
        /// </summary>
        /// <exception cref="UnsupportedFileTypeException">Thrown when MIME type doesn't match extension</exception>
        public void ValidateMimeType(string fileName, string contentType)
        {
            var ext = Path.GetExtension(fileName);

            if (string.IsNullOrWhiteSpace(ext) || string.IsNullOrWhiteSpace(contentType))
                throw new UnsupportedFileTypeException("File name or content type is missing.");

            if (!FileTypeHelper.ExtensionMimeMap.TryGetValue(ext, out var allowedMimes))
                throw new UnsupportedFileTypeException($"No MIME type mapping found for extension '{ext}'.");

            if (!allowedMimes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
                throw new UnsupportedFileTypeException(
                    $"MIME type '{contentType}' does not match extension '{ext}'. Expected: {string.Join(", ", allowedMimes)}");
        }

        /// <summary>
        /// Validates file content against known magic byte signatures
        /// </summary>
        /// <exception cref="UnsupportedFileTypeException">Thrown when content doesn't match expected type</exception>
        public void ValidateMagicBytes(Stream stream, string contentType)
        {
            if (!FileTypeHelper.MatchesMagicBytes(stream, contentType))
                throw new UnsupportedFileTypeException(
                    $"File content (magic bytes) does not match the declared MIME type '{contentType}'.");
        }

        /// <summary>
        /// Performs complete file validation including size, extension, MIME type, and magic bytes
        /// </summary>
        public void ValidateFile(string fileName, long fileSize, string contentType, Stream stream)
        {
            ValidateFileSize(fileSize);
            ValidateFileExtension(fileName);
            ValidateMimeType(fileName, contentType);
            ValidateMagicBytes(stream, contentType);
        }
    }

}
