using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Core.Configuration;
using MediaService.Core.Entities;

namespace MediaService.Application.UseCases
{
    /// <summary>
    /// Implements image file operations use cases.
    /// Orchestrates media processing and storage services, maps between DTOs and entities.
    /// </summary>
    public sealed class ImageUseCase : IImageUseCase
    {
        private readonly IStorageService _storageService;
        private readonly IMediaProcessingService _processingService;

        public ImageUseCase(
            IStorageService storageService,
            IMediaProcessingService processingService)
        {
            _storageService = storageService;
            _processingService = processingService;
        }

        /// <summary>
        /// Uploads a single image file with validation and optional image processing.
        /// </summary>
        /// <param name="request">File upload request containing stream and metadata.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        public async Task<UploadResponseDto> UploadFileAsync(
            UploadImageFileDto request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.FileStream);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FileName);

            // applies default values to optional parameters when not provided.
            ApplyDefaults(request);

            // 1. Build processing options from request
            var processingOptions = new ImageProcessingDto
            {
                GenerateThumbnail = request.GenerateThumbnail!.Value,
                ThumbnailWidth = request.ThumbnailWidth!.Value,
                ConvertToWebP = request.ConvertToWebP!.Value,
                WebPQuality = request.WebPQuality!.Value
            };

            // 2. Process the image (validation, transformation, metadata extraction)
            var uploadMedia = await _processingService.ProcessImageFromStreamAsync(
                request.FileStream,
                request.FileName,
                processingOptions,
                cancellationToken);

            // 3. Save to storage
            var relativePath = await _storageService.SaveMediaAsync(uploadMedia, cancellationToken);

            // 4. Build MediaFile entity from processed data
            var mediaFile = BuildMediaFile(uploadMedia, relativePath);

            // 5. Map to response DTO
            return MapToUploadResponse(mediaFile);
        }

        /// <summary>
        /// Uploads an image from Base64 data with validation and optional image processing.
        /// </summary>
        /// <param name="request">Base64 upload request containing encoded data and metadata.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        public async Task<UploadResponseDto> UploadBase64Async(
            UploadImageBase64Dto request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Base64Data);

            // applies default values to optional parameters when not provided.
            ApplyDefaults(request);

            // 1. Build processing options from request
            var processingOptions = new ImageProcessingDto
            {
                GenerateThumbnail = request.GenerateThumbnail!.Value,
                ThumbnailWidth = request.ThumbnailWidth!.Value,
                ConvertToWebP = request.ConvertToWebP!.Value,
                WebPQuality = request.WebPQuality!.Value
            };

            // 2. Process the image (validation, transformation, metadata extraction)
            var uploadMedia = await _processingService.ProcessImageFromBase64Async(
                request.Base64Data,
                request.FileName,
                processingOptions,
                cancellationToken);

            // 3. Save to storage
            var relativePath = await _storageService.SaveMediaAsync(uploadMedia, cancellationToken);

            // 4. Build MediaFile entity from processed data
            var mediaFile = BuildMediaFile(uploadMedia, relativePath);

            // 5. Map to response DTO
            return MapToUploadResponse(mediaFile);
        }

        /// <summary>
        /// Deletes an image by relative path.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted successfully, false if not found.</returns>
        public async Task<bool> DeleteFileAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
            return await _storageService.DeleteFileAsync(relativePath, cancellationToken);
        }

        /// <summary>
        /// Checks if an image exists in storage.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if file exists, false otherwise.</returns>
        public async Task<bool> FileExistsAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
            return await _storageService.FileExistsAsync(relativePath, cancellationToken);
        }

        /// <summary>
        /// Builds MediaFile entity from processed upload data.
        /// </summary>
        /// <param name="uploadMedia">Processed media data from processing service.</param>
        /// <param name="relativePath">Relative path where the file was saved.</param>
        /// <returns>MediaFile entity with all metadata populated.</returns>
        private static MediaFile BuildMediaFile(UploadMediaDto uploadMedia, string relativePath)
        {
            // Extract dimensions from metadata with null-safe handling
            var width = uploadMedia.Metadata?.TryGetValue("Width", out var w) == true && w != null
                ? Convert.ToInt32(w)
                : 0;

            var height = uploadMedia.Metadata?.TryGetValue("Height", out var h) == true && h != null
                ? Convert.ToInt32(h)
                : 0;

            return new MediaFile
            {
                Url = relativePath,
                ThumbnailUrl = uploadMedia.ThumbnailSavePath,
                FileName = uploadMedia.FileName,
                MimeType = uploadMedia.MimeType,
                FileSize = uploadMedia.FileSize,
                Hash = uploadMedia.Hash,
                OriginalExtension = uploadMedia.OriginalExtension,
                Width = width,
                Height = height,
                UploadedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }

        /// <summary>
        /// Maps MediaFile entity to UploadResponse DTO.
        /// Returns only client-facing information.
        /// </summary>
        /// <param name="mediaFile">MediaFile entity to map.</param>
        /// <returns>UploadResponse DTO with file metadata and URLs.</returns>
        private static UploadResponseDto MapToUploadResponse(MediaFile mediaFile)
        {
            return new UploadResponseDto
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

        /// <summary>
        /// Applies default values to optional image processing parameters when not provided.
        /// Uses constants from ImageProcessingDefaults to ensure valid values.
        /// </summary>
        /// <param name="request">Request object inheriting from ImageUploadRequestBase.</param>
        private static void ApplyDefaults(UploadImageBaseDto request)
        {
            request.GenerateThumbnail ??= ImageProcessingDefaults.GenerateThumbnail;
            request.ConvertToWebP ??= ImageProcessingDefaults.ConvertToWebP;
            request.ThumbnailWidth ??= ImageProcessingDefaults.ThumbnailWidth;
            request.WebPQuality ??= ImageProcessingDefaults.WebPQuality;
        }
    }
}
