namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// Request model for uploading an image from Base64-encoded string.
    /// </summary>
    public class ImageUploadBase64Request : ImageUploadRequestBase
    {
        /// <summary>
        /// Base64-encoded image data.
        /// Can include data URI prefix (e.g., "data:image/png;base64,iVBORw0KG...")
        /// or just the base64 string.
        /// </summary>
        public string Base64Data { get; set; } = null!;

        /// <summary>
        /// File name with extension (e.g., "photo.jpg").
        /// Required to determine file type.
        /// </summary>
        public string? FileName { get; set; } = null!;
    }

}
