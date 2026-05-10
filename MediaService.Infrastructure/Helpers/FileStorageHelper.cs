using System.Security.Cryptography;

namespace MediaService.Infrastructure.Helpers
{
    /// <summary>
    /// Helper class for file storage operations including path generation,
    /// file I/O, and directory management.
    /// </summary>
    public static class FileStorageHelper
    {
        /// <summary>
        /// Generates a unique filename using MD5 hash of a new GUID.
        /// Example output: "a3f2c1d2e5b6f7a8"
        /// </summary>
        public static string GenerateUniqueFileName()
        {
            var guidBytes = Guid.NewGuid().ToByteArray();
            var hash = MD5.HashData(guidBytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        /// <summary>
        /// Builds relative paths for the main WebP image and its thumbnail.
        /// Format: media/{year}/{month}/{filename}.webp
        /// </summary>
        /// <param name="baseName">Base filename without extension.</param>
        /// <param name="date">Date for directory structure.</param>
        /// <returns>Tuple containing main image path and thumbnail path.</returns>
        public static (string MainPath, string ThumbnailPath) BuildImagePaths(string baseName, DateTime date)
        {
            var directory = BuildMediaDirectory(date);
            var mainPath = $"{directory}/{baseName}.webp";
            var thumbnailPath = $"{directory}/{baseName}_thumb.webp";
            return (mainPath, thumbnailPath);
        }

        /// <summary>
        /// Builds the relative path for a non-image file.
        /// Format: media/{year}/{month}/{filename}{extension}
        /// </summary>
        /// <param name="baseName">Base filename without extension.</param>
        /// <param name="extension">File extension including the dot (e.g., ".pdf").</param>
        /// <param name="date">Date for directory structure.</param>
        /// <returns>Tuple containing main file path and null for thumbnail.</returns>
        public static (string MainPath, string? ThumbnailPath) BuildFilePath(
            string baseName,
            string extension,
            DateTime date)
        {
            var directory = BuildMediaDirectory(date);
            var mainPath = $"{directory}/{baseName}{extension}";
            return (mainPath, null);
        }

        /// <summary>
        /// Derives the thumbnail relative path from a main file's relative path.
        /// Returns null if the path does not follow the expected naming convention.
        /// </summary>
        /// <param name="relativePath">Main file's relative path.</param>
        /// <returns>Thumbnail path or null if not applicable.</returns>
        public static string? BuildThumbnailRelativePath(string relativePath)
        {
            if (!relativePath.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
                return null;

            var directory = Path.GetDirectoryName(relativePath)?.Replace('\\', '/');
            var baseName = Path.GetFileNameWithoutExtension(relativePath);

            return directory is not null ? $"{directory}/{baseName}_thumb.webp" : null;
        }

        /// <summary>
        /// Converts a relative path to an absolute disk path.
        /// </summary>
        /// <param name="storageRootPath">Root storage directory path.</param>
        /// <param name="relativePath">Relative path within storage.</param>
        /// <returns>Absolute file system path.</returns>
        public static string ToAbsolutePath(string storageRootPath, string relativePath)
        {
            return Path.Combine(
                storageRootPath,
                relativePath.Replace('/', Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// Ensures the directory for the given file path exists.
        /// Creates all necessary parent directories if they don't exist.
        /// </summary>
        /// <param name="absoluteFilePath">Absolute path to a file.</param>
        public static void EnsureDirectoryExists(string absoluteFilePath)
        {
            var directory = Path.GetDirectoryName(absoluteFilePath);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Writes a stream to disk at the specified absolute path asynchronously.
        /// Uses optimized buffer size for performance.
        /// </summary>
        /// <param name="absolutePath">Absolute path where file will be written.</param>
        /// <param name="sourceStream">Source stream to write.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of bytes written.</returns>
        public static async Task<long> WriteStreamToFileAsync(
            string absolutePath,
            Stream sourceStream,
            CancellationToken cancellationToken = default)
        {
            const int bufferSize = 81920; // 80 KB buffer for optimal performance

            await using var fileStream = new FileStream(
                absolutePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize,
                useAsync: true);

            await sourceStream.CopyToAsync(fileStream, bufferSize, cancellationToken);
            return fileStream.Length;
        }

        /// <summary>
        /// Opens a file stream for reading with optimized settings.
        /// Returns null if the file doesn't exist.
        /// </summary>
        /// <param name="absolutePath">Absolute path to the file.</param>
        /// <returns>FileStream for reading, or null if file not found.</returns>
        public static Stream? OpenReadStream(string absolutePath)
        {
            if (!File.Exists(absolutePath))
                return null;

            const int bufferSize = 81920; // 80 KB buffer

            return new FileStream(
                absolutePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize,
                useAsync: true);
        }

        /// <summary>
        /// Deletes a file if it exists; silently ignores missing files.
        /// </summary>
        /// <param name="absolutePath">Absolute path to the file.</param>
        /// <returns>True if the file was deleted, false if it didn't exist.</returns>
        public static bool DeleteFileIfExists(string absolutePath)
        {
            if (!File.Exists(absolutePath))
                return false;

            try
            {
                File.Delete(absolutePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a file exists at the given absolute path.
        /// </summary>
        /// <param name="absolutePath">Absolute path to check.</param>
        /// <returns>True if file exists, false otherwise.</returns>
        public static bool FileExists(string absolutePath)
        {
            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Constructs a public URL from a relative path and base URL.
        /// </summary>
        /// <param name="baseUrl">Base URL (e.g., "https://yourdomain.com/uploads").</param>
        /// <param name="relativePath">Relative path to the file.</param>
        /// <returns>Complete public URL.</returns>
        public static string BuildPublicUrl(string baseUrl, string relativePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

            return $"{baseUrl.TrimEnd('/')}/{relativePath.Replace('\\', '/')}";
        }

        // ── Private helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Builds the media directory path based on date.
        /// Format: media/{year}/{month}
        /// </summary>
        private static string BuildMediaDirectory(DateTime date)
        {
            return $"media/{date.Year}/{date.Month:D2}";
        }
    }
}
