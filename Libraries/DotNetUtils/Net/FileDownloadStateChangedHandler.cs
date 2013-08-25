namespace DotNetUtils.Net
{
    /// <summary>
    /// Invoked whenever the state or progress of a download changes.
    /// </summary>
    /// <param name="fileDownloadState"></param>
    public delegate void FileDownloadStateChangedHandler(FileDownloadState fileDownloadState);
}