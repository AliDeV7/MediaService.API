namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Custom exception for media service operations
    /// </summary>
    public class MediaServiceException : Exception
    {
        public string ErrorCode { get; set; }

        public MediaServiceException(string message, string errorCode = "MEDIA_ERROR")
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public MediaServiceException(string message, Exception innerException, string errorCode = "MEDIA_ERROR")
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
