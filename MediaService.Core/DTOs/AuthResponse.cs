namespace MediaService.Core.DTOs
{
    /// <summary>
    /// Response model for successful authentication
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// JWT access token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Token type (always "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";
    }
}
