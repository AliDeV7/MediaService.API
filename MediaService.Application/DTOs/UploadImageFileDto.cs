namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request model for file upload using stream (multipart/form-data).
    /// This is the RECOMMENDED and most efficient method for file uploads.
    /// Default values are applied during mapping from ViewModel.
    /// </summary>
    public class UploadImageFileDto : UploadImageBaseDto
    {
        /// <summary>
        /// File content as stream
        /// </summary>
        public Stream FileStream { get; set; } = null!;
    }

}
