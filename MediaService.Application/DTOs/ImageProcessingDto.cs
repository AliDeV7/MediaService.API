using MediaService.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaService.Application.DTOs
{
    /// <summary>
    /// Request options for image processing operations.
    /// </summary>
    public sealed class ImageProcessingDto
    {
        /// <summary>
        /// Whether to generate a thumbnail.
        /// </summary>
        public bool GenerateThumbnail { get; set; } = ImageProcessingDefaults.GenerateThumbnail;

        /// <summary>
        /// Thumbnail width in pixels.
        /// </summary>
        public int ThumbnailWidth { get; set; } = ImageProcessingDefaults.ThumbnailWidth;

        /// <summary>
        /// Whether to convert the image to WebP format.
        /// </summary>
        public bool ConvertToWebP { get; set; } = ImageProcessingDefaults.ConvertToWebP;

        /// <summary>
        /// WebP quality (1-100).
        /// </summary>
        public int WebPQuality { get; set; } = ImageProcessingDefaults.WebPQuality;
    }
}
