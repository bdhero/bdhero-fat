using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Mono.Options;

namespace Versioner
{
    class Program
    {
        private const bool DefaultLimit10 = true;

        private const string InstallBuilderPath = @"Installer.xml";
        private const string InstallBuilderAutoUpdatePath = @"AutoUpdate.xml";
        private const string InstallBuilderUpdatePath = @"update.xml";
        private const string InnoSetupPath = @"Installer\InnoSetup\setup.iss";
        private const string BDHeroPath = @"BDHero\Properties\AssemblyInfo.cs";
        private const string BDHeroCLIPath = @"BDHeroCLI\Properties\AssemblyInfo.cs";
        private const string BDHeroGUIPath = @"BDHeroGUI\Properties\AssemblyInfo.cs";

        static readonly string[] Files = new[]
            {
                InstallBuilderPath,
                InstallBuilderAutoUpdatePath,
                InstallBuilderUpdatePath,
                InnoSetupPath,
                BDHeroPath,
                BDHeroCLIPath,
                BDHeroGUIPath
            };

        /*
[assembly: AssemblyVersion("0.7.5.7")]
[assembly: AssemblyFileVersion("0.7.5.7")]
         */
        static readonly Regex AssemblyRegex = new Regex(@"^(\[assembly: Assembly(?:File)?Version\(.)((?:\d+\.){3}\d+)(.\)\])", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        static readonly Regex InstallBuilderVersionRegex = new Regex(@"(<version>)((?:\d+\.){3}\d+)(</version>)", RegexOptions.IgnoreCase);
        static readonly Regex InstallBuilderVersionIdRegex = new Regex(@"(<name>application_version_id</name>\s+<value>)(\d+)(</value>)", RegexOptions.IgnoreCase);
        static readonly Regex InstallBuilderUpdateVersionIdRegex = new Regex(@"(<versionId>)(\d+)(</versionId>)", RegexOptions.IgnoreCase);
        static readonly Regex InnoSetupVersionRegex = new Regex(@"(#define MyAppVersion .)((?:\d+\.){3}\d+)(.)", RegexOptions.IgnoreCase);
        static readonly Regex ArtifactFileNameRegex = new Regex(@"(<filename>\w+-)([\d.]+)(-(?:(?:windows|mac|linux)-)?(?:installer|setup|portable).(?:exe|zip|run|bin|tgz|dmg)+</filename>)", RegexOptions.IgnoreCase);

        private static bool _limit10 = DefaultLimit10;

        private static bool _commitChanges = true;
        private static bool _printCurrentVersion = false;
        private static bool _printCurrentVersionId = false;

        private static bool IsPrintAndExit { get { return _printCurrentVersion || _printCurrentVersionId; } }

        static void Main(string[] args)
        {
            var workspace = Environment.CurrentDirectory;
            var strategy = VersionStrategy.None;
            var custom = "";
            var testVersion = "";

            var optionSet = new OptionSet
                {
                    { "h|?|help", s => PrintUsageAndExit() },
                    { "workspace=", s => workspace = s },
                    { "test", s => _commitChanges = false },
                    { "test-with=|testwith=", s => testVersion = s },
                    { "v|version|p|print", s => _printCurrentVersion = true },
                    { "id|version-id", s => _printCurrentVersionId = true },
                    { "strategy=", s => strategy = VersionStrategyParser.Parse(s) },
                    { "custom=", s => custom = s },
                    { "infinite|no-limit", s => _limit10 = false }
                };

            optionSet.Parse(args);

            Environment.CurrentDirectory = workspace;

            var overrideCurrentVersion = !string.IsNullOrWhiteSpace(testVersion);
            if (overrideCurrentVersion)
                _commitChanges = false;

            if (!_commitChanges && !IsPrintAndExit)
                Console.WriteLine("TEST RUN - changes will NOT be written to disk");

            var currentVersion = overrideCurrentVersion ? Version.Parse(testVersion) : CurrentVersion;
            var newVersion = strategy == VersionStrategy.Custom ? Version.Parse(custom) : Bump(currentVersion, strategy);

            if (_printCurrentVersion)
                PrintCurrentVersionAndExit(currentVersion);

            if (_printCurrentVersionId)
                PrintCurrentVersionIdAndExit(currentVersion);

            foreach (var filePath in Files)
            {
                SetVersion(filePath, newVersion);
            }

            Console.WriteLine("{0} => {1}", currentVersion, newVersion);
        }

        private static void PrintUsageAndExit()
        {
            var exeName = Assembly.GetEntryAssembly().GetName().Name;
            var usage = new Usage(exeName).TransformText();
            Console.Error.WriteLine(usage);
            Environment.Exit(0);
        }

        private static void PrintCurrentVersionAndExit(Version version = null)
        {
            Console.Write(version ?? CurrentVersion);
            Environment.Exit(0);
        }

        private static void PrintCurrentVersionIdAndExit(Version version = null)
        {
            Console.Write((version ?? CurrentVersion).GetId());
            Environment.Exit(0);
        }

        static void SetVersion(string filePath, Version newVersion)
        {
            var file = ReadFile(filePath);
            var contents = file.Key;
            var encoding = file.Value;

            Console.WriteLine("File \"{0}\" has encoding {1}", filePath, encoding.EncodingName);

            contents = AssemblyRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = InstallBuilderVersionRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = InstallBuilderVersionIdRegex.Replace(contents, "${1}" + newVersion.GetId() + "${3}");
            contents = InstallBuilderUpdateVersionIdRegex.Replace(contents, "${1}" + newVersion.GetId() + "${3}");
            contents = InnoSetupVersionRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = ArtifactFileNameRegex.Replace(contents, "${1}" + newVersion + "${3}");

            if (_commitChanges)
            {
                File.WriteAllText(filePath, contents, encoding);
            }
        }

        static KeyValuePair<string, Encoding> ReadFile(string filePath)
        {
            // open the file with the stream-reader:
            using (var reader = new StreamReader(filePath, true))
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
                case VersionStrategy.MinorFeature:
                    return BumpMinorFeature(version);
                case VersionStrategy.FullRelease:
                    return BumpFullRelease(version);
                case VersionStrategy.MajorMilestone:
                    return BumpMajorMilestone(version);
            }
            return version;
        }

