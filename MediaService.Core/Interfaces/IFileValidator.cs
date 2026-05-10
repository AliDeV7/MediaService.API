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
        /// <returns>True if valid, false otherwise</returns>
        bool ValidateMimeType(string fileName, string contentType);
    }

}
