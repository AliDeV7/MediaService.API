namespace MediaService.Core.Configuration
{
    /// <summary>
    /// Default validation settings for document files.
    /// </summary>
    public static class DocumentValidationDefaults
    {
        /// <summary>
        /// Maximum file size for documents in bytes (50 MB).
        /// </summary>
        public const long MaxFileSizeBytes = 50 * 1024 * 1024;

        /// <summary>
        /// Allowed document extensions.
        /// </summary>
        public static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" };
    }
}
