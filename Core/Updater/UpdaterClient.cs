using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetUtils.Annotations;
using DotNetUtils.Crypto;
using DotNetUtils.Net;
using Newtonsoft.Json;

namespace Updater
{
    public class UpdaterClient
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static UpdaterClient Instance = new UpdaterClient();

        private Update _latestUpdate;

        private volatile UpdaterClientState _state;

        /// <summary>
        /// Invoked just before all HTTP requests are made, allowing observers to modify requests before they are sent.
        ///  This can be useful to override the system's default proxy settings, set custom timeout values, etc.
        /// </summary>
        public event BeforeRequestEventHandler BeforeRequest;

        private void NotifyBeforeRequest(HttpWebRequest request)
        {
            if (BeforeRequest != null)
                BeforeRequest(request);
        }

        private void CheckState()
        {
            if (_state == UpdaterClientState.Checking)
                throw new InvalidOperationException("Already checking for updates");
            if (_state == UpdaterClientState.Downloading)
                throw new InvalidOperationException("Already downloading update");
            if (_state == UpdaterClientState.Paused)
                throw new InvalidOperationException("Already downloading update (paused)");
        }

        public Update GetLatestVersionSync()
        {
            CheckState();

            try
            {
                _state = UpdaterClientState.Checking;
                HttpRequest.BeforeRequestGlobal += NotifyBeforeRequest;
                var json = HttpRequest.Get("http://update.bdhero.org/update.json");
                var response = JsonConvert.DeserializeObject<UpdateResponse>(json);
                _latestUpdate = Update.FromResponse(response);
                _state = UpdaterClientState.Ready;
                return _latestUpdate;
            }
            catch (Exception e)
            {
                _state = UpdaterClientState.Error;
                Logger.Error("Error occurred while checking for application update", e);
                throw;
            }
            finally
            {
                HttpRequest.BeforeRequestGlobal -= NotifyBeforeRequest;
            }
        }

        public bool IsUpdateAvailableSync(Version currentVersion)
        {
            var update = GetLatestVersionSync();
            return IsUpdateAvailable(currentVersion, update);
        }

        private static bool IsUpdateAvailable(Version currentVersion, Update update)
        {
            var isUpdateAvailable = update.Version > currentVersion;
            return isUpdateAvailable;
        }

        /// <summary>
        /// Downloads the latest update for the current platform synchronously.
        /// </summary>
        /// <returns>Path of the downloaded update file.</returns>
        /// <exception cref="IOException">
        /// Thrown if a network error occurs or the SHA-1 hash of the downloaded file
        /// does not match the expected value in the update manifest.
        /// </exception>
        [CanBeNull]
        public string DownloadUpdateSync()
        {
            var update = GetLatestVersionSync();
            return DownloadUpdateSync(update);
        }

        private string DownloadUpdateSync(Update update)
        {
            var path = Path.Combine(Path.GetTempPath(), update.FileName);

            var downloader = new FileDownloader
                {
                    Uri = update.Uri,
                    Path = path
                };

            downloader.BeforeRequest += NotifyBeforeRequest;
            downloader.StateChanged += DownloaderOnStateChanged;

            downloader.DownloadSync();
            var hash = new SHA1Algorithm().ComputeFile(path);
            if (!String.Equals(hash, update.SHA1, StringComparison.OrdinalIgnoreCase))
            {
                Logger.ErrorFormat(
                    "Unable to verify integrity of \"{0}\" via SHA-1 hash: expected {1}, but found {2}",
                    path, update.SHA1, hash);
                throw new IOException("Update file is corrupt or has been tampered with; SHA-1 hash is incorrect");
            }
            return path;
        }

        /// <summary>
        /// Downloads the latest update for the current platform synchronously
        /// and returns the path to the downloaded file.
        /// </summary>
        /// <returns>Path of the downloaded update file.</returns>
        /// <exception cref="IOException">
        /// Thrown if a network error occurs or the SHA-1 hash of the downloaded file
        /// does not match the expected value in the update manifest.
        /// </exception>
        [CanBeNull]
        public string DownloadUpdateIfAvailableSync(Version currentVersion)
        {
            var update = GetLatestVersionSync();
            return IsUpdateAvailable(currentVersion, update)
                ? DownloadUpdateSync(update)
                : null;
        }

        // TODO: Save state from other methods in this class.
        // When an update is either found or not, cache the result and save the setup EXE path.
        // Callers shouldn't have to pass around return values from the other methods.
        public void InstallUpdate()
        {
        }

        private void DownloaderOnStateChanged(FileDownloadState fileDownloadState)
        {
        }
    }

    enum UpdaterClientState
    {
        Ready,
        Checking,
        Downloading,
        Paused,
        Error
    }
}
