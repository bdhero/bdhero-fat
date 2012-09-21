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
            return HasFileExtension(e, new string[] { extension });
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
            // Make sure ever extension starts with a period (e.g., ".ext")
            ISet<string> normalizedExtensions = new HashSet<string>();
            foreach (string ext in extensions)
            {
                string extNorm = ext.Trim().ToLower();
                if (!ext.StartsWith("."))
                    extNorm = "." + extNorm;
                normalizedExtensions.Add(extNorm);
            }

            foreach (string path in GetFilePaths(e))
            {
                if (normalizedExtensions.Contains(Path.GetExtension(path).ToLower()))
                    return true;
            }

            return false;
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
            string[] paths = GetPaths(e);
            return paths.Length > 0 ? paths[0] : null;
        }

        public static string[] GetFilePaths(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            IList<string> filePaths = new List<string>();
            foreach (string path in paths)
            {
                if (FileUtils.IsFile(path))
                    filePaths.Add(path);
            }
            return filePaths.ToArray();
        }

        public static string GetFirstFilePath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            foreach (string path in paths)
            {
                if (FileUtils.IsFile(path))
                    return path;
            }
            return null;
        }

        public static string[] GetDirectoryPaths(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            IList<string> directoryPaths = new List<string>();
            foreach (string path in paths)
            {
                if (FileUtils.IsDirectory(path))
                    directoryPaths.Add(path);
            }
            return directoryPaths.ToArray();
        }

        public static string GetFirstDirectoryPath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            foreach (string path in paths)
            {
                if (FileUtils.IsDirectory(path))
                    return path;
            }
            return null;
        }

        public static string GetFirstFileNameWithoutExtension(DragEventArgs e)
        {
            string path = DragUtils.GetFirstFilePath(e);
            return path == null ? null : Path.GetFileNameWithoutExtension(path);
        }
    }
}
