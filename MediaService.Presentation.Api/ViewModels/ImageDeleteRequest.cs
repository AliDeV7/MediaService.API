namespace MediaService.Presentation.Api.ViewModels
{
    /// <summary>
    /// Request model for deleting an image file.
    /// </summary>
    public sealed class ImageDeleteRequest
    {
        /// <summary>
        /// Relative path of the file to delete.
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;
    }
}
