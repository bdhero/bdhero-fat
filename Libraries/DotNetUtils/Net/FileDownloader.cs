using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetUtils.Net
{
    /// <summary>
    /// Asynchronous HTTP file downloader that runs in a separate thread and reports its progress to observers.
    /// </summary>
    public class FileDownloader
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// URI of the remote Web resource to download.
        /// </summary>
        public string Uri;

        /// <summary>
        /// Path to where the download will be saved on disk.
        /// </summary>
        public string Path;

        /// <summary>
        /// Execution context (a.k.a. thread) to invoke <see cref="StateChanged"/> notifications on.
        /// Defaults to the current thread's context if none is specified.
        /// </summary>
        public TaskScheduler CallbackThread;

        /// <summary>
        /// Invoked on the <c>TaskScheduler</c> specified by <see cref="CallbackThread"/> whenever the state or progress of the download changes.
        /// </summary>
        public event FileDownloadStateChangedHandler StateChanged;

        /// <summary>
        /// Retrieves the value of the HTTP <c>Content-Length</c> response header
        /// by performing an HTTP <c>HEAD</c> request for <see cref="Uri"/>.
        /// </summary>
        /// <param name="proxy">Optional proxy to send requests through.  If none is specified, the default system proxy settings will be used.</param>
        /// <returns>Size of the download in bytes</returns>
        public long GetContentLength(WebProxy proxy = null)
        {
            var request = HttpRequest.BuildRequest("HEAD", Uri);

            if (proxy != null)
                request.Proxy = proxy;

            using (var response = request.GetResponse())
            {
                return response.ContentLength;
            }
        }

        /// <summary>
        /// Streams the remote resource to the local file.
        /// </summary>
        /// <param name="proxy">Optional proxy to send requests through.  If none is specified, the default system proxy settings will be used.</param>
        public void DownloadSync(WebProxy proxy = null)
        {
            var request = HttpRequest.BuildRequest("GET", Uri);

            if (proxy != null)
                request.Proxy = proxy;

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var fileStream = File.Open(Path, FileMode.Create))
            {
                // Default buffer size in .NET is 8 KiB.
                // http://stackoverflow.com/a/1863003/467582
                const int bufferSize = 1024 * 8;

                var buffer = new byte[bufferSize];
                int bytesRead;
                var fileSize = 0;

                Tick(fileSize, force: true);

                do
                {
                    Tick(fileSize);

                    bytesRead = responseStream.Read(buffer, 0, bufferSize);
                    fileSize += bytesRead;

                    fileStream.Write(buffer, 0, bytesRead);

                    if (bytesRead > 0)
                        Notify(fileSize, response.ContentLength);
                } while (bytesRead > 0);

                fileStream.Close();

                if (fileSize != response.ContentLength)
                {
                    throw new IOException("Number of bytes received (" + fileSize + ") does not match Content-Length (" + response.ContentLength + ")");
                }

                var length = new FileInfo(Path).Length;
                if (length != response.ContentLength)
                {
                    throw new IOException("Number of bytes written to disk (" + length + ") does not match Content-Length (" + response.ContentLength + ")");
                }
            }
        }

        /// <summary>
        /// Streams the remote resource to the local file.
        /// </summary>
        /// <param name="proxy">Optional proxy to send requests through.  If none is specified, the default system proxy settings will be used.</param>
        public Task DownloadAsync(WebProxy proxy = null)
        {
            return Task.Factory.StartNew(() => DownloadSync(proxy));
        }

        private bool HasEnoughTimeElapsed { get { return (DateTime.Now - _lastTick).TotalMilliseconds > 100; } }

        private void Tick(int fileSize, bool force = false)
        {
            if (!HasEnoughTimeElapsed && !force) return;
            _lastTick = DateTime.Now;
            _lastFileSize = fileSize;
        }

        private DateTime _lastTick;
        private int _lastFileSize;

        private void Notify(int fileSize, long contentLength)
        {
            var @continue = HasEnoughTimeElapsed || _lastFileSize == 0 || fileSize >= contentLength;
            if (!@continue) return;

            var fileSizeDelta = (fileSize - _lastFileSize);
            var timeSpan = (DateTime.Now - _lastTick);
            var bytesPerSecond = fileSizeDelta / timeSpan.TotalSeconds;

            if (timeSpan.TotalSeconds == 0)
            {
                Logger.Error("Not enough time between notifications to generate meaningful data");
                return;
            }

            var state = new FileDownloadState(fileSize, contentLength, bytesPerSecond * 8);

            Console.WriteLine(state);

            if (StateChanged != null)
            {
                var thread = CallbackThread
                                ?? (SynchronizationContext.Current != null
                                  ? TaskScheduler.FromCurrentSynchronizationContext()
                                  : TaskScheduler.Default);
                Task.Factory.StartNew(() => StateChanged(state), CancellationToken.None, TaskCreationOptions.None, thread);
                StateChanged(state);
            }
        }
    }
}
