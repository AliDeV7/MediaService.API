namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when file size exceeds limit
    /// </summary>
    public class FileSizeLimitException : MediaServiceException
    {
        public FileSizeLimitException(string message)
            : base(message, "FILE_TOO_LARGE") { }
    }

}
