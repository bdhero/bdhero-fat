using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDHero.BDROM;
using DotNetUtils;
using I18N;
using IniParser;

namespace BDHero.Plugin.DiscReader.Transformer
{
    public static class DiscMetadataTransformer
    {
        private static readonly Regex BdmtFileNameRegex = new Regex(
            "bdmt_([a-z]{3}).xml",
            RegexOptions.IgnoreCase);

        private static readonly Regex BdmtXmlTitleRegex = new Regex(
            "<di:name>(.*?)</di:name>",
            RegexOptions.IgnoreCase);

        private static readonly Regex DBOXTitleRegex = new Regex(
            "<Title>(.*?)</Title>",
            RegexOptions.IgnoreCase);

        private static readonly Regex ISANContentIdRegex = new Regex(
            "<mcmf[^>]+contentID=\"(?:0x)?[a-z0-9]+([a-z0-9]{24})\"",
            RegexOptions.IgnoreCase);

        public static void Transform(Disc disc)
        {
            var metadata = new DiscMetadata
                {
                    HardwareVolumeLabel = GetHardwareVolumeLabel(disc),
                    DiscInf = GetAnyDVDDiscInf(disc),
                    AllBdmtTitles = GetAllBdmtTitles(disc),
                    ValidBdmtTitles = null, /* assigned below */
                    DBOXTitle = GetDBOXTitle(disc),
                    V_ISAN = GetVISAN(disc),
                    ISAN = null /* populated by another plugin */
                };

            metadata.ValidBdmtTitles = GetValidBdmtTitles(metadata.AllBdmtTitles);

            disc.Metadata = metadata;
        }

        private static IDictionary<Language, string> GetValidBdmtTitles(IDictionary<Language, string> allBdmtTitles)
        {
            var valid = new Dictionary<Language, string>();
            var filtered = allBdmtTitles.Where(IsBdmtTitleValid);
            foreach (var kvp in filtered)
            {
                valid[kvp.Key] = kvp.Value;
            }
            return valid;
        }

        private static bool IsBdmtTitleValid(KeyValuePair<Language, string> keyValuePair)
        {
            var title = (keyValuePair.Value ?? "").Trim();
            return !string.IsNullOrWhiteSpace(title);
        }

        private static string GetHardwareVolumeLabel(Disc disc)
        {
            var root = disc.FileSystem.Directories.Root;
            var parent = Directory.GetParent(root.FullName);

            if (parent == null)
            {
                // path is the root directory
                var drives = DriveInfo.GetDrives().ToArray();
                var drive = drives.FirstOrDefault(info => info.Name == root.FullName);
                if (drive != null)
                {
                    return drive.VolumeLabel;
                }
            }

            return root.Name;
        }

        private static AnyDVDDiscInf GetAnyDVDDiscInf(Disc disc)
        {
            var discInf = disc.FileSystem.Files.AnyDVDDiscInf;
            if (discInf == null)
                return null;
            var parser = new FileIniDataParser();
            var data = parser.LoadFile(discInf.FullName);
            var discData = data["disc"];
            var anyDVDDiscInf = new AnyDVDDiscInf
                {
                    AnyDVDVersion = discData["version"],
                    VolumeLabel = discData["label"],
                    Region = RegionCodeParser.Parse(discData["region"])
                };
            return anyDVDDiscInf;
        }

        private static IDictionary<Language, string> GetAllBdmtTitles(Disc disc)
        {
            var titles = new Dictionary<Language, string>();
            foreach (var file in disc.FileSystem.Files.BDMT)
            {
                var filenameMatch = BdmtFileNameRegex.Match(file.Name);
                var iso639_2 = filenameMatch.Groups[1].Value;
                var language = Language.FromCode(iso639_2);
                var xml = string.Join("", File.ReadLines(file.FullName));

                if (!BdmtXmlTitleRegex.IsMatch(xml))
                    continue;

                var xmlMatch = BdmtXmlTitleRegex.Match(xml);
                var title = xmlMatch.Groups[1].Value.Trim();

                titles[language] = title;
            }
            return titles;
        }

        private static string GetDBOXTitle(Disc disc)
        {
            var dbox = disc.FileSystem.Files.DBOX;
            if (dbox == null)
                return null;

            string xml;
            var encoding = FileUtils.DetectEncodingAuto(dbox.FullName, out xml);

            // Replace newlines with spaces
            xml = Regex.Replace(xml, @"[\n\r\f]+", " ");

            if (!DBOXTitleRegex.IsMatch(xml))
                return null;

            var match = DBOXTitleRegex.Match(xml);
            var title = match.Groups[1].Value.Trim();
            return title;
        }

        private static ISAN GetVISAN(Disc disc)
        {
            var file = disc.FileSystem.Files.MCMF;
            if (file == null)
                return null;

            var xml = string.Join(" ", File.ReadAllLines(file.FullName));
            if (!ISANContentIdRegex.IsMatch(xml))
                return null;

            var match = ISANContentIdRegex.Match(xml);
            var contentId = match.Groups[1].Value;

            return ISAN.TryParse(contentId);
        }
    }
}
