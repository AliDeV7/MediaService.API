using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaService.Core.Entities
{
    /// <summary>
    /// Represents an authenticated client application
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Unique identifier for the client
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Display name of the client application
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Hashed client secret for authentication
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Roles assigned to this client
        /// </summary>
        public string[] Roles { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Indicates if the client is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
