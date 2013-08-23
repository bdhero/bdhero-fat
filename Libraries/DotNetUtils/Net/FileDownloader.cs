using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DotNetUtils.Net
{
    /// <summary>
    /// Asynchronous HTTP file downloader that runs in a separate thread and reports its progress to observers.
    /// </summary>
    public class FileDownloader
    {
        /// <summary>
        /// URI of the remote Web resource to download.
        /// </summary>
        public string Uri;

        /// <summary>
        /// Path to where the download will be saved on disk.
        /// </summary>
        public string Path;

        public event DownloadStateChangedHandler StateChanged;

        /// <summary>
        /// Streams the remote resource to the local file.
        /// </summary>
        /// <param name="proxy">Optional proxy to send requests through.  If none is specified, the default system proxy settings will be used.</param>
        public void DownloadSync(WebProxy proxy = null)
        {
            var request = HttpRequest.BuildRequest("GET", Uri);

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream()) // This actually sends the request
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
                    throw new IOException("Number of bytes received does not match expected Content-Length");
                }
                if (new FileInfo(Path).Length != response.ContentLength)
                {
                    throw new IOException("Number of bytes written to disk does not match Content-Length");
                }
            }
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
                Console.WriteLine("OOPS");
                return;
            }

            var state = new DownloadState(fileSize, contentLength, bytesPerSecond * 8);

            if (StateChanged != null)
                StateChanged(state);

            Console.WriteLine(state);
        }
    }

    public class DownloadState
    {
        /// <summary>
        /// Number of bytes downloaded so far.
        /// </summary>
        public readonly int BytesDownloaded;

        /// <summary>
        /// Total expected file size of the download based on the server's Content-Length HTTP response header.
        /// </summary>
        public readonly long ContentLength;

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        public readonly double PercentComplete;

        /// <summary>
        /// Download speed measured in bits per second.
        /// </summary>
        public readonly double BitsPerSecond;

        /// <summary>
        /// Human-readable download speed (e.g., "29.8 KiB/s", "2.4 MiB/s").
        /// </summary>
        public string HumanSpeed { get { return string.Format("{0}/s", FileUtils.HumanFriendlyFileSize((long)(BitsPerSecond / 8))); } }

        public DownloadState(int bytesDownloaded, long contentLength, double bitsPerSecond)
        {
            BytesDownloaded = bytesDownloaded;
            ContentLength = contentLength;
            PercentComplete = 100.0 * ((double)BytesDownloaded / ContentLength);
            BitsPerSecond = bitsPerSecond;
        }

        public override string ToString()
        {
            return string.Format("{0:N0} of {1:N0} bytes downloaded ({2:P}) @ {3:N0} bits/sec ({4})",
                BytesDownloaded,
                ContentLength,
                PercentComplete / 100.0,
                BitsPerSecond,
                HumanSpeed
            );
        }
    }

    public delegate void DownloadStateChangedHandler(DownloadState downloadState);
}
