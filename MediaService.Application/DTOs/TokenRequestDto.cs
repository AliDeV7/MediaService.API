namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Data transfer object for client authentication requests
    /// </summary>
    public sealed class TokenRequestDto
    {
        /// <summary>
        /// Unique identifier for the service client
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Secret key used to authenticate the client
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