        private static Version BumpBugFix(Version version)
        {
            if (!IsPrintAndExit)
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
            if (!IsPrintAndExit)
                Console.WriteLine("BumpMinorFeature({0})", version);
            var build = version.Build + 1;
            if (_limit10 && build > 9)
            {
                build = 0;
                version = BumpFullRelease(version);
            }
            return new Version(version.Major, version.Minor, build, 0);
        }

        private static Version BumpFullRelease(Version version)
        {
            if (!IsPrintAndExit)
                Console.WriteLine("BumpFullRelease({0})", version);
            var minor = version.Minor + 1;
            if (_limit10 && minor > 9)
            {
                minor = 0;
                version = BumpMajorMilestone(version);
            }
            return new Version(version.Major, minor, 0, 0);
        }

        private static Version BumpMajorMilestone(Version version)
        {
            if (!IsPrintAndExit)
                Console.WriteLine("BumpMajorMilestone({0})", version);
            return new Version(version.Major + 1, 0, 0, 0);
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
        Custom,
        None
    }

    static class VersionStrategyParser
    {
        public static VersionStrategy Parse(string arg)
        {
            arg = (arg ?? "").Trim();
            Console.WriteLine("arg = {0}", arg);
            if (arg.StartsWith("_._._.x", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.BugFix;
            if (arg.StartsWith("_._.x._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.MinorFeature;
            if (arg.StartsWith("_.x._._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.FullRelease;
            if (arg.StartsWith("x._._._", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.MajorMilestone;
            if (arg.StartsWith("x.x.x.x", StringComparison.InvariantCultureIgnoreCase))
                return VersionStrategy.Custom;
            return VersionStrategy.None;
        }
    }

    static class VersionExtensions
    {
        /// <summary>
        /// <para>
        /// Converts the <c>Version</c> to a signed integer representation suitable for use in the
        /// <c>&lt;versionId&gt;</c> tag of a BitRock InstallBuilder update.xml file.
        /// </para>
        /// <para>
        /// Each octet in <paramref name="version"/> is converted to 2 decimal digits and concatenated in
        /// descending order of significance.  Therefore, the value of each octet must not exceed 99.
        /// </para>
        /// </summary>
        /// <example><code>new Version(1, 2, 3, 4).GetId() == 1020304</code></example>
        /// <example><code>new Version(0, 8, 0, 1).GetId() ==   80001</code></example>
        /// <param name="version"></param>
        /// <returns>The value of <paramref name="version"/> as a signed <c>Int32</c></returns>
        /// <seealso cref="http://installbuilder.bitrock.com/docs/installbuilder-userguide/ar01s23.html">BitRock InstallBuilder update.xml file</seealso>
        public static int GetId(this Version version)
        {
            var v = version;
            return int.Parse(string.Format("{0:D2}{1:D2}{2:D2}{3:D2}", v.Major, v.Minor, v.Build, v.Revision));
        }
    }
}
