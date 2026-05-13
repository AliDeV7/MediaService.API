namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a file deletion operation fails.
    /// </summary>
    public sealed class FileDeletionFailedException : MediaServiceException
    {
        public FileDeletionFailedException(string relativePath)
            : base("FILE_DELETION_FAILED", $"Failed to delete file: {relativePath}")
        {
            RelativePath = relativePath;
        }

        public string RelativePath { get; }
    }
}
