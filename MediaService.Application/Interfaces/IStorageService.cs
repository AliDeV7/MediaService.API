using MediaService.Application.DTOs;

namespace MediaService.Application.Interfaces
{
    /// <summary>
    /// Storage service for saving and managing media files.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Saves a media file (image, video, document, etc.) to storage.
        /// Returns the saved file's relative path.
        /// </summary>
        Task<string> SaveMediaAsync(UploadMediaDto uploadMedia, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file from storage using its relative path.
        /// Also attempts to delete the corresponding thumbnail if it exists.
        /// </summary>
        Task<bool> DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a file exists in storage.
        /// </summary>
        Task<bool> FileExistsAsync(string relativePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a thumbnail exists for the given image relative path.
        /// </summary>
        /// <param name="relativePath">Original image relative path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Tuple containing thumbnail path (if exists) and whether it exists.</returns>
        Task<(string? ThumbnailPath, bool Exists)> CheckThumbnailExistsAsync(
            string relativePath,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Constructs a public URL from a relative path.
        /// </summary>
        string GetPublicUrl(string relativePath);
    }


}
