using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BDAutoMuxer.controllers
{
    public class FileUtils
    {
        public static bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        public static bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static bool IsFileWritable1(string path)
        {
            FileStream stream = null;
            FileAttributes fileAttributes = File.GetAttributes(path);
            if ((fileAttributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
            {
                try { stream = File.OpenWrite(path); }
                catch (Exception ex) { ex.ToString(); }
            }
            return stream != null && stream.CanWrite;
        }

        /// <summary>
        /// Checks for write access for the given file.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <returns>true, if write access is allowed, otherwise false</returns>
        public static bool IsFileWritableRecursive(string filePath)
        {
            string curPath = filePath;
            if (!File.Exists(filePath))
            {
                while (!Directory.Exists(curPath = Path.GetDirectoryName(curPath))) ;
            }
            return IsFileWritable(curPath);
        }

        /// <summary>
        /// Checks for write access for the given file.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <returns>true, if write access is allowed, otherwise false</returns>
        public static bool IsFileWritable(string filePath)
        {
            if ((File.GetAttributes(filePath) & FileAttributes.ReadOnly) != 0)
                return false;

            // Get the access rules of the specified files (user groups and user names that have access to the file)
            var rules = File.GetAccessControl(filePath).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            // Get the identity of the current user and the groups that the user is in.
            var groups = WindowsIdentity.GetCurrent().Groups;
            string sidCurrentUser = WindowsIdentity.GetCurrent().User.Value;

            // Check if writing to the file is explicitly denied for this user or a group the user is in.
            if (rules.OfType<FileSystemAccessRule>().Any(r => (groups.Contains(r.IdentityReference) || r.IdentityReference.Value == sidCurrentUser) && r.AccessControlType == AccessControlType.Deny && (r.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData))
                return false;

            // Check if writing is allowed
            return rules.OfType<FileSystemAccessRule>().Any(r => (groups.Contains(r.IdentityReference) || r.IdentityReference.Value == sidCurrentUser) && r.AccessControlType == AccessControlType.Allow && (r.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData);
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

            bool success = GetDiskFreeSpaceEx(path, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);
            
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
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            string fractionalFormat = maxFractionalDigits > 0 ? "." + new string('#', maxFractionalDigits) : "";

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0" + fractionalFormat + "} {1}", len, sizes[order]);
        }

        /// <see cref="http://stackoverflow.com/questions/62771/how-check-if-given-string-is-legal-allowed-file-name-under-windows"/>
        public static bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
            return !containsABadCharacter.IsMatch(testName);
        }

        public static string SanitizeFileName(string fileName)
        {
            Regex badCharRegex = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
            Regex multiSpaceRegex = new Regex(@"\s{2,}");

            string sanitizedFileName = fileName;
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

        [DllImport("shell32.dll")]
        static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);

        public static bool HasProgramAssociation(string filePath)
        {
            bool hasAssoc = false;
            if (Path.HasExtension(filePath))
            {
                try
                {
                    string defaultProgram = FileExtentionInfo(AssocStr.Executable, Path.GetExtension(filePath));
                    hasAssoc = !String.IsNullOrEmpty(defaultProgram) && File.Exists(defaultProgram);
                }
                catch (Exception ex)
                {
                    ex.ToString();
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

            StringBuilder pszOut = new StringBuilder((int)pcchOut);
            AssocQueryString(AssocF.Verify, assocStr, doctype, null, pszOut, ref pcchOut);
            return pszOut.ToString();
        }

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
    }
}
