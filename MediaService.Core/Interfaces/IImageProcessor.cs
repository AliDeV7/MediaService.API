namespace MediaService.Core.Interfaces
{
    /// <summary>
    /// Provides image processing operations including thumbnail generation, format conversion, validation, and dimension extraction.
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Generates a thumbnail by resizing the image to fit within the specified maximum dimensions while maintaining aspect ratio.
        /// </summary>
        /// <param name="inputStream">The input image stream (position will be reset).</param>
        /// <param name="maxSize">Maximum width or height in pixels (default: 400).</param>
        /// <param name="quality">WebP quality (1-100, default: 100).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A stream containing the WebP-encoded thumbnail. Caller must dispose.</returns>
        Task<Stream> GenerateThumbnailAsync(
            Stream inputStream,
            int maxSize = 400,
            int quality = 100,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Converts an image to WebP format.
        /// </summary>
        /// <param name="inputStream">The input image stream (position will be reset).</param>
        /// <param name="quality">WebP quality (1-100, default: 80).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A stream containing the WebP-encoded image. Caller must dispose.</returns>
        Task<Stream> ConvertToWebPAsync(
            Stream inputStream,
            int quality = 80,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates whether the stream contains a valid image.
        /// </summary>
        /// <param name="stream">The stream to validate (position will be reset).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the stream is a valid image; otherwise, false.</returns>
        Task<bool> IsValidImageAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the dimensions of an image without loading the full image into memory.
        /// </summary>
        /// <param name="stream">The image stream (position will be reset).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A tuple containing width and height, or null if the image is invalid.</returns>
        Task<(int Width, int Height)?> GetImageDimensionsAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
