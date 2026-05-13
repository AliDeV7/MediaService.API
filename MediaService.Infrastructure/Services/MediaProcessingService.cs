using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Application.Interfaces.Validation;
using MediaService.Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MediaService.Infrastructure.Services
{
    /// <summary>
    /// Implementation of media processing service.
    /// Handles image validation, processing, and preparation for storage.
    /// </summary>
    public sealed class MediaProcessingService : IMediaProcessingService
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IImageValidator _imageValidator;

        public MediaProcessingService(
            IImageProcessor imageProcessor,
            IImageValidator imageValidator)
        {
            _imageProcessor = imageProcessor;
            _imageValidator = imageValidator;
        }

        /// <summary>
        /// Processes a raw image stream and prepares it for storage.
        /// </summary>
        public async Task<UploadMediaDto> ProcessImageFromStreamAsync(
            Stream fileStream,
            string fileName,
            ImageProcessingDto options,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(fileStream);
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

            // Ensure stream is at the beginning
            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }

            // 1. Validate complete image (size, extension, MIME type, magic bytes, dimensions)
            await _imageValidator.ValidateCompleteAsync(fileStream, fileName, cancellationToken);

            // 2. Process main image (convert to WebP if requested)
            Stream mainStream;
            if (options.ConvertToWebP)
            {
                mainStream = await _imageProcessor.ConvertToWebPAsync(
                    fileStream,
                    options.WebPQuality,
                    cancellationToken);
            }
            else
            {
                // Use original stream
                if (fileStream.CanSeek)
                {
                    fileStream.Position = 0;
                }
                mainStream = fileStream;
            }

            // 3. Get dimensions from processed stream
            var dimensions = await _imageProcessor.GetImageDimensionsAsync(mainStream, cancellationToken);
            if (dimensions == null)
            {
                throw new InvalidOperationException("Failed to extract image dimensions after processing.");
            }

            // 4. Generate thumbnail if requested
            Stream? thumbnailStream = null;
            if (options.GenerateThumbnail)
            {
                thumbnailStream = await _imageProcessor.GenerateThumbnailAsync(
                    mainStream,
                    options.ThumbnailWidth,
                    options.WebPQuality,
                    cancellationToken);
            }

            // 5. Compute hash for the processed main stream
            var hash = await FileStorageHelper.ComputeHashAsync(mainStream, cancellationToken);

            // 6. Determine file extension
            var extension = FileStorageHelper.DetermineFileExtension(fileName, options.ConvertToWebP);

            // 7. Generate save paths
            var (mainPath, thumbnailPath) = FileStorageHelper.GenerateSavePathsForImage(
                extension,
                options.GenerateThumbnail);

            // 8. Determine MIME type
            var mimeType = FileTypeHelper.ExtensionMimeMap.TryGetValue(extension, out var mimeTypes)
                ? mimeTypes[0]
                : "application/octet-stream";

            // 9. Create UploadMediaDto
            return new UploadMediaDto
            {
                FileStream = mainStream,
                FileName = fileName,
                MimeType = mimeType,
                FileSize = mainStream.Length,
                SavePath = mainPath,
                ThumbnailStream = thumbnailStream,
                ThumbnailSavePath = thumbnailPath,
                Hash = hash,
                OriginalExtension = Path.GetExtension(fileName).TrimStart('.'),
                Metadata = new Dictionary<string, object>
                {
                    ["Width"] = dimensions.Value.Width,
                    ["Height"] = dimensions.Value.Height,
                    ["ProcessedAt"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    ["ConvertedToWebP"] = options.ConvertToWebP,
                    ["ThumbnailGenerated"] = options.GenerateThumbnail
                }
            };
        }

        /// <summary>
        /// Processes a Base64-encoded image and prepares it for storage.
        /// Automatically detects MIME type and generates file name if not provided.
        /// </summary>
        /// <param name="base64Data">Base64-encoded image data (with or without data URI prefix)</param>
        /// <param name="fileName">Optional file name. If null, empty, or without extension, a name will be auto-generated</param>
        /// <param name="options">Processing options for thumbnail and WebP conversion</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Processed upload media with original and transformed images</returns>
        public async Task<UploadMediaDto> ProcessImageFromBase64Async(
            string base64Data,
            string? fileName,
            ImageProcessingDto options,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(base64Data);

            // Extract pure Base64 and detect MIME type from data URI prefix
            var (cleanBase64, detectedMimeType) = FileTypeHelper.ExtractBase64AndMimeType(base64Data);

            // Determine final file name based on user input and detected MIME type
            var finalFileName = FileTypeHelper.DetermineFileName(fileName, detectedMimeType);

            Stream fileStream;
            try
            {
                // Convert Base64 to Stream using helper
                fileStream = FileStorageHelper.ConvertBase64ToStream(cleanBase64);
            }
            catch (FormatException ex)
            {
                throw new ValidationException("Invalid Base64 format.", ex);
            }

            // Delegate to stream processing method
            return await ProcessImageFromStreamAsync(fileStream, finalFileName, options, cancellationToken);
        }
    }
}
