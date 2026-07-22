namespace MediaService.Infrastructure.Options
{
    /// <summary>
    /// Runtime image processing configuration loaded from appsettings.
    /// </summary>
    public sealed class ImageProcessingOptions
    {
        /// <summary>Configuration section name.</summary>
        public const string SectionName = "ImageProcessing";

        /// <summary>Default thumbnail width in pixels.</summary>
        public int ThumbnailWidth { get; set; } = 400;

        /// <summary>Default WebP quality (1-100).</summary>
        public int WebPQuality { get; set; } = 82;

        /// <summary>Whether thumbnail generation is enabled by default.</summary>
        public bool GenerateThumbnail { get; set; } = false;

        /// <summary>Whether WebP conversion is enabled by default.</summary>
        public bool ConvertToWebP { get; set; } = true;
    }
}
