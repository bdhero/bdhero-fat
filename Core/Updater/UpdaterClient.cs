using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DotNetUtils.Net;
using Newtonsoft.Json;

namespace Updater
{
    public class UpdaterClient
    {
        public Update GetLatestVersion()
        {
            var xml = HttpRequest.Get("http://update.bdhero.org/update.xml");
            xml = SanitizeXml(xml);
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc);
            var response = JsonConvert.DeserializeObject<UpdateResponse>(json);
            var update = Update.FromResponse(response);
            return update;
        }

        private static string SanitizeXml(string xml)
        {
            xml = Regex.Replace(xml, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            xml = Regex.Replace(xml, @"<\?xml[^>]*>[\n\r\f]*", string.Empty, RegexOptions.IgnoreCase);
            xml = xml.Replace("?xml", "xml");
            return xml;
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

        private void DownloaderOnStateChanged(DownloadState downloadState)
        {
        }
    }

    public class Update
    {
        public readonly Version Version;
        public readonly string Uri;
        public readonly string FileName;

        public Update(Version version, string uri, string fileName)
        {
            Version = version;
            Uri = uri;
            FileName = fileName;
        }

        public static Update FromResponse(UpdateResponse response)
        {
            var root = response.InstallerInformation.DownloadLocationList.DownloadLocation.Url;
            var filename = response.InstallerInformation.PlatformFileList.PlatformFile.Filename;
            var uri = root + filename;

//            var strVersion = new Regex(@"((?:\d+\.){3}\d+)").Match(filename).Groups[1].Value;
//            var version = Version.Parse(strVersion);

            var version = Version.Parse(response.InstallerInformation.Version);

            return new Update(version, uri, filename);
        }
    }
}
