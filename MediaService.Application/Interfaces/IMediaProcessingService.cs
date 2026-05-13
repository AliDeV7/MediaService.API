using MediaService.Application.DTOs;

namespace MediaService.Application.Interfaces
{
    /// <summary>
    /// Service for processing raw media uploads into storage-ready DTOs.
    /// Handles validation, transformation, and metadata extraction.
    /// </summary>
    public interface IMediaProcessingService
    {
        /// <summary>
        /// Processes a raw image stream and prepares it for storage.
        /// </summary>
        /// <param name="fileStream">Input file stream</param>
        /// <param name="fileName">Original file name</param>
        /// <param name="options">Processing options (thumbnail, WebP conversion, etc.)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>UploadMediaDto ready for storage</returns>
        Task<UploadMediaDto> ProcessImageFromStreamAsync(
            Stream fileStream,
            string fileName,
            ImageProcessingDto options,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes a Base64-encoded image and prepares it for storage.
        /// </summary>
        /// <param name="base64Data">Base64-encoded image data</param>
        /// <param name="fileName">Original file name</param>
        /// <param name="options">Processing options (thumbnail, WebP conversion, etc.)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>UploadMediaDto ready for storage</returns>
        Task<UploadMediaDto> ProcessImageFromBase64Async(
            string base64Data,
            string? fileName,
            ImageProcessingDto options,
            CancellationToken cancellationToken = default);
    }
}
