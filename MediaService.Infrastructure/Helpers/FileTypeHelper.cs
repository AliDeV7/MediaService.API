using MediaService.Core.Enums;

namespace MediaService.Infrastructure.Helpers
{
    /// <summary>
    /// Provides static lookup maps and magic byte validation for file type detection
    /// </summary>
    public static class FileTypeHelper
    {
        /// <summary>
        /// Maps file extensions to their allowed MIME types
        /// </summary>
        public static readonly Dictionary<string, string[]> ExtensionMimeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".jpg",  ["image/jpeg"] },
        { ".jpeg", ["image/jpeg"] },
        { ".png",  ["image/png"] },
        { ".gif",  ["image/gif"] },
        { ".webp", ["image/webp"] },
        { ".mp4",  ["video/mp4"] },
        { ".mov",  ["video/quicktime"] },
        { ".avi",  ["video/x-msvideo"] },
        { ".mkv",  ["video/x-matroska"] },
        { ".mp3",  ["audio/mpeg"] },
        { ".wav",  ["audio/wav", "audio/x-wav"] },
        { ".ogg",  ["audio/ogg"] },
        { ".pdf",  ["application/pdf"] },
    };

        /// <summary>
        /// Maps file extensions to their corresponding FileType enum value
        /// </summary>
        public static readonly Dictionary<string, FileType> ExtensionTypeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".jpg",  FileType.Image },
        { ".jpeg", FileType.Image },
        { ".png",  FileType.Image },
        { ".gif",  FileType.Image },
        { ".webp", FileType.Image },
        { ".mp4",  FileType.Video },
        { ".mov",  FileType.Video },
        { ".avi",  FileType.Video },
        { ".mkv",  FileType.Video },
        { ".mp3",  FileType.Audio },
        { ".wav",  FileType.Audio },
        { ".ogg",  FileType.Audio },
        { ".pdf",  FileType.Document },
    };

        /// <summary>
        /// Known magic byte signatures keyed by MIME type.
        /// Each entry may have multiple valid signatures (e.g. different MP4 variants).
        /// </summary>
        private static readonly Dictionary<string, byte[][]> MagicBytes = new()
    {
        { "image/jpeg", [ [0xFF, 0xD8, 0xFF] ] },
        { "image/png",  [ [0x89, 0x50, 0x4E, 0x47] ] },
        { "image/gif",  [ [0x47, 0x49, 0x46, 0x38] ] },
        { "image/webp", [ [0x52, 0x49, 0x46, 0x46] ] },
        { "video/mp4",  [
            [0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70],
            [0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70]
        ]},
        { "audio/mpeg", [ [0xFF, 0xFB], [0xFF, 0xF3], [0xFF, 0xF2], [0x49, 0x44, 0x33] ] },
        { "audio/wav",  [ [0x52, 0x49, 0x46, 0x46] ] },
        { "audio/x-wav",[ [0x52, 0x49, 0x46, 0x46] ] },
        { "audio/ogg",  [ [0x4F, 0x67, 0x67, 0x53] ] },
        { "application/pdf", [ [0x25, 0x50, 0x44, 0x46] ] },
    };

        /// <summary>
        /// Reads the stream header and checks it against known magic byte signatures.
        /// Returns true if no signature is registered for the given MIME type (unknown = skip check).
        /// Stream position is restored after reading.
        /// </summary>
        /// <param name="stream">File stream to inspect</param>
        /// <param name="expectedMimeType">MIME type to validate against</param>
        /// <returns>True if content matches or no signature is registered</returns>
        public static bool MatchesMagicBytes(Stream stream, string expectedMimeType)
        {
            if (!MagicBytes.TryGetValue(expectedMimeType, out var signatures))
                return true;

            const int bufferSize = 8;
            var buffer = new byte[bufferSize];

            var originalPosition = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            var bytesRead = stream.Read(buffer, 0, bufferSize);
            stream.Seek(originalPosition, SeekOrigin.Begin);

            if (bytesRead < 2) return false;

            return signatures.Any(sig =>
                sig.Length <= bytesRead &&
                buffer.Take(sig.Length).SequenceEqual(sig));
        }
    }
}
