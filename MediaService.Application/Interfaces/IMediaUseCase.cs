using MediaService.Application.DTOs;

namespace MediaService.Application.Interfaces
{

    /// <summary>
    /// Defines media file operations use cases.
    /// Accepts framework-agnostic DTOs only.
    /// </summary>
    public interface IMediaUseCase
    {
        /// <summary>
        /// Uploads a single file with optional image processing.
        /// </summary>
        /// <param name="request">File upload request containing stream and metadata.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        Task<UploadResponse> UploadFileAsync(
            FileUploadRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file by relative path.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted successfully, false if not found.</returns>
        Task<bool> DeleteFileAsync(
            string relativePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a file exists in storage.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if file exists, false otherwise.</returns>
        Task<bool> FileExistsAsync(
            string relativePath,
            CancellationToken cancellationToken = default);
    }
}
