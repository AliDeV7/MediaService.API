namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when file type is not allowed
    /// </summary>
    public class UnsupportedFileTypeException : MediaServiceException
    {
        public UnsupportedFileTypeException(string message)
            : base(message, "UNSUPPORTED_FILE_TYPE") { }
    }
}
