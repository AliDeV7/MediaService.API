using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Core.Entities;

namespace MediaService.Application.UseCases
{
    /// <summary>
    /// Implements media file operations use cases.
    /// Orchestrates storage service and maps between DTOs and entities.
    /// </summary>
    public sealed class MediaUseCase : IMediaUseCase
    {
        private readonly IStorageService _storageService;

        public MediaUseCase(IStorageService storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        /// <summary>
        /// Uploads a single file with optional image processing.
        /// </summary>
        public async Task<UploadResponse> UploadFileAsync(
            UploadFileRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var mediaFile = await _storageService.SaveFileAsync(request, cancellationToken);

            return MapToUploadResponse(mediaFile);
        }

        /// <summary>
        /// Deletes a file by relative path.
        /// </summary>
        public async Task<bool> DeleteFileAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
            return await _storageService.DeleteFileAsync(relativePath, cancellationToken);
        }

        /// <summary>
        /// Checks if a file exists in storage.
        /// </summary>
        public async Task<bool> FileExistsAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
            return await _storageService.FileExistsAsync(relativePath, cancellationToken);
        }

        /// <summary>
        /// Maps MediaFile entity to UploadResponse DTO.
        /// Returns only client-facing information.
        /// </summary>
        private static UploadResponse MapToUploadResponse(MediaFile mediaFile)
        {
            return new UploadResponse
            {
                // Relative paths (for client database storage)
                RelativePath = mediaFile.Url,
                ThumbnailRelativePath = mediaFile.ThumbnailUrl,

                // Metadata (for client database storage)
                FileType = mediaFile.MimeType,
                FileSize = mediaFile.FileSize,
                Width = mediaFile.Width,
                Height = mediaFile.Height,

                // Identification & integrity
                Hash = mediaFile.Hash,
                OriginalFileName = mediaFile.FileName,
                OriginalExtension = mediaFile.OriginalExtension,

                // Timestamp
                CreatedAt = mediaFile.UploadedAt
            };
        }


    }
}
