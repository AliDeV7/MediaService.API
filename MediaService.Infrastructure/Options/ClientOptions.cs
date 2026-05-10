using MediaService.Core.Entities;

namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Configuration for registered clients
    /// </summary>
    public class ClientOptions
    {
        public const string SectionName = "Clients";

        /// <summary>
        /// List of registered client applications
        /// </summary>
        public List<Client> RegisteredClients { get; set; } = new();
    }
}
