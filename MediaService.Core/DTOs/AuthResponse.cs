namespace MediaService.Core.DTOs
{
    /// <summary>
    /// Response model containing authentication token
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// JWT access token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token type (always "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token expiration timestamp (Unix epoch)
        /// </summary>
        public long ExpiresAt { get; set; }

        /// <summary>
        /// Client name for display purposes
        /// </summary>
        public string ClientName { get; set; } = string.Empty;
    }
}
