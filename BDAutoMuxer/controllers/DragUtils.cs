using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BDAutoMuxer.controllers
{
    public class DragUtils
    {
        public static bool HasFile(DragEventArgs e)
        {
            return GetFirstFilePath(e) != null;
        }

        public static bool HasDirectory(DragEventArgs e)
        {
            return GetFirstDirectoryPath(e) != null;
        }

        /// <summary>
        /// Returns true if at least one (>= 1) of the dragged files has an extension that matches
        /// at least one (>= 1) of the given file extensions.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="extension">File extension of the form ".ext" or "ext" (case insensitive)</param>
        /// <returns></returns>
        public static bool HasFileExtension(DragEventArgs e, string extension)
        {
            return HasFileExtension(e, new[] { extension });
        }

        /// <summary>
        /// Returns true if at least one (>= 1) of the dragged files has an extension that matches
        /// at least one (>= 1) of the given file extensions.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="extensions">Collection of file extensions of the form ".ext" or "ext" (case insensitive)</param>
        /// <returns></returns>
        public static bool HasFileExtension(DragEventArgs e, ICollection<string> extensions)
        {
            var normalizedExtensions = NormalizeFileExtensions(extensions);
            return GetFilePaths(e).Any(path => IsIn(normalizedExtensions, path));
        }

        public static IList<string> GetFilesWithExtension(DragEventArgs e, string extension)
        {
            return GetFilesWithExtension(e, new[] { extension });
        }

        public static IList<string> GetFilesWithExtension(DragEventArgs e, ICollection<string> extensions)
        {
            var normalizedExtensions = NormalizeFileExtensions(extensions);
            return GetFilePaths(e).Where(path => IsIn(normalizedExtensions, path)).ToList();
        }

        private static bool IsIn(ICollection<string> normalizedExtensions, string path)
        {
            var extension = Path.GetExtension(path);
            return extension != null && normalizedExtensions.Contains(extension.ToLower());
        }

        public static string GetFirstFileWithExtension(DragEventArgs e, string extension)
        {
            return GetFirstFileWithExtension(e, new[] { extension });
        }

        public static string GetFirstFileWithExtension(DragEventArgs e, ICollection<string> extensions)
        {
            var files = GetFilesWithExtension(e, extensions);
            return files.Count > 0 ? files[0] : null;
        }

        private static ICollection<string> NormalizeFileExtensions(IEnumerable<string> extensions)
        {
            return extensions.Select(NormalizeFileExtension).ToList();
        }

        private static string NormalizeFileExtension(string ext)
        {
            // Make sure every extension starts with a period (e.g., ".ext")
            var extNorm = ext.Trim().ToLower();
            if (!ext.StartsWith("."))
                extNorm = "." + extNorm;
            return extNorm;
        }

        public static string[] GetPaths(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return (string[])e.Data.GetData(DataFormats.FileDrop, false);
            }
            return new string[0];
        }

        public static string GetFirstPath(DragEventArgs e)
        {
            var paths = GetPaths(e);
            return paths.Length > 0 ? paths[0] : null;
        }

        public static string[] GetFilePaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsFile).ToArray();
        }

        public static string GetFirstFilePath(DragEventArgs e)
        {
            return GetPaths(e).FirstOrDefault(FileUtils.IsFile);
        }

        public static string[] GetDirectoryPaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsDirectory).ToArray();
        }

        public static string GetFirstDirectoryPath(DragEventArgs e)
        {
            var paths = GetPaths(e);
            return paths.FirstOrDefault(FileUtils.IsDirectory);
        }

        public static string GetFirstFileNameWithoutExtension(DragEventArgs e)
        {
            var path = DragUtils.GetFirstFilePath(e);
            return path == null ? null : Path.GetFileNameWithoutExtension(path);
        }
    }
}
