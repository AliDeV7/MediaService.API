namespace MediaService.Core.Interfaces
{
    /// <summary>
    /// Interface for image processing operations
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Generates thumbnail for image
        /// </summary>
        /// <param name="imageStream">Original image stream</param>
        /// <param name="width">Thumbnail width in pixels</param>
        /// <returns>Thumbnail as byte array</returns>
        Task<byte[]> GenerateThumbnailAsync(Stream imageStream, int width = 400);

        /// <summary>
        /// Converts image to WebP format
        /// </summary>
        /// <param name="imageStream">Original image stream</param>
        /// <param name="quality">WebP quality (1-100)</param>
        /// <returns>WebP image as byte array</returns>
        Task<byte[]> ConvertToWebPAsync(Stream imageStream, int quality = 80);

        /// <summary>
        /// Checks if file is a valid image
        /// </summary>
        /// <param name="imageStream">File stream to check</param>
        /// <returns>True if valid image, false otherwise</returns>
        Task<bool> IsValidImageAsync(Stream imageStream);

        /// <summary>
        /// Gets image dimensions
        /// </summary>
        /// <param name="imageStream">Image stream</param>
        /// <returns>Tuple of (width, height)</returns>
        Task<(int width, int height)> GetImageDimensionsAsync(Stream imageStream);
    }

}
