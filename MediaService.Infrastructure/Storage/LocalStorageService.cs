using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Infrastructure.Helpers;
using MediaService.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace MediaService.Infrastructure.Storage
{
    /// <summary>
    /// Stores and manages media files on the local filesystem.
    /// Files are organized under: {StorageRootPath}/media/{year}/{month}/{filename}
    /// Public URLs are constructed using the configured BaseUrl.
    /// </summary>
    public sealed class LocalStorageService : IStorageService
    {
        private readonly FileStorageOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="LocalStorageService"/>.
        /// </summary>
        public LocalStorageService(IOptions<FileStorageOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Saves media file and optional thumbnail to local storage.
        /// Returns the relative path of the saved file.
        /// </summary>
        public async Task<string> SaveMediaAsync(
            UploadMediaDto uploadMedia,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(uploadMedia);
            ArgumentNullException.ThrowIfNull(uploadMedia.FileStream);
            ArgumentException.ThrowIfNullOrWhiteSpace(uploadMedia.SavePath);

            // Save main file
            var absolutePath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, uploadMedia.SavePath);
            FileStorageHelper.EnsureDirectoryExists(absolutePath);

            await FileStorageHelper.WriteStreamToFileAsync(
                absolutePath,
                uploadMedia.FileStream,
                cancellationToken);

            // Save thumbnail if provided
            if (uploadMedia.ThumbnailStream != null && !string.IsNullOrWhiteSpace(uploadMedia.ThumbnailSavePath))
            {
                var absoluteThumbPath = FileStorageHelper.ToAbsolutePath(
                    _options.StorageRootPath,
                    uploadMedia.ThumbnailSavePath);

                FileStorageHelper.EnsureDirectoryExists(absoluteThumbPath);

                await FileStorageHelper.WriteStreamToFileAsync(
                    absoluteThumbPath,
                    uploadMedia.ThumbnailStream,
                    cancellationToken);
            }

            return uploadMedia.SavePath;
        }

        /// <summary>
        /// Deletes a file from storage using its relative path.
        /// Also attempts to delete the corresponding thumbnail if it exists.
        /// </summary>
        public Task<bool> DeleteFileAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

            var absolutePath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, relativePath);
            var deleted = FileStorageHelper.DeleteFileIfExists(absolutePath);

            // Attempt to delete thumbnail (best-effort, no error if missing)
            var thumbPath = FileStorageHelper.BuildThumbnailRelativePath(relativePath);
            if (thumbPath is not null)
            {
                var absoluteThumbPath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, thumbPath);
                FileStorageHelper.DeleteFileIfExists(absoluteThumbPath);
            }

            return Task.FromResult(deleted);
        }

        /// <summary>
        /// Checks if a file exists in storage.
        /// </summary>
        public Task<bool> FileExistsAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

            var absolutePath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, relativePath);
            return Task.FromResult(FileStorageHelper.FileExists(absolutePath));
        }

        /// <summary>
        /// Checks if a thumbnail exists for the given image relative path.
        /// </summary>
        /// <param name="relativePath">Original image relative path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Tuple containing thumbnail path (if exists) and whether it exists.</returns>
        public async Task<(string? ThumbnailPath, bool Exists)> CheckThumbnailExistsAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            var thumbnailPath = FileStorageHelper.BuildThumbnailRelativePath(relativePath);

            if (thumbnailPath is null)
                return (null, false);

            var exists = await FileExistsAsync(thumbnailPath, cancellationToken);
            return (thumbnailPath, exists);
        }

        /// <summary>
        /// Constructs a public URL from a relative path.
        /// </summary>
        public string GetPublicUrl(string relativePath)
        {
            return FileStorageHelper.BuildPublicUrl(_options.BaseUrl, relativePath);
        }
    }

}
