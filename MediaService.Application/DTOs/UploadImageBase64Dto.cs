namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request model for base64 file upload (JSON payload)
    /// Use this when client cannot send multipart/form-data (e.g., some AJAX scenarios)
    /// Note: Base64 encoding increases payload size by ~33%, use FileUploadRequest when possible
    /// </summary>
    public class UploadImageBase64Dto : UploadImageBaseDto
    {
        /// <summary>
        /// Base64-encoded file data
        /// Can include data URI prefix (e.g., "data:image/png;base64,iVBORw0KG...")
        /// or just the base64 string
        /// </summary>
        public string Base64Data { get; set; } = string.Empty;
    }
}
