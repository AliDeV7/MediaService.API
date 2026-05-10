using MediaService.Application.DTOs;
using MediaService.Core.Entities;

namespace MediaService.Application.Interfaces
{
    /// <summary>
    /// Interface for file storage operations.
    /// Supports both IFormFile (efficient) and Base64 (compatibility) inputs.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Saves file from IFormFile (efficient, recommended method).
        /// Used when receiving multipart/form-data uploads.
        /// Automatically converts images to WebP and generates thumbnails.
        /// </summary>
        /// <param name="request">File upload request containing file and metadata.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>MediaFile entity with file metadata.</returns>
        Task<MediaFile> SaveFileAsync(FileUploadRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves file from base64 string (for backward compatibility).
        /// Used when client sends base64-encoded data in JSON.
        /// Automatically converts images to WebP and generates thumbnails.
        /// </summary>
        /// <param name="request">Base64 upload request containing encoded data and metadata.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>MediaFile entity with file metadata.</returns>
        Task<MediaFile> SaveFileAsync(Base64UploadRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves file content as stream for efficient memory usage.
        /// </summary>
        /// <param name="relativePath">Relative path of the file (e.g., "media/2026/05/abc123.webp").</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>File stream, or null if not found.</returns>
        Task<Stream?> GetFileStreamAsync(string relativePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file from storage using its relative path.
        /// Also attempts to delete the corresponding thumbnail if it exists.
        /// </summary>
        /// <param name="relativePath">Relative path of the file to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted successfully, false otherwise.</returns>
        Task<bool> DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a file exists in storage.
        /// </summary>
        /// <param name="relativePath">Relative path of the file to check.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if file exists, false otherwise.</returns>
        Task<bool> FileExistsAsync(string relativePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Constructs a public URL from a relative path.
        /// </summary>
        /// <param name="relativePath">Relative path of the file.</param>
        /// <returns>Public URL for accessing the file.</returns>
        string GetPublicUrl(string relativePath);
    }
}
