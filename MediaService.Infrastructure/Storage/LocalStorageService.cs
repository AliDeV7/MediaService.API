using MediaService.Core.DTOs;
using MediaService.Core.Entities;
using MediaService.Core.Exceptions;
using MediaService.Core.Interfaces;
using MediaService.Infrastructure.Helpers;
using MediaService.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace MediaService.Infrastructure.Storage
{
    /// <summary>
    /// Stores and manages media files on the local filesystem.
    /// Files are organized under: {StorageRootPath}/media/{year}/{month}/{filename}
    /// Public URLs are constructed using the configured BaseUrl.
    /// Images are automatically converted to WebP format with thumbnail generation.
    /// </summary>
    public sealed class LocalStorageService : IStorageService
    {
        private readonly FileStorageOptions _options;
        private readonly IImageProcessor _imageProcessor;
        private readonly IFileValidator _fileValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="LocalStorageService"/>.
        /// </summary>
        public LocalStorageService(
            IOptions<FileStorageOptions> options,
            IImageProcessor imageProcessor,
            IFileValidator fileValidator)
        {
            _options = options.Value;
            _imageProcessor = imageProcessor;
            _fileValidator = fileValidator;
        }

        /// <summary>
        /// Saves file from stream (efficient, recommended method).
        /// Used when receiving multipart/form-data uploads.
        /// Automatically converts images to WebP and generates thumbnails.
        /// </summary>
        public async Task<MediaFile> SaveFileAsync(
            FileUploadRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.FileStream);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FileName);

            var now = DateTime.UtcNow;
            var originalExtension = Path.GetExtension(request.FileName).ToLowerInvariant();

            // Validate file
            _fileValidator.ValidateFile(
                request.FileName,
                request.FileSize,
                request.ContentType,
                request.FileStream);

            request.FileStream.Position = 0;

            // Check if it's an image
            var isImage = await _imageProcessor.IsValidImageAsync(request.FileStream, cancellationToken);
            request.FileStream.Position = 0;

            // Generate unique filename and hash
            var baseName = FileStorageHelper.GenerateUniqueFileName();
            var hash = await ComputeHashAsync(request.FileStream, cancellationToken);
            request.FileStream.Position = 0;

            string relativePath;
            string? thumbnailRelativePath = null;
            long fileSize;
            int? width = null;
            int? height = null;

            if (isImage && request.ConvertToWebP)
            {
                // Get image dimensions
                var dimensions = await _imageProcessor.GetImageDimensionsAsync(request.FileStream, cancellationToken);
                if (dimensions.HasValue)
                {
                    width = dimensions.Value.Width;
                    height = dimensions.Value.Height;
                }
                request.FileStream.Position = 0;

                // Build paths for WebP image and thumbnail
                (relativePath, thumbnailRelativePath) = FileStorageHelper.BuildImagePaths(baseName, now);

                var absolutePath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, relativePath);
                var absoluteThumbPath = thumbnailRelativePath != null
                    ? FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, thumbnailRelativePath)
                    : null;

                FileStorageHelper.EnsureDirectoryExists(absolutePath);

                // Convert main image to WebP
                await using (var webpStream = await _imageProcessor.ConvertToWebPAsync(
                    request.FileStream,
                    request.WebPQuality,
                    cancellationToken))
                {
                    fileSize = await FileStorageHelper.WriteStreamToFileAsync(
                        absolutePath,
                        webpStream,
                        cancellationToken);
                }

                // Generate thumbnail if requested
                if (request.GenerateThumbnail && absoluteThumbPath != null)
                {
                    request.FileStream.Position = 0;
                    await using var thumbStream = await _imageProcessor.GenerateThumbnailAsync(
                        request.FileStream,
                        request.ThumbnailWidth,
                        request.WebPQuality,
                        cancellationToken);

                    await FileStorageHelper.WriteStreamToFileAsync(
                        absoluteThumbPath,
                        thumbStream,
                        cancellationToken);
                }
                else
                {
                    thumbnailRelativePath = null;
                }
            }
            else
            {
                // Non-image file or no conversion: save as-is
                (relativePath, _) = FileStorageHelper.BuildFilePath(baseName, originalExtension, now);

                var absolutePath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, relativePath);
                FileStorageHelper.EnsureDirectoryExists(absolutePath);

                fileSize = await FileStorageHelper.WriteStreamToFileAsync(
                    absolutePath,
                    request.FileStream,
                    cancellationToken);
            }

            // Create MediaFile entity
            return new MediaFile
            {
                FileName = request.FileName,
                FilePath = relativePath,
                ThumbnailPath = thumbnailRelativePath,
                FileSize = fileSize,
                MimeType = isImage && request.ConvertToWebP ? "image/webp" : request.ContentType,
                Width = width,
                Height = height,
                UploadedAt = now,
                Url = GetPublicUrl(relativePath),
                ThumbnailUrl = thumbnailRelativePath != null ? GetPublicUrl(thumbnailRelativePath) : null,
                Hash = hash,
                OriginalExtension = originalExtension,
                Title = request.Title,
                AltText = request.AltText,
                SortingOrder = request.SortingOrder
            };
        }


        /// <summary>
        /// Saves file from base64 string (for backward compatibility).
        /// Used when client sends base64-encoded data in JSON.
        /// Automatically converts images to WebP and generates thumbnails.
        /// </summary>
        public async Task<MediaFile> SaveFileAsync(
            Base64UploadRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.Base64Data);
            ArgumentException.ThrowIfNullOrWhiteSpace(request.FileName);

            // Remove data URI prefix if present (e.g., "data:image/png;base64,")
            var base64Data = request.Base64Data;
            if (base64Data.Contains(','))
            {
                base64Data = base64Data.Split(',')[1];
            }

            // Decode base64 to byte array
            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid base64 string.", nameof(request.Base64Data), ex);
            }

            // Detect MIME type from file extension if not provided
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };

            // Create FileUploadRequest from decoded data
            var fileUploadRequest = new FileUploadRequest
            {
                FileStream = new MemoryStream(fileBytes),
                FileName = request.FileName,
                ContentType = contentType,
                FileSize = fileBytes.Length,
                Title = request.Title,
                AltText = request.AltText,
                GenerateThumbnail = request.GenerateThumbnail,
                ConvertToWebP = request.ConvertToWebP,
                ThumbnailWidth = request.ThumbnailWidth,
                WebPQuality = request.WebPQuality,
                SortingOrder = request.SortingOrder
            };

            try
            {
                return await SaveFileAsync(fileUploadRequest, cancellationToken);
            }
            finally
            {
                await fileUploadRequest.FileStream.DisposeAsync();
            }
        }

        /// <summary>
        /// Retrieves file content as stream for efficient memory usage.
        /// </summary>
        public Task<Stream?> GetFileStreamAsync(
            string relativePath,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

            var absolutePath = FileStorageHelper.ToAbsolutePath(_options.StorageRootPath, relativePath);
            var stream = FileStorageHelper.OpenReadStream(absolutePath);

            return Task.FromResult(stream);
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
        /// Constructs a public URL from a relative path.
        /// </summary>
        public string GetPublicUrl(string relativePath)
        {
            return FileStorageHelper.BuildPublicUrl(_options.BaseUrl, relativePath);
        }

        // ── Private helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Computes MD5 hash of stream content for deduplication.
        /// Stream position is reset after computation.
        /// </summary>
        private static async Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken)
        {
            var hash = await MD5.HashDataAsync(stream, cancellationToken);
            stream.Position = 0;
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
