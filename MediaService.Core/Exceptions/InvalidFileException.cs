namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when file validation fails
    /// </summary>
    public class InvalidFileException : MediaServiceException
    {
        public InvalidFileException(string message)
            : base(message, "INVALID_FILE") { }
    }
}
