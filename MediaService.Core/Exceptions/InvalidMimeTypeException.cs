namespace MediaService.Core.Exceptions
{

    /// <summary>
    /// Exception thrown when MIME type doesn't match file extension
    /// </summary>
    public class InvalidMimeTypeException : Exception
    {
        public InvalidMimeTypeException(string message) : base(message) { }
    }
}
