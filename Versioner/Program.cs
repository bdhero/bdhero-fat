using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mono.Options;

namespace Versioner
{
    class Program
    {
        private const bool DefaultLimit10 = true;

        private const string InstallBuilderPath = @"BDHero.xml";
        private const string BDHeroPath = @"BDHero\Properties\AssemblyInfo.cs";
        private const string BDHeroCLIPath = @"BDHeroCLI\Properties\AssemblyInfo.cs";
        private const string BDHeroGUIPath = @"BDHeroGUI\Properties\AssemblyInfo.cs";

        static readonly string[] Files = new[]
            {
                InstallBuilderPath,
                BDHeroPath,
                BDHeroCLIPath,
                BDHeroGUIPath
            };

        /*
[assembly: AssemblyVersion("0.7.5.7")]
[assembly: AssemblyFileVersion("0.7.5.7")]
         */
        static readonly Regex AssemblyRegex = new Regex(@"^(\[assembly: Assembly(?:File)?Version\(.)((?:\d+\.){3}\d+)(.\)\])", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        static readonly Regex InstallBuilderRegex = new Regex(@"(<version>)((?:\d+\.){3}\d+)(</version>)", RegexOptions.IgnoreCase);

        private static bool _limit10 = DefaultLimit10;

        static void Main(string[] args)
        {
            var strategy = VersionStrategy.MinorFeature;
            var custom = "";
            var workspace = Environment.CurrentDirectory;

            var optionSet = new OptionSet
                {
                    { "strategy=", s => strategy = VersionStrategyParser.Parse(s) },
                    { "custom=", s => custom = s },
                    { "infinite", s => _limit10 = false },
                    { "workspace=", s => workspace = s }
                };

            optionSet.Parse(args);

            Environment.CurrentDirectory = workspace;

            var currentVersion = CurrentVersion;
            var newVersion = strategy == VersionStrategy.Custom ? Version.Parse(custom) : Bump(currentVersion, strategy);

            foreach (var filePath in Files)
            {
                SetVersion(filePath, newVersion);
            }
        }

        static void SetVersion(string filePath, Version newVersion)
        {
            var file = ReadFile(filePath);
            var contents = file.Key;
            var encoding = file.Value;
            Console.WriteLine("File \"{0}\" has encoding {1}", filePath, encoding);
            contents = AssemblyRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = InstallBuilderRegex.Replace(contents, "${1}" + newVersion + "${3}");
            File.WriteAllText(filePath, contents, encoding);
        }

        static KeyValuePair<string, Encoding> ReadFile(string filePath)
        {
            // open the file with the stream-reader:
            using (StreamReader reader = new StreamReader(filePath, true))
            {
                // read the contents of the file into a string
                var contents = reader.ReadToEnd();

                // return the encoding.
                return new KeyValuePair<string, Encoding>(contents, reader.CurrentEncoding);
            }
        }

        static Version CurrentVersion
        {
            get
            {
                var assemblyInfo = File.ReadAllText(BDHeroPath);
                var match = AssemblyRegex.Match(assemblyInfo);
                return Version.Parse(match.Groups[2].Value);
            }
        }

        static Version Bump(Version version, VersionStrategy strategy)
        {
            switch (strategy)
            {
                case VersionStrategy.BugFix:
                    return BumpBugFix(version);
                case VersionStrategy.FullRelease:
                    return BumpFullRelease(version);
                case VersionStrategy.MajorMilestone:
                    return BumpMajorMilestone(version);
            }
            return BumpMinorFeature(version);
        }

        private static Version BumpBugFix(Version version)
        {
            Console.WriteLine("BumpBugFix({0})", version);
            var revision = version.Revision + 1;
            if (_limit10 && revision > 9)
            {
                revision = 0;
                version = BumpMinorFeature(version);
            }
            return new Version(version.Major, version.Minor, version.Build, revision);
        }

        private static Version BumpMinorFeature(Version version)
        {
            Console.WriteLine("BumpMinorFeature({0})", version);
            var build = version.Build + 1;
            if (_limit10 && build > 9)
            {
                build = 0;
                version = BumpFullRelease(version);
            }
            return new Version(version.Major, version.Minor, build, version.Revision);
        }

        private static Version BumpFullRelease(Version version)
        {
            Console.WriteLine("BumpFullRelease({0})", version);
            var minor = version.Minor + 1;
            if (_limit10 && minor > 9)
            {
                minor = 0;
                version = BumpMajorMilestone(version);
            }
            return new Version(version.Major, minor, version.Build, version.Revision);
        }

        private static Version BumpMajorMilestone(Version version)
        {
            Console.WriteLine("BumpMajorMilestone({0})", version);
            return new Version(version.Major + 1, version.Minor, version.Build, version.Revision);
        }
    }

    /*
_._.x._ - Minor feature/enhancement (default)
_._._.x - Bug fix
_.x._._ - Full release
x._._._ - Major milestone
x.x.x.x - Custom
     */
    enum VersionStrategy
    {
        BugFix,
        MinorFeature,
        FullRelease,
        MajorMilestone,
        Custom
    }

    static class VersionStrategyParser
    {
        public static VersionStrategy Parse(string arg)
        {
            arg = (arg ?? "").Trim();
            Console.WriteLine("arg = {0}", arg);
            if (arg.StartsWith("_._._.x", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.BugFix;
            if (arg.StartsWith("_.x._._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.FullRelease;
            if (arg.StartsWith("x._._._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.MajorMilestone;
            if (arg.StartsWith("x.x.x.x", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.Custom;
            return VersionStrategy.MinorFeature;
        }
    }
}
