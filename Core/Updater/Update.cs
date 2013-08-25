using System;
using System.Linq;

namespace Updater
{
    public class Update
    {
        public readonly Version Version;
        public readonly string FileName;
        public readonly string Uri;
        public readonly string SHA1;
        public readonly long Size;

        public Update(Version version, string fileName, string uri, string sha1, long size)
        {
            Version = version;
            FileName = fileName;
            Uri = uri;
            SHA1 = sha1;
            Size = size;
        }

        public static Update FromResponse(UpdateResponse response)
        {
            var mirror = response.Mirrors.First();
            var platform = response.Platforms.Windows;
            var package = platform.Packages.Setup;

            var version = response.Version;
            var filename = package.FileName;
            var uri = mirror + filename;

            return new Update(version, filename, uri, package.SHA1, package.Size);
        }
    }
}