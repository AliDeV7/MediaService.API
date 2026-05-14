namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Data transfer object for authentication token responses
    /// </summary>
    public sealed class TokenResponseDto
    {
        /// <summary>
        /// JWT access token for authenticating subsequent requests
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token type, typically "Bearer"
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token expiration time in seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Timestamp when the token was issued (Unix epoch)
        /// </summary>
        public long IssuedAt { get; set; }
    }
}
