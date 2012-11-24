using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace BDAutoMuxer.controllers
{
    public static class FileUtils
    {
        public static Icon ExtractIcon(string exePath)
        {
            return Icon.ExtractAssociatedIcon(exePath);
        }

        public static Image ExtractIconAsBitmap(string exePath)
        {
            var icon = ExtractIcon(exePath);
            return icon != null ? icon.ToBitmap() : null;
        }

        /// <summary>
        /// Returns the absolute path to the default program associated with the given file type, or null if no association exists.
        /// </summary>
        /// <param name="fileNameOrExt"></param>
        /// <returns></returns>
        public static String GetDefaultProgram(string fileNameOrExt)
        {
            string program = null;
            string filePath = fileNameOrExt;
            if (!File.Exists(filePath))
            {
                var fileName = Path.GetFileName(filePath);
                var fileExt = fileName != null && fileName.Contains(".") ? Path.GetExtension(fileName) : "." + fileName;
                filePath = Path.Combine(Path.GetTempPath(), "empty" + fileExt);
                File.Create(filePath, 8, FileOptions.DeleteOnClose);
            }
            var process = new Process {StartInfo = {FileName = filePath, ErrorDialog = false}};
            try
            {
                process.Start();
                process.PriorityClass = ProcessPriorityClass.Idle;
                program = process.MainModule.FileName;
                process.Kill();
            }
            catch
            {
            }
            return program;
        }

        public static Icon GetDefaultProgramIcon(string fileNameOrExt)
        {
            var defaultProgram = GetDefaultProgram(fileNameOrExt);
            return defaultProgram != null ? ExtractIcon(defaultProgram) : null;
        }

        public static Image GetDefaultProgramIconAsBitmap(string filenameOrExt)
        {
            var defaultProgramIcon = GetDefaultProgramIcon(filenameOrExt);
            return defaultProgramIcon != null ? defaultProgramIcon.ToBitmap() : null;
        }

        public static bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        public static bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(
            string lpDirectoryName,
            out ulong lpFreeBytesAvailable,
            out ulong lpTotalNumberOfBytes,
            out ulong lpTotalNumberOfFreeBytes);

        /// <summary>
        /// Returns the amount of free disk space in bytes.
        /// Supports local and UNC paths.
        /// </summary>
        /// <see cref="http://pinvoke.net/default.aspx/kernel32.GetDiskFreeSpaceEx"/>
        public static ulong GetFreeSpace(string path)
        {
            ulong freeBytesAvailable;
            ulong totalNumberOfBytes;
            ulong totalNumberOfFreeBytes;

            var success = GetDiskFreeSpaceEx(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
            
            if (!success)
                throw new System.ComponentModel.Win32Exception();

            //Console.WriteLine("Free Bytes Available:      {0,15:D}", FreeBytesAvailable);
            //Console.WriteLine("Total Number Of Bytes:     {0,15:D}", TotalNumberOfBytes);
            //Console.WriteLine("Total Number Of FreeBytes: {0,15:D}", TotalNumberOfFreeBytes);

            return freeBytesAvailable;
        }

        public static string FormatFileSize(string filepath)
        {
            return FormatFileSize(new FileInfo(filepath).Length);
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
            var badCharRegex = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
            var multiSpaceRegex = new Regex(@"\s{2,}");

            var sanitizedFileName = fileName;
            sanitizedFileName = badCharRegex.Replace(sanitizedFileName, " - ");
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
                System.Diagnostics.Process.Start(filePath);
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

        [DllImport("shell32.dll")]
        static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);

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
