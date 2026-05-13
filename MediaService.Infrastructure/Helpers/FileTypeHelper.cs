using MediaService.Core.Enums;
using MediaService.Core.Exceptions;

namespace MediaService.Infrastructure.Helpers
{
    /// <summary>
    /// Utility class for file type detection and validation
    /// Single source of truth for file extensions, MIME types, and magic bytes
    /// </summary>
    public static class FileTypeHelper
    {
        /// <summary>
        /// Maps file extensions to their corresponding MIME types
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string[]> ExtensionMimeMap = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            // Images
            [".jpg"] = new[] { "image/jpeg" },
            [".jpeg"] = new[] { "image/jpeg" },
            [".png"] = new[] { "image/png" },
            [".gif"] = new[] { "image/gif" },
            [".bmp"] = new[] { "image/bmp" },
            [".webp"] = new[] { "image/webp" },
            [".svg"] = new[] { "image/svg+xml" },
            [".ico"] = new[] { "image/x-icon", "image/vnd.microsoft.icon" },
            [".tiff"] = new[] { "image/tiff" },
            [".tif"] = new[] { "image/tiff" },

            // Videos
            [".mp4"] = new[] { "video/mp4" },
            [".avi"] = new[] { "video/x-msvideo" },
            [".mov"] = new[] { "video/quicktime" },
            [".wmv"] = new[] { "video/x-ms-wmv" },
            [".flv"] = new[] { "video/x-flv" },
            [".mkv"] = new[] { "video/x-matroska" },
            [".webm"] = new[] { "video/webm" },
            [".mpeg"] = new[] { "video/mpeg" },
            [".mpg"] = new[] { "video/mpeg" },

