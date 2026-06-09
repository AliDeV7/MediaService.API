using MediaService.Core.Enums;
using MediaService.Core.Exceptions;
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
        /// Format: media/images/{year}/{month}/{filename}.webp
        /// </summary>
        /// <param name="baseName">Base filename without extension.</param>
        /// <param name="date">Date for directory structure.</param>
        /// <returns>Tuple containing main image path and thumbnail path.</returns>
        public static (string MainPath, string ThumbnailPath) BuildImagePaths(string baseName, DateTime date)
        {
            var directory = BuildMediaDirectory("images", date);
            var mainPath = $"{directory}/{baseName}.webp";
            var thumbnailPath = $"{directory}/{baseName}_thumb.webp";
            return (mainPath, thumbnailPath);
        }

        /// <summary>
        /// Builds the relative path for a non-image file based on its type.
        /// Format: media/{type}/{year}/{month}/{filename}{extension}
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
            var mediaType = DetermineMediaType(extension);
            var directory = BuildMediaDirectory(mediaType, date);
            var mainPath = $"{directory}/{baseName}{extension}";
            return (mainPath, null);
        }

        /// <summary>
        /// Builds the thumbnail relative path from the original image relative path.
        /// Returns null if the path is invalid or not an image file.
        /// </summary>
        /// <param name="relativePath">Original image relative path.</param>
        /// <returns>Thumbnail relative path or null.</returns>
        public static string? BuildThumbnailRelativePath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            var extension = Path.GetExtension(relativePath);

            if (string.IsNullOrEmpty(extension))
                return null;

            // Use FileTypeHelper as source of truth for file type detection
            try
            {
                var fileType = FileTypeHelper.GetFileTypeFromExtension(extension);

                // Only images can have thumbnails
                if (fileType != FileType.Image)
                    return null;
            }
            catch (UnsupportedFileTypeException)
            {
                // Not a supported file type, no thumbnail
                return null;
            }

            var directory = Path.GetDirectoryName(relativePath) ?? string.Empty;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
            var thumbnailFileName = $"{fileNameWithoutExtension}_thumb.webp";

            return Path.Combine(directory, thumbnailFileName).Replace("\\", "/");
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
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
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
        /// <param name="baseUrl">Base URL (e.g., "https://cdn.aurora.com" or empty for relative).</param>
        /// <param name="relativePath">Relative path to the file.</param>
        /// <returns>Complete public URL or relative path.</returns>
        public static string BuildPublicUrl(string baseUrl, string relativePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

            var normalizedPath = relativePath.Replace('\\', '/');

            // Ensure leading slash
            if (!normalizedPath.StartsWith('/'))
                normalizedPath = "/" + normalizedPath;

            // If baseUrl is empty, return app-relative public path
            if (string.IsNullOrWhiteSpace(baseUrl))
                return normalizedPath;

            // Combine baseUrl + normalized path
            return $"{baseUrl.TrimEnd('/')}{normalizedPath}";
        }

        /// <summary>
        /// Computes SHA256 hash of the stream.
        /// </summary>
        /// <param name="stream">The stream to hash.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Lowercase hexadecimal hash string.</returns>
        public static async Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using var sha256 = SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(stream, cancellationToken);

            // Reset stream position after hashing
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        /// <summary>
        /// Determines the file extension based on conversion settings.
        /// </summary>
        /// <param name="fileName">Original file name.</param>
        /// <param name="convertToWebP">Whether to convert to WebP format.</param>
        /// <returns>File extension with leading dot.</returns>
        public static string DetermineFileExtension(string fileName, bool convertToWebP)
        {
            if (convertToWebP)
            {
                return ".webp";
            }

            var extension = Path.GetExtension(fileName);
            return string.IsNullOrWhiteSpace(extension) ? ".jpg" : extension;
        }

        /// <summary>
        /// Converts Base64 string to a memory stream.
        /// </summary>
        /// <param name="base64Data">Base64 string (with or without data URI prefix).</param>
        /// <returns>Memory stream containing the decoded bytes.</returns>
        public static MemoryStream ConvertBase64ToStream(string base64Data)
        {
            // Remove data URI prefix if present (e.g., "data:image/png;base64,")
            var base64String = base64Data.Contains(',')
                ? base64Data.Split(',')[1]
                : base64Data;

            var bytes = Convert.FromBase64String(base64String);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Generates save paths for main file and optional thumbnail.
        /// </summary>
        /// <param name="extension">File extension with leading dot.</param>
        /// <param name="generateThumbnail">Whether to generate thumbnail path.</param>
        /// <returns>Tuple of main path and optional thumbnail path.</returns>
        public static (string MainPath, string? ThumbnailPath) GenerateSavePathsForImage(
            string extension,
            bool generateThumbnail)
        {
            var baseName = GenerateUniqueFileName();
            var date = DateTime.UtcNow;

            // Ensure extension has leading dot
            var cleanExtension = extension.StartsWith(".") ? extension : $".{extension}";

            // Use BuildImagePaths for WebP
            if (cleanExtension.Equals(".webp", StringComparison.OrdinalIgnoreCase))
            {
                var (mainPath, thumbnailPath) = BuildImagePaths(baseName, date);
                return (mainPath, generateThumbnail ? thumbnailPath : null);
            }

            // Use BuildFilePath for other formats
            var (filePath, _) = BuildFilePath(baseName, cleanExtension, date);

            // Generate thumbnail path with SAME base name if needed
            string? thumbPath = null;
            if (generateThumbnail)
            {
                var directory = BuildMediaDirectory("images", date);
                thumbPath = $"{directory}/{baseName}_thumb{cleanExtension}";
            }

            return (filePath, thumbPath);
        }



        // ── Private helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Builds the media directory path based on media type and date.
        /// Format: media/{type}/{year}/{month}
        /// </summary>
        /// <param name="mediaType">Type of media (images, videos, documents, audio).</param>
        /// <param name="date">Date for directory structure.</param>
        /// <returns>Directory path string.</returns>
        private static string BuildMediaDirectory(string mediaType, DateTime date)
        {
            return $"media/{mediaType}/{date.Year}/{date.Month:D2}";
        }

        /// <summary>
        /// Determines the media type category based on file extension.
        /// </summary>
        /// <param name="extension">File extension including the dot (e.g., ".pdf").</param>
        /// <returns>Media type category string.</returns>
        private static string DetermineMediaType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".bmp" or ".svg" => "images",
                ".mp4" or ".avi" or ".mov" or ".wmv" or ".flv" or ".mkv" => "videos",
                ".mp3" or ".wav" or ".ogg" or ".flac" or ".aac" => "audio",
                ".pdf" or ".doc" or ".docx" or ".txt" or ".xls" or ".xlsx" or ".ppt" or ".pptx" => "documents",
                _ => "others"
            };
        }
    }

}
