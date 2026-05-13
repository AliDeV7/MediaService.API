namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// View model for media file upload from HTTP multipart/form-data request.
    /// Contains framework-specific IFormFile type.
    /// </summary>
    public class ImageUploadFileRequest : ImageUploadRequestBase
    {
        /// <summary>
        /// File from multipart/form-data request.
        /// </summary>
        public IFormFile File { get; set; } = null!;
    }
}
