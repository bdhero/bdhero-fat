using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Annotations;

namespace DotNetUtils
{
    public static class FileUtils
    {
        /// <see cref="http://stackoverflow.com/a/4975942/467582"/>
        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" }; // Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return string.Format("{0} {1}", (Math.Sign(byteCount) * num).ToString("0.0"), suf[place]);
        }

        /// <summary>
        /// Creates an <see cref="Image"/> object without locking the source file.
        /// </summary>
        /// <param name="path">Path to the image file</param>
        /// <returns><see cref="Image"/> object</returns>
        /// <see cref="http://stackoverflow.com/a/1105330/467582"/>
        public static Image ImageFromFile(string path)
        {
            return Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
        }

        public static Icon ExtractIcon(string exePath)
        {
            try
            {
                return Icon.ExtractAssociatedIcon(exePath);
            }
            catch
            {
                return null;
            }
        }

        public static Image ExtractIconAsBitmap(string exePath)
        {
            var icon = ExtractIcon(exePath);
            return icon != null ? icon.ToBitmap() : null;
        }

        public static bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        public static bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static string FormatFileSize(long filesize, ushort maxFractionalDigits = 2)
        {
            string[] sizes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };
            double len = filesize;
            var order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            var fractionalFormat = maxFractionalDigits > 0 ? "." + new string('#', maxFractionalDigits) : "";

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return string.Format("{0:0" + fractionalFormat + "} {1}", len, sizes[order]);
        }

        // TODO: UNIT TEST THIS METHOD
        /// <summary>
        /// Creates all directories in the specified path hierarchy if they do not already exist.
        /// If the path contains a file name with an extension, the file's parent directory will be created.
        /// </summary>
        /// <param name="path">Path to a file or directory</param>
        public static void CreateDirectory(string path)
        {
            // TODO: Does this check make sense?
            // Path will resolve to current working directory
            if (string.IsNullOrEmpty(path))
                return;

            // More accurate checks first
            if (File.Exists(path) || Directory.Exists(path))
                return;

            var dirPath = path;

            if (ContainsFileName(dirPath))
            {
                dirPath = Path.GetDirectoryName(dirPath);
            }

            if (!string.IsNullOrEmpty(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        // TODO: UNIT TEST THIS METHOD
        /// <summary>
        /// Determines whether the given path <em>likely</em> contains (ends with) a standard Windows filename (i.e., one that has an extension).
        /// </summary>
        /// <param name="path">Relative or absolute path to a file or directory</param>
        /// <returns><code>true</code> if the path ends in a period followed by at least one letter; otherwise <code>false</code></returns>
        /// <example>"C:\some\dir\a.out" => true</example>
        /// <example>"a.out" => true</example>
        /// <example>"file.c" => true</example>
        /// <example>"file.php3" => true</example>
        /// <example>"file.3" => true</example>
        /// <example>"file" => false</example>
        /// <example>"C:\some\dir\file" => false</example>
        /// <example>"C:\some\dir" => false</example>
        /// <example>"C:\" => false</example>
        /// <example>"" => false</example>
        public static bool ContainsFileName([NotNull] string path)
        {
            return new Regex(@"[^/\\]\.\w+$").IsMatch(path);
        }

        /// <see cref="http://stackoverflow.com/questions/62771/how-check-if-given-string-is-legal-allowed-file-name-under-windows"/>
        public static bool IsValidFilename(string testName)
        {
            var containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
            return !containsABadCharacter.IsMatch(testName);
        }

        public static string SanitizeFileName(string fileName)
        {
            var badCharRegex = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]+");
            var multiBadCharRegex = new Regex(@"-(?:\s*-)+");
            var multiSpaceRegex = new Regex(@"\s{2,}");

            var sanitizedFileName = fileName;
            sanitizedFileName = badCharRegex.Replace(sanitizedFileName, " - ");
            sanitizedFileName = multiBadCharRegex.Replace(sanitizedFileName, "-");
            sanitizedFileName = multiSpaceRegex.Replace(sanitizedFileName, " ");

            return sanitizedFileName;
        }

        public static bool IsEmpty(DirectoryInfo dir)
        {
            return dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0;
        }

        public static void OpenFile(string filePath)
        {
            if (HasProgramAssociation(filePath))
                Process.Start(filePath);
        }

        public static void ShowInFolder(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            // combine the arguments together
            // it doesn't matter if there is a space after ','
            string argument = "/select, \"" + filePath + "\"";

            Process.Start("explorer.exe", argument);
        }

        public static bool HasProgramAssociation(string filePath)
        {
            var hasAssoc = false;
            if (Path.HasExtension(filePath))
            {
                try
                {
                    var defaultProgram = FileExtentionInfo(AssocStr.Executable, Path.GetExtension(filePath));
                    hasAssoc = !string.IsNullOrEmpty(defaultProgram) && File.Exists(defaultProgram);
                }
                catch
                {
                }
            }
            return hasAssoc;
        }

        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, [In][Out] ref uint pcchOut);

        public static string FileExtentionInfo(AssocStr assocStr, string doctype)
        {
            uint pcchOut = 0;
            AssocQueryString(AssocF.Verify, assocStr, doctype, null, null, ref pcchOut);

            var pszOut = new StringBuilder((int)pcchOut);
            AssocQueryString(AssocF.Verify, assocStr, doctype, null, pszOut, ref pcchOut);
            return pszOut.ToString();
        }

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

        [Flags]
        public enum AssocF
        {
            Init_NoRemapCLSID = 0x1,
            Init_ByExeName = 0x2,
            Open_ByExeName = 0x2,
            Init_DefaultToStar = 0x4,
            Init_DefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }

        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic
        }

// ReSharper restore UnusedMember.Local
// ReSharper restore InconsistentNaming
    }
}