            // Documents
            [".pdf"] = new[] { "application/pdf" },
            [".doc"] = new[] { "application/msword" },
            [".docx"] = new[] { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            [".xls"] = new[] { "application/vnd.ms-excel" },
            [".xlsx"] = new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            [".ppt"] = new[] { "application/vnd.ms-powerpoint" },
            [".pptx"] = new[] { "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            [".txt"] = new[] { "text/plain" },
            [".csv"] = new[] { "text/csv" },

            // Audio
            [".mp3"] = new[] { "audio/mpeg" },
            [".wav"] = new[] { "audio/wav", "audio/wave" },
            [".ogg"] = new[] { "audio/ogg" },
            [".flac"] = new[] { "audio/flac" },
            [".aac"] = new[] { "audio/aac" },
            [".m4a"] = new[] { "audio/mp4" }
        };

        /// <summary>
        /// Maps file extensions to their file type category
        /// </summary>
        public static readonly IReadOnlyDictionary<string, FileType> ExtensionTypeMap = new Dictionary<string, FileType>(StringComparer.OrdinalIgnoreCase)
        {
            // Images
            [".jpg"] = FileType.Image,
            [".jpeg"] = FileType.Image,
            [".png"] = FileType.Image,
            [".gif"] = FileType.Image,
            [".bmp"] = FileType.Image,
            [".webp"] = FileType.Image,
            [".svg"] = FileType.Image,
            [".ico"] = FileType.Image,
            [".tiff"] = FileType.Image,
            [".tif"] = FileType.Image,

            // Videos
            [".mp4"] = FileType.Video,
            [".avi"] = FileType.Video,
            [".mov"] = FileType.Video,
            [".wmv"] = FileType.Video,
            [".flv"] = FileType.Video,
            [".mkv"] = FileType.Video,
            [".webm"] = FileType.Video,
            [".mpeg"] = FileType.Video,
            [".mpg"] = FileType.Video,

            // Documents
            [".pdf"] = FileType.Document,
            [".doc"] = FileType.Document,
            [".docx"] = FileType.Document,
            [".xls"] = FileType.Document,
            [".xlsx"] = FileType.Document,
            [".ppt"] = FileType.Document,
            [".pptx"] = FileType.Document,
            [".txt"] = FileType.Document,
            [".csv"] = FileType.Document,

            // Audio
            [".mp3"] = FileType.Audio,
            [".wav"] = FileType.Audio,
            [".ogg"] = FileType.Audio,
            [".flac"] = FileType.Audio,
            [".aac"] = FileType.Audio,
            [".m4a"] = FileType.Audio
        };

        /// <summary>
        /// Maps MIME types to their magic bytes (file signatures)
        /// </summary>
        public static readonly IReadOnlyDictionary<string, byte[][]> MagicBytesMap = new Dictionary<string, byte[][]>(StringComparer.OrdinalIgnoreCase)
        {
            // Images
            ["image/jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
            ["image/png"] = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
            ["image/gif"] = new[] { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 } },
            ["image/bmp"] = new[] { new byte[] { 0x42, 0x4D } },
            ["image/webp"] = new[] { new byte[] { 0x52, 0x49, 0x46, 0x46 } }, // RIFF header
            ["image/tiff"] = new[] { new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new byte[] { 0x4D, 0x4D, 0x00, 0x2A } },

            // Videos
            ["video/mp4"] = new[] { new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70 }, new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70 } },
            ["video/x-msvideo"] = new[] { new byte[] { 0x52, 0x49, 0x46, 0x46 } }, // RIFF header
            ["video/quicktime"] = new[] { new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70 } },
            ["video/x-matroska"] = new[] { new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } },
            ["video/webm"] = new[] { new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } },

            // Documents
            ["application/pdf"] = new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } }, // %PDF
            ["application/msword"] = new[] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } },
            ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"] = new[] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } }, // ZIP
            ["application/vnd.ms-excel"] = new[] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } },
            ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"] = new[] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } }, // ZIP

            // Audio
            ["audio/mpeg"] = new[] { new byte[] { 0xFF, 0xFB }, new byte[] { 0x49, 0x44, 0x33 } }, // MP3
            ["audio/wav"] = new[] { new byte[] { 0x52, 0x49, 0x46, 0x46 } }, // RIFF header
            ["audio/ogg"] = new[] { new byte[] { 0x4F, 0x67, 0x67, 0x53 } }, // OggS
            ["audio/flac"] = new[] { new byte[] { 0x66, 0x4C, 0x61, 0x43 } } // fLaC
        };

        /// <summary>
        /// Gets MIME type from file extension
        /// </summary>
        /// <param name="extension">File extension (with or without dot)</param>
        /// <returns>Primary MIME type for the extension</returns>
        /// <exception cref="ArgumentException">Thrown when extension is null or empty</exception>
        /// <exception cref="UnsupportedFileTypeException">Thrown when extension is not supported</exception>
        public static string GetMimeTypeFromExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException("Extension cannot be null or empty", nameof(extension));

            extension = extension.StartsWith(".") ? extension : $".{extension}";

            if (!ExtensionMimeMap.TryGetValue(extension, out var mimeTypes))
                throw new UnsupportedFileTypeException($"Unsupported file extension: {extension}");

            return mimeTypes[0];
        }

        /// <summary>
        /// Gets all allowed extensions for a specific file type
        /// </summary>
        /// <param name="fileType">Type of file (Image, Video, Document, Audio)</param>
        /// <returns>Array of allowed extensions including the dot</returns>
        public static string[] GetAllowedExtensionsForType(FileType fileType)
        {
            return ExtensionTypeMap
                .Where(kvp => kvp.Value == fileType)
                .Select(kvp => kvp.Key)
                .ToArray();
        }

        /// <summary>
        /// Gets all allowed MIME types for a specific file type
        /// </summary>
        /// <param name="fileType">Type of file (Image, Video, Document, Audio)</param>
        /// <returns>Array of allowed MIME types</returns>
        public static string[] GetAllowedMimeTypesForType(FileType fileType)
        {
            var extensions = GetAllowedExtensionsForType(fileType);
            return extensions
                .SelectMany(ext => ExtensionMimeMap.TryGetValue(ext, out var mimes) ? mimes : Array.Empty<string>())
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Gets file type category from extension
        /// </summary>
        /// <param name="extension">File extension (with or without dot)</param>
        /// <returns>File type category</returns>
        /// <exception cref="ArgumentException">Thrown when extension is null or empty</exception>
        /// <exception cref="UnsupportedFileTypeException">Thrown when extension is not supported</exception>
        public static FileType GetFileTypeFromExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException("Extension cannot be null or empty", nameof(extension));

            extension = extension.StartsWith(".") ? extension : $".{extension}";

            if (!ExtensionTypeMap.TryGetValue(extension, out var fileType))
                throw new UnsupportedFileTypeException($"Unsupported file extension: {extension}");

            return fileType;
        }

        /// <summary>
        /// Validates if MIME type matches the file extension
        /// </summary>
        /// <param name="extension">File extension (with or without dot)</param>
        /// <param name="mimeType">MIME type to validate</param>
        /// <returns>True if MIME type matches extension, false otherwise</returns>
        public static bool IsMimeTypeValid(string extension, string mimeType)
        {
            if (string.IsNullOrWhiteSpace(extension) || string.IsNullOrWhiteSpace(mimeType))
                return false;

            extension = extension.StartsWith(".") ? extension : $".{extension}";

            if (!ExtensionMimeMap.TryGetValue(extension, out var allowedMimeTypes))
                return false;

            return allowedMimeTypes.Any(m => m.Equals(mimeType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Validates file magic bytes against expected signature
        /// </summary>
        /// <param name="stream">File stream to validate</param>
        /// <param name="mimeType">Expected MIME type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if magic bytes match, false otherwise</returns>
        public static async Task<bool> MatchesMagicBytesAsync(Stream stream, string mimeType, CancellationToken cancellationToken = default)
        {
            if (stream == null || !stream.CanRead || !stream.CanSeek)
                return false;

            if (string.IsNullOrWhiteSpace(mimeType))
                return false;

            if (!MagicBytesMap.TryGetValue(mimeType, out var expectedSignatures))
                return true; // No magic bytes defined for this MIME type, skip validation

            var originalPosition = stream.Position;
            try
            {
                stream.Position = 0;
                var maxLength = expectedSignatures.Max(s => s.Length);
                var buffer = new byte[maxLength];
                var bytesRead = await stream.ReadAsync(buffer, 0, maxLength, cancellationToken);

                if (bytesRead < expectedSignatures.Min(s => s.Length))
                    return false;

                foreach (var signature in expectedSignatures)
                {
                    if (bytesRead >= signature.Length && buffer.Take(signature.Length).SequenceEqual(signature))
                        return true;
                }

                return false;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        /// <summary>
        /// Extracts file extension from filename
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns>File extension including the dot</returns>
        /// <exception cref="ArgumentException">Thrown when filename is null or empty</exception>
        public static string GetExtension(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException($"Filename '{fileName}' does not have an extension", nameof(fileName));

            return extension;
        }
    }
}
