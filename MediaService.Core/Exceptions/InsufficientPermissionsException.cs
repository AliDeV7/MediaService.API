namespace MediaService.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a client attempts an operation without the required permission
    /// </summary>
    public sealed class InsufficientPermissionsException : MediaServiceException
    {
        /// <summary>
        /// The permission that was required but missing
        /// </summary>
        public string RequiredPermission { get; }

        /// <summary>
        /// Initializes a new instance of InsufficientPermissionsException
        /// </summary>
        /// <param name="permission">The permission that the client lacks</param>
        public InsufficientPermissionsException(string permission)
            : base($"Client lacks required permission: {permission}", "INSUFFICIENT_PERMISSIONS")
        {
            RequiredPermission = permission;
        }
    }
}
