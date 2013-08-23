using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Updater
{
    public class UpdateResponse
    {
        [JsonProperty(PropertyName = "installerInformation")]
        public InstallerInformation InstallerInformation { get; set; }
    }

    public class InstallerInformation
    {
        [JsonProperty(PropertyName = "versionId")]
        public int VersionId { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "platformFileList")]
        public PlatformFileList PlatformFileList { get; set; }

        [JsonProperty(PropertyName = "downloadLocationList")]
        public DownloadLocationList DownloadLocationList { get; set; }
    }

    public class PlatformFileList
    {
        [JsonProperty(PropertyName = "platformFile")]
        public PlatformFile PlatformFile { get; set; }
    }

    public class PlatformFile
    {
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }
    }

    public class DownloadLocationList
    {
        [JsonProperty(PropertyName = "downloadLocation")]
        public DownloadLocation DownloadLocation { get; set; }
    }

    public class DownloadLocation
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
