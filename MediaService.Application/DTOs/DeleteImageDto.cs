namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request data for deleting an image file.
    /// </summary>
    public sealed class DeleteImageDto
    {
        /// <summary>
        /// Relative path of the file to delete.
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;
    }
}
