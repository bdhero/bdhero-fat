namespace DotNetUtils.Net
{
    /// <summary>
    /// Represents the current state of a <see cref="FileDownloader"/>.
    /// </summary>
    public enum FileDownloadState
    {
        /// <summary>
        /// Download has not started yet.
        /// </summary>
        Ready,

        /// <summary>
        /// Download is in progress.
        /// </summary>
        Downloading,

        /// <summary>
        /// Download is in progress, but is currently paused.
        /// </summary>
        Paused,

        /// <summary>
        /// An exception occurred while downloading the file.
        /// </summary>
        Error,

        /// <summary>
        /// The download completed successfully.
        /// </summary>
        Success
    }
}