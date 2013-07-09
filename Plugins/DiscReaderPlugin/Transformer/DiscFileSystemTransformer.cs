using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.BDROM;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class DiscFileSystemTransformer
    {
        public static void Transform(BDInfo.BDROM bdrom, Disc disc)
        {
            var fs = new DiscFileSystem
                {
                    DirectoryRoot = bdrom.DirectoryRoot,
                    DirectoryBDMV = bdrom.DirectoryBDMV,
                    DirectoryBDJO = bdrom.DirectoryBDJO,
                    DirectoryCLIPINF = bdrom.DirectoryCLIPINF,
                    DirectoryPLAYLIST = bdrom.DirectoryPLAYLIST,
                    DirectorySTREAM = bdrom.DirectorySTREAM,
                    DirectorySSIF = bdrom.DirectorySSIF,
                    DirectoryBDMT = GetBDMTDirectory(bdrom.DirectoryBDMV),
                    DirectorySNP = bdrom.DirectorySNP,
                    DirectoryANY = GetDirectory("ANY!", bdrom.DirectoryRoot),
                    DirectoryMAKEMKV = GetDirectory("MAKEMKV", bdrom.DirectoryRoot)
                };

            fs.DirectoryAACS = GetAACSDirectory(fs);
            fs.FileMCMF = GetFile("mcmf.xml", fs.DirectoryAACS);
            fs.FileDBOX = GetFile("FilmIndex.xml", fs.DirectoryRoot);
            fs.FilesBDMT = GetFilesByPattern("bdmt_???.xml", fs.DirectoryBDMT);

            disc.FileSystem = fs;
        }

        private static DirectoryInfo GetBDMTDirectory(DirectoryInfo bdmv)
        {
            var path = Path.Combine(bdmv.FullName, "META", "DL");
            return Directory.Exists(path) ? new DirectoryInfo(path) : null;
        }

        private static DirectoryInfo GetAACSDirectory(DiscFileSystem fs)
        {
            return fs.DirectoryANY ??
                   GetDirectory("AACS", fs.DirectoryMAKEMKV) ??
                   GetDirectory("AACS", fs.DirectoryRoot);
        }

        private static DirectoryInfo GetDirectory(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetDirectories().FirstOrDefault(info => info.Name == name) : null;
        }

        private static FileInfo GetFile(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetFiles().FirstOrDefault(info => info.Name == name) : null;
        }

        private static FileInfo[] GetFilesByPattern(string pattern, DirectoryInfo dir)
        {
            return dir != null ? dir.GetFiles(pattern) : null;
        }
    }
}
