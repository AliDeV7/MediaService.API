using MediaService.Core.DTOs;
using MediaService.Core.Entities;

namespace MediaService.Core.Interfaces
{
    // <summary>
    /// Interface for file storage operations
    /// Supports both IFormFile (efficient) and Base64 (compatibility) inputs
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Saves file from IFormFile (efficient, recommended method)
        /// Used when receiving multipart/form-data uploads
        /// </summary>
        /// <param name="request">File upload request containing file and metadata</param>
        /// <returns>MediaFile entity with file metadata</returns>
        Task<MediaFile> SaveFileAsync(FileUploadRequest request);

        /// <summary>
        /// Saves file from base64 string (for backward compatibility)
        /// Used when client sends base64-encoded data in JSON
        /// </summary>
        /// <param name="request">Base64 upload request containing encoded data and metadata</param>
        /// <returns>MediaFile entity with file metadata</returns>
        Task<MediaFile> SaveFileAsync(Base64UploadRequest request);

        /// <summary>
        /// Retrieves file content as byte array
        /// </summary>
        /// <param name="fileName">Name of the file to retrieve</param>
        /// <returns>File content as byte array, or null if not found</returns>
        Task<byte[]?> GetFileAsync(string fileName);

        /// <summary>
        /// Deletes a file from storage
        /// </summary>
        /// <param name="fileName">Name of the file to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        Task<bool> DeleteFileAsync(string fileName);

        /// <summary>
        /// Checks if a file exists in storage
        /// </summary>
        /// <param name="fileName">Name of the file to check</param>
        /// <returns>True if file exists, false otherwise</returns>
        Task<bool> FileExistsAsync(string fileName);
    }
}
