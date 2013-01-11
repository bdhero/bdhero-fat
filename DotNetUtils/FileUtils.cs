using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUtils
{
    public static class FileUtils
    {
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
            return String.Format("{0:0" + fractionalFormat + "} {1}", len, sizes[order]);
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
            var multiBadCharRegex = new Regex(@"-(?:\s+-)+");
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
                    hasAssoc = !String.IsNullOrEmpty(defaultProgram) && File.Exists(defaultProgram);
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
