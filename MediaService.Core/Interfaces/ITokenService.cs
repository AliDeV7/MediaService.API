namespace MediaService.Core.Interfaces
{
    /// <summary>
    /// Interface for JWT token operations
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates JWT token for authenticated client
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="roles">Client roles</param>
        /// <returns>JWT token string</returns>
        string GenerateToken(string clientId, string[] roles);

        /// <summary>
        /// Validates client credentials against configuration
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="clientSecret">Client secret</param>
        /// <returns>True if valid, false otherwise</returns>
        bool ValidateCredentials(string clientId, string clientSecret);
    }
}
