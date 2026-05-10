namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Strongly-typed options for JWT token generation and validation.
    /// Bound from appsettings.json section "Jwt".
    /// </summary>
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        /// <summary>Signing secret key (min 32 chars recommended).</summary>
        public string SecretKey { get; init; } = string.Empty;

        /// <summary>Token issuer identifier.</summary>
        public string Issuer { get; init; } = string.Empty;

        /// <summary>Intended audience for the token.</summary>
        public string Audience { get; init; } = string.Empty;

        /// <summary>Token expiry duration in minutes.</summary>
        public int ExpiryMinutes { get; init; } = 60;
    }
}
