namespace MediaService.Application.Interfaces.Validation
{
    /// <summary>
    /// Validator interface for image files
    /// </summary>
    public interface IImageValidator : IMediaValidator
    {
        /// <summary>
        /// Validates image dimensions (width and height)
        /// </summary>
        /// <param name="stream">Image stream to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="InvalidImageDimensionsException">Thrown when dimensions exceed limits</exception>
        Task ValidateImageDimensionsAsync(Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs complete validation of an image file (size, extension, MIME type, magic bytes, dimensions)
        /// </summary>
        /// <param name="stream">Image stream to validate</param>
        /// <param name="fileName">Name of the file including extension</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ValidateCompleteAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
    }
}
