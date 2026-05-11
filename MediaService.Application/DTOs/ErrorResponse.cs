namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Standard error response model
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Error code for client handling
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of error
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
