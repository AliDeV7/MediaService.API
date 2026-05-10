using MediaService.Core.Enums;

namespace MediaService.Core.Interfaces
{
    /// <summary>
    /// Interface for file validation operations
    /// </summary>
    public interface IFileValidator
    {
        /// <summary>
        /// Validates file size
        /// </summary>
        /// <param name="fileSize">File size in bytes</param>
        /// <exception cref="FileSizeLimitException">Thrown when file is too large</exception>
        void ValidateFileSize(long fileSize);

        /// <summary>
        /// Validates file extension
        /// </summary>
        /// <param name="fileName">File name with extension</param>
        /// <exception cref="UnsupportedFileTypeException">Thrown when file type is not allowed</exception>
        void ValidateFileExtension(string fileName);

        /// <summary>
        /// Gets file type from extension
        /// </summary>
        /// <param name="fileName">File name with extension</param>
        /// <returns>FileType enum value</returns>
        FileType GetFileType(string fileName);

        /// <summary>
        /// Validates MIME type matches extension
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="contentType">MIME type</param>
        /// <exception cref="UnsupportedFileTypeException">Thrown when MIME type doesn't match extension</exception>
        void ValidateMimeType(string fileName, string contentType);

        /// <summary>
        /// Validates file content against known magic byte signatures
        /// </summary>
        /// <param name="stream">File stream to inspect</param>
        /// <param name="contentType">Expected MIME type</param>
        /// <exception cref="UnsupportedFileTypeException">Thrown when content doesn't match expected type</exception>
        void ValidateMagicBytes(Stream stream, string contentType);

        /// <summary>
        /// Performs complete file validation including size, extension, MIME type, and magic bytes
        /// </summary>
        /// <param name="fileName">File name with extension</param>
        /// <param name="fileSize">File size in bytes</param>
        /// <param name="contentType">MIME type</param>
        /// <param name="stream">File stream for content validation</param>
        /// <exception cref="FileSizeLimitException">Thrown when file is too large</exception>
        /// <exception cref="UnsupportedFileTypeException">Thrown when file type validation fails</exception>
        void ValidateFile(string fileName, long fileSize, string contentType, Stream stream);
    }
}
