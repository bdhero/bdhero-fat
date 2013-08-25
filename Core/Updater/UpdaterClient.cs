﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetUtils.Crypto;
using DotNetUtils.Net;
using Newtonsoft.Json;

namespace Updater
{
    public class UpdaterClient
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Update GetLatestVersionSync()
        {
            var json = HttpRequest.Get("http://update.bdhero.org/update.json");
            var response = JsonConvert.DeserializeObject<UpdateResponse>(json);
            var update = Update.FromResponse(response);
            return update;
        }

        public bool IsUpdateAvailableSync(Version currentVersion)
        {
            var update = GetLatestVersionSync();
            var isUpdateAvailable = update.Version > currentVersion;
            return isUpdateAvailable;
        }

        /// <summary>
        /// Checks for application updates asynchronously.
        /// </summary>
        /// <param name="currentVersion">Current version number of BDHero</param>
        /// <returns>
        /// A <c>Task</c> whose <see cref="Task{T}.Result"/> returns <c>true</c> if
        /// an update is available or <c>false</c> if no update is available.
        /// </returns>
        public Task<bool> IsUpdateAvailableAsync(Version currentVersion)
        {
            return Task.Factory.StartNew(() => IsUpdateAvailableSync(currentVersion));
        }

        /// <summary>
        /// Begins downloading the latest update for the current platform asynchronously.
        /// </summary>
        /// <returns>
        /// <para><c>Task</c> object that returns a <c>string</c> containing the path of the downloaded update file.</para>
        /// <para><strong>NOTE</strong>: If the download is canceled, <see cref="Task{T}.Result"/> will return <c>null</c>.</para>
        /// </returns>
        /// <exception cref="IOException">
        /// Thrown if a network error occurs or the SHA-1 hash of the downloaded file
        /// does not match the expected value in the update manifest.
        /// </exception>
        public Task<string> DownloadUpdateAsync()
        {
            var update = GetLatestVersionSync();
            var path = Path.Combine(Path.GetTempPath(), update.FileName);

            var downloader = new FileDownloader
                {
                    Uri = update.Uri,
                    Path = path
                };

            downloader.StateChanged += DownloaderOnStateChanged;

            return downloader.DownloadAsync().ContinueWith(delegate(Task task)
                {
                    if (!task.IsCompleted) return null;
                    var hash = new SHA1Algorithm().ComputeFile(path);
                    if (!String.Equals(hash, update.SHA1, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.ErrorFormat(
                            "Unable to verify integrity of \"{0}\" via SHA-1 hash: expected {1}, but found {2}",
                            path, update.SHA1, hash);
                        throw new IOException("Update file is corrupt or has been tampered with; SHA-1 hash is incorrect");
                    }
                    return path;
                });
        }

        private void DownloaderOnStateChanged(FileDownloadState fileDownloadState)
        {
        }
    }
}
