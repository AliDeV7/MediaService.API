using MediaService.Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MediaService.Infrastructure.Processing
{
    /// <summary>
    /// Provides image processing operations including thumbnail generation, format conversion, validation, and dimension extraction.
    /// </summary>
    public class ImageProcessor : IImageProcessor
    {
        /// <summary>
        /// Generates a thumbnail by resizing the image to fit within the specified maximum dimensions while maintaining aspect ratio.
        /// </summary>
        /// <param name="inputStream">The input image stream.</param>
        /// <param name="maxSize">Maximum width or height in pixels (default: 400).</param>
        /// <param name="quality">WebP quality (1-100, default: 100).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A stream containing the WebP-encoded thumbnail.</returns>
        public async Task<Stream> GenerateThumbnailAsync(
            Stream inputStream,
            int maxSize = 400,
            int quality = 100,
            CancellationToken cancellationToken = default)
        {
            inputStream.Position = 0;

            using var image = await Image.LoadAsync<Rgba32>(inputStream, cancellationToken);

            // Resize using ResizeMode.Max (fits within maxSize x maxSize, maintains aspect ratio)
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(maxSize, maxSize),
                Mode = ResizeMode.Max
            }));

            var outputStream = new MemoryStream();
            await image.SaveAsWebpAsync(outputStream, new WebpEncoder { Quality = quality }, cancellationToken);
            outputStream.Position = 0;

            return outputStream;
        }

        /// <summary>
        /// Converts an image to WebP format.
        /// </summary>
        /// <param name="inputStream">The input image stream.</param>
        /// <param name="quality">WebP quality (1-100, default: 80).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A stream containing the WebP-encoded image.</returns>
        public async Task<Stream> ConvertToWebPAsync(
            Stream inputStream,
            int quality = 80,
            CancellationToken cancellationToken = default)
        {
            inputStream.Position = 0;

            using var image = await Image.LoadAsync(inputStream, cancellationToken);

            var outputStream = new MemoryStream();
            await image.SaveAsWebpAsync(outputStream, new WebpEncoder { Quality = quality }, cancellationToken);
            outputStream.Position = 0;

            return outputStream;
        }

        /// <summary>
        /// Validates whether the stream contains a valid image.
        /// </summary>
        /// <param name="stream">The stream to validate.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the stream is a valid image; otherwise, false.</returns>
        public async Task<bool> IsValidImageAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            try
            {
                stream.Position = 0;
                var info = await Image.IdentifyAsync(stream, cancellationToken);
                return info != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the dimensions of an image.
        /// </summary>
        /// <param name="stream">The image stream.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A tuple containing width and height, or null if the image is invalid.</returns>
        public async Task<(int Width, int Height)?> GetImageDimensionsAsync(
            Stream stream,
            CancellationToken cancellationToken = default)
        {
            try
            {
                stream.Position = 0;
                var info = await Image.IdentifyAsync(stream, cancellationToken);
                return info != null ? (info.Width, info.Height) : null;
            }
            catch
            {
                return null;
            }
        }
    }

}
