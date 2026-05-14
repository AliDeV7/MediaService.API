namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration settings for JWT token generation and validation
    /// </summary>
    public sealed class JwtSettings
    {
        /// <summary>
        /// Configuration section name in appsettings.json
        /// </summary>
        public const string SectionName = "JwtSettings";

        /// <summary>
        /// Secret key used for signing JWT tokens
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer identifier
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience identifier
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration time in minutes
        /// </summary>
        public int ExpirationMinutes { get; set; } = 60;
    }
}
