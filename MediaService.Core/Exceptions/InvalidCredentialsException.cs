namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when client authentication fails due to invalid credentials
    /// </summary>
    public sealed class InvalidCredentialsException : MediaServiceException
    {
        /// <summary>
        /// Initializes a new instance of InvalidCredentialsException with a default message
        /// </summary>
        public InvalidCredentialsException()
            : base("Invalid client credentials.", "INVALID_CREDENTIALS") { }

        /// <summary>
        /// Initializes a new instance of InvalidCredentialsException with a custom message
        /// </summary>
        /// <param name="message">Custom error message</param>
        public InvalidCredentialsException(string message)
            : base(message, "INVALID_CREDENTIALS") { }
    }
}
