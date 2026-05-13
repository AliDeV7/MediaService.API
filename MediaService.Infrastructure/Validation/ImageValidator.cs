using MediaService.Application.Interfaces.Validation;
using MediaService.Core.Configuration;
using MediaService.Core.Enums;
using MediaService.Core.Exceptions;
using MediaService.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;

namespace MediaService.Infrastructure.Validation
{
    /// <summary>
    /// Validator for image files
    /// Handles validation of image size, dimensions, format, and integrity
    /// </summary>
    public sealed class ImageValidator : BaseMediaValidator, IImageValidator
    {
        private readonly ImageValidationOptions _options;

        /// <summary>
        /// Initializes a new instance of ImageValidator
        /// </summary>
        /// <param name="options">Image validation configuration options</param>
        public ImageValidator(IOptions<ImageValidationOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Maximum allowed image file size in bytes
        /// </summary>
        protected override long MaxFileSizeBytes => _options.MaxFileSizeBytes;

        /// <summary>
        /// Media file type category
        /// </summary>
        protected override FileType MediaFileType => FileType.Image;

        /// <summary>
        /// Validates image dimensions (width and height)
        /// </summary>
        /// <param name="stream">Image stream to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="InvalidImageDimensionsException">
        /// Thrown when image dimensions exceed allowed limits
        /// </exception>
        public async Task ValidateImageDimensionsAsync(
            Stream stream,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);

            if (!stream.CanRead)
                throw new ArgumentException("Stream must be readable", nameof(stream));

            if (!stream.CanSeek)
                throw new ArgumentException("Stream must be seekable", nameof(stream));

            var originalPosition = stream.Position;

            try
            {
                stream.Position = 0;

                using var image = await Image.LoadAsync(stream, cancellationToken);

                if (image.Width > _options.MaxWidth)
                {
                    throw new InvalidImageDimensionsException(
                        $"Image width ({image.Width}px) exceeds maximum allowed width ({_options.MaxWidth}px)");
                }

                if (image.Height > _options.MaxHeight)
                {
                    throw new InvalidImageDimensionsException(
                        $"Image height ({image.Height}px) exceeds maximum allowed height ({_options.MaxHeight}px)");
                }

                if (image.Width < _options.MinWidth)
                {
                    throw new InvalidImageDimensionsException(
                        $"Image width ({image.Width}px) is below minimum allowed width ({_options.MinWidth}px)");
                }

                if (image.Height < _options.MinHeight)
                {
                    throw new InvalidImageDimensionsException(
                        $"Image height ({image.Height}px) is below minimum allowed height ({_options.MinHeight}px)");
                }
            }
            catch (UnknownImageFormatException ex)
            {
                throw new InvalidImageDimensionsException(
                    "Invalid or unsupported image format", ex);
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        /// <summary>
        /// Performs complete validation of an image file
        /// </summary>
        /// <param name="stream">Image stream to validate</param>
        /// <param name="fileName">Name of the file including extension</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task ValidateCompleteAsync(
            Stream stream,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream);

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(
                    "Filename cannot be null or empty",
                    nameof(fileName));

            ValidateFileSize(stream.Length);

            ValidateFileExtension(fileName);

            var extension = FileTypeHelper.GetExtension(fileName);
            var mimeType = FileTypeHelper.GetMimeTypeFromExtension(extension);

            ValidateMimeType(fileName, mimeType);

            await ValidateMagicBytesAsync(
                stream,
                fileName,
                cancellationToken);

            await ValidateImageDimensionsAsync(
                stream,
                cancellationToken);
        }
    }
}
