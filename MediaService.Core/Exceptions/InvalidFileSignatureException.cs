namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when file signature (magic bytes) is invalid
    /// </summary>
    public class InvalidFileSignatureException : Exception
    {
        public InvalidFileSignatureException(string message) : base(message) { }
    }
}
