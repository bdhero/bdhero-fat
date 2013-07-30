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

        private const string InstallBuilderPath = @"BDHero.xml";
        private const string InstallBuilderUpdatePath = @"update.xml";
        private const string BDHeroPath = @"BDHero\Properties\AssemblyInfo.cs";
        private const string BDHeroCLIPath = @"BDHeroCLI\Properties\AssemblyInfo.cs";
        private const string BDHeroGUIPath = @"BDHeroGUI\Properties\AssemblyInfo.cs";

        static readonly string[] Files = new[]
            {
                InstallBuilderPath,
                InstallBuilderUpdatePath,
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
        static readonly Regex ArtifactFileNameRegex = new Regex(@"(<filename>\w+-)([\d.]+)(-(?:(?:windows|mac|linux)-)?(?:installer|setup|portable).(?:exe|zip|run|bin|tgz|dmg)+</filename>)", RegexOptions.IgnoreCase);

        private static bool _limit10 = DefaultLimit10;

        static void Main(string[] args)
        {
            var strategy = VersionStrategy.None;
            var custom = "";
            var workspace = Environment.CurrentDirectory;

            var optionSet = new OptionSet
                {
                    { "h|?|help", s => PrintUsageAndExit() },
                    { "v|version|p|print", s => PrintCurrentVersionAndExit() },
                    { "id|version-id", s => PrintCurrentVersionIdAndExit() },
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

        private static void PrintUsageAndExit()
        {
            var exe = Assembly.GetEntryAssembly().GetName().Name;
            Console.WriteLine("USAGE:");
            Console.WriteLine("    {0} [OPTIONS...]", exe);
            Console.WriteLine();
            Console.WriteLine("DESCRIPTION:");
            Console.WriteLine("    Utility to update BDHero version numbers.  Allows incremental \"bumps\",");
            Console.WriteLine("    custom version numbers, and normalization (ensuring that the version numbers");
            Console.WriteLine("    in all files are in sync).");
            Console.WriteLine();
            Console.WriteLine("    The \"current\" version number is read from the AssemblyVersion or");
            Console.WriteLine("    AssemblyFileVersion attributes in BDHero/Properties/AssemblyInfo.cs,");
            Console.WriteLine("    whichever appears first in the file.");
            Console.WriteLine();
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("    -h, --help, /?");
            Console.WriteLine("        Display this message and exit.");
            Console.WriteLine();
            Console.WriteLine("    -v, --version, -p, --print");
            Console.WriteLine("        Print the current BDHero version number to stdout and exit.");
            Console.WriteLine();
            Console.WriteLine("    --id, --version-id");
            Console.WriteLine("        Print the current BDHero version number ID to stdout and exit.");
            Console.WriteLine("        The version ID is a signed integer representation of the version");
            Console.WriteLine("        number suitable for use in the <versionId> tag of a");
            Console.WriteLine("        BitRock InstallBuilder update.xml file.");
            Console.WriteLine();
            Console.WriteLine("    --strategy=STRATEGY");
            Console.WriteLine("        Determines how {0} updates version numbers in the solution.", exe);
            Console.WriteLine();
            Console.WriteLine("        STRATEGY must be one of the following:");
            Console.WriteLine();
            Console.WriteLine("            \"_._._.x\": Incremental: bug fix                   (a.k.a. Version.Revision)");
            Console.WriteLine("            \"_._.x._\": Incremental: minor feature/enhancement (a.k.a. Version.Build)");
            Console.WriteLine("            \"_.x._._\": Incremental: full release              (a.k.a. Version.Minor)");
            Console.WriteLine("            \"x._._._\": Incremental: major milestone           (a.k.a. Version.Major)");
            Console.WriteLine("            \"x.x.x.x\": Non-incremental: use custom version number (see --custom)");
            Console.WriteLine("            \"_._._._\": None: don't increment the version number; keep it as is");
            Console.WriteLine();
            Console.WriteLine("    --custom=VERSION_NUMBER");
            Console.WriteLine("        Use a custom version number instead of incrementing the current number.");
            Console.WriteLine();
            Console.WriteLine("    --infinite");
            Console.WriteLine("        Don't limit version number groups (major, minor, build, revision) to 0-9");
            Console.WriteLine("        when incrementing; if a group's current value is 9, allow it to go to 10");
            Console.WriteLine("        instead of setting it to zero and incrementing the next most significant group.");
            Console.WriteLine();
            Console.WriteLine("        Examples:");
            Console.WriteLine();
            Console.WriteLine("            > 6.7.8.9 => 6.7.8.10 (with --infinite flag)");
            Console.WriteLine("            > 6.7.8.9 => 6.7.9.0  (default behavior)");
            Console.WriteLine("            > 1.9.9.9 => 2.0.0.0  (default behavior)");
            Console.WriteLine();
            Console.WriteLine("    --workspace=SOLUTION_DIR");
            Console.WriteLine("        Absolute path to the Visual Studio root solution directory.");
            Console.WriteLine("        If not specified, defaults to the current working directory (%CD%).");
            Environment.Exit(0);
        }

        private static void PrintCurrentVersionAndExit()
        {
            Console.Write(CurrentVersion);
            Environment.Exit(0);
        }

        private static void PrintCurrentVersionIdAndExit()
        {
            Console.Write(CurrentVersion.GetId());
            Environment.Exit(0);
        }

        static void SetVersion(string filePath, Version newVersion)
        {
            var file = ReadFile(filePath);
            var contents = file.Key;
            var encoding = file.Value;

            Console.WriteLine("File \"{0}\" has encoding {1}", filePath, encoding);

            contents = AssemblyRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = InstallBuilderVersionRegex.Replace(contents, "${1}" + newVersion + "${3}");
            contents = InstallBuilderVersionIdRegex.Replace(contents, "${1}" + newVersion.GetId() + "${3}");
            contents = InstallBuilderUpdateVersionIdRegex.Replace(contents, "${1}" + newVersion.GetId() + "${3}");
            contents = ArtifactFileNameRegex.Replace(contents, "${1}" + newVersion + "${3}");

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
            return new Version(version.Major, version.Minor, build, 0);
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
            return new Version(version.Major, minor, 0, 0);
        }

        private static Version BumpMajorMilestone(Version version)
        {
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
        /// <example><code>new Version(0, 8, 0, 1).GetId() == 80001</code></example>
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
