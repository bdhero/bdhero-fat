using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetUtils.Net;
using Newtonsoft.Json;

namespace Updater
{
    public class UpdaterClient
    {
        public Update GetLatestVersion()
        {
            var json = HttpRequest.Get("http://update.bdhero.org/update.json");
            var response = JsonConvert.DeserializeObject<UpdateResponse>(json);
            var update = Update.FromResponse(response);
            return update;
        }

        public bool IsUpdateAvailable(Version currentVersion)
        {
            var update = GetLatestVersion();
            var isUpdateAvailable = update.Version > currentVersion;
            return isUpdateAvailable;
        }

        public void DownloadUpdateAsync()
        {
            var update = GetLatestVersion();

            var downloader = new FileDownloader
                {
                    Uri = update.Uri,
                    Path = Path.Combine(Path.GetTempPath(), update.FileName)
                };

            downloader.StateChanged += DownloaderOnStateChanged;
            downloader.DownloadAsync();
        }

        private void DownloaderOnStateChanged(FileDownloadState fileDownloadState)
        {
        }
    }
}
