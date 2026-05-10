namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when authentication fails
    /// </summary>
    public class UnauthorizedException : MediaServiceException
    {
        public UnauthorizedException(string message)
            : base(message, "UNAUTHORIZED") { }
    }
}
