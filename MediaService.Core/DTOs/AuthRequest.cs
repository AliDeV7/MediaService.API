namespace MediaService.Core.DTOs
{
    /// <summary>
    /// Request model for service-to-service authentication
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Client identifier (e.g., "admin-service", "shop-service")
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Client secret for authentication
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
