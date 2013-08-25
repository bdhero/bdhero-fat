using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BuildUtils;
using DotNetUtils.Crypto;
using Mono.Options;
using Newtonsoft.Json;
using Updater;

namespace UpdateManifestGenerator
{
    class Program
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Platform _curPlatform;

        static void Main(string[] args)
        {
            string outputPath = "update.json";
            var update = new UpdateResponse
                {
                    Version = VersionUtils.CurrentVersion
                };

            var optionSet = new OptionSet
                {
//                    { "h|?|help", s => PrintUsageAndExit() },

                    { "workspace", s => Environment.CurrentDirectory = s },

                    { "v=|version=", s => update.Version = Version.Parse(s) },
                    { "r=|mirror=",  s => update.Mirrors.Add(s)             },

                    { "w|windows", s => _curPlatform = update.Platforms.Windows },
                    { "m|mac",     s => _curPlatform = update.Platforms.Mac     },
                    { "l|linux",   s => _curPlatform = update.Platforms.Linux   },

                    { "s=|setup=",              s => _curPlatform.Setup    = CreatePackage(s) },
                    { "x=|sfx=",                s => _curPlatform.Sfx      = CreatePackage(s) },
                    { "7=|7z=|7zip=|sevenZip=", s => _curPlatform.SevenZip = CreatePackage(s) },
                    { "z=|zip=",                s => _curPlatform.Zip      = CreatePackage(s) },

                    { "o=|output=", s => outputPath = s },
                };

            var extra = optionSet.Parse(args);

            if (extra.Any())
            {
                Logger.WarnFormat("Warning: Unknown arguments passed; ignoring: [ \"{0}\" ]", string.Join("\", \"", extra));
            }

            var json = JsonConvert.SerializeObject(update, Formatting.Indented);
            File.WriteAllText(outputPath, json);
        }

        private static Package CreatePackage(string path)
        {
            var package = new Package
                {
                    FileName = Path.GetFileName(path),
                    Size = new FileInfo(path).Length,
                    SHA1 = new SHA1Algorithm().ComputeFile(path)
                };
            return package;
        }
    }
}
