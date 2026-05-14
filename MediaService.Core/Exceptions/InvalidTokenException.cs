namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when JWT token validation fails
    /// </summary>
    public sealed class InvalidTokenException : MediaServiceException
    {
        /// <summary>
        /// Initializes a new instance of InvalidTokenException with a specific error message
        /// </summary>
        /// <param name="message">Description of the token validation failure</param>
        public InvalidTokenException(string message)
            : base(message, "INVALID_TOKEN") { }

        /// <summary>
        /// Initializes a new instance of InvalidTokenException with a custom error code
        /// </summary>
        /// <param name="message">Description of the token validation failure</param>
        /// <param name="errorCode">Specific error code for the token issue</param>
        public InvalidTokenException(string message, string errorCode)
            : base(message, errorCode) { }
    }
}
