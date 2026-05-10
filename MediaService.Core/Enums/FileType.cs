namespace MediaService.Core.Enums
{
    /// <summary>
    /// Represents the type of media file
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Image files (jpg, png, gif, webp, etc.)
        /// </summary>
        Image = 1,

        /// <summary>
        /// Video files (mp4, avi, mov, etc.)
        /// </summary>
        Video = 2,

        /// <summary>
        /// Audio files (mp3, wav, ogg, etc.)
        /// </summary>
        Audio = 3,

        /// <summary>
        /// Document files (pdf, doc, txt, etc.)
        /// </summary>
        Document = 4,

        /// <summary>
        /// Other file types
        /// </summary>
        Other = 5
    }
}
