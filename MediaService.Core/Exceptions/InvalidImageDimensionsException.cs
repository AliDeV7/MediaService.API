namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when image dimensions are invalid
    /// </summary>
    public class InvalidImageDimensionsException : Exception
    {
        public InvalidImageDimensionsException(string message)
            : base(message)
        {
        }

        public InvalidImageDimensionsException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
