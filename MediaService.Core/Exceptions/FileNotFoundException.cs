namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when file is not found
    /// </summary>
    public class FileNotFoundException : MediaServiceException
    {
        public FileNotFoundException(string message)
            : base(message, "FILE_NOT_FOUND") { }
    }
}
