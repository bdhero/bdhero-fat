using System;
using System.Collections.Generic;
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

        public void DownloadUpdate()
        {
            var update = GetLatestVersion();

            var downloader = new FileDownloader
                {
                    Uri = update.Uri,
                    Path = Environment.ExpandEnvironmentVariables(@"%TEMP%\" + update.FileName)
                };

            downloader.StateChanged += DownloaderOnStateChanged;
            downloader.DownloadSync();
        }

        private void DownloaderOnStateChanged(FileDownloadState fileDownloadState)
        {
        }
    }

    public class Update
    {
        public readonly Version Version;
        public readonly string FileName;
        public readonly string Uri;

        public Update(Version version, string fileName, string uri)
        {
            Version = version;
            FileName = fileName;
            Uri = uri;
        }

        public static Update FromResponse(UpdateResponse response)
        {
            var mirror = response.Mirrors.First();
            var platform = response.Platforms.Windows;
            var package = platform.Setup;

            var version = response.Version;
            var filename = package.FileName;
            var uri = mirror + filename;

            return new Update(version, uri, filename);
        }
    }
}
