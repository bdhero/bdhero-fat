using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using DotNetUtils.Annotations;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class DiscFileSystemTransformer
    {
        public static void Transform(BDInfo.BDROM bdrom, Disc disc)
        {
            var fs = new DiscFileSystem
                {
                    Directories = new DiscFileSystem.DiscDirectories
                        {
                            Root = bdrom.DirectoryRoot,
                            BDMV = bdrom.DirectoryBDMV,
                            CLIPINF = bdrom.DirectoryCLIPINF,
                            PLAYLIST = bdrom.DirectoryPLAYLIST,
                            STREAM = bdrom.DirectorySTREAM,
                            SSIF = bdrom.DirectorySSIF,
                            BDMT = GetBDMTDirectory(bdrom.DirectoryBDMV),
                            BDJO = bdrom.DirectoryBDJO,
                            SNP = bdrom.DirectorySNP,
                            ANY = GetDirectory("ANY!", bdrom.DirectoryRoot),
                            MAKEMKV = GetDirectory("MAKEMKV", bdrom.DirectoryRoot),
                            AACS = null /* assigned below */
                        },
                    Files = new DiscFileSystem.DiscFiles
                        {
                            DBOX = GetFile("FilmIndex.xml", bdrom.DirectoryRoot),
                            MCMF = null, /* assigned below */
                            BDMT = null  /* assigned below */
                        }
                };

            fs.Directories.AACS = GetAACSDirectory(fs);
            fs.Files.MCMF = GetFile("mcmf.xml", fs.Directories.AACS);
            fs.Files.BDMT = GetFilesByPattern("bdmt_???.xml", fs.Directories.BDMT);

            disc.FileSystem = fs;
        }

        [CanBeNull]
        private static DirectoryInfo GetBDMTDirectory(DirectoryInfo bdmv)
        {
            var path = Path.Combine(bdmv.FullName, "META", "DL");
            return Directory.Exists(path) ? new DirectoryInfo(path) : null;
        }

        private static DirectoryInfo GetAACSDirectory(DiscFileSystem fs)
        {
            return fs.Directories.ANY ??
                   GetDirectory("AACS", fs.Directories.MAKEMKV) ??
                   GetDirectory("AACS", fs.Directories.Root);
        }

        [CanBeNull]
        private static DirectoryInfo GetDirectory(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetDirectories().FirstOrDefault(info => info.Name == name) : null;
        }

        [CanBeNull]
        private static FileInfo GetFile(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetFiles().FirstOrDefault(info => info.Name == name) : null;
        }

        [NotNull]
        private static FileInfo[] GetFilesByPattern(string pattern, DirectoryInfo dir)
        {
            return dir != null ? dir.GetFiles(pattern) : new FileInfo[0];
        }
    }
}
