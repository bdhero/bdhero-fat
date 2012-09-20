using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace BDInfo.controllers
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

        public static string[] GetPaths(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return (string[])e.Data.GetData(DataFormats.FileDrop, false);
            }
            return new string[0];
        }

        public static string GetFirstFilePath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            foreach (string path in paths)
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == 0)
                    return path;
            }
            return null;
        }

        public static string GetFirstDirectoryPath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            foreach (string path in paths)
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return path;
            }
            return null;
        }

        public static string GetFirstPath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            return paths.Length > 0 ? paths[0] : null;
        }

        public static string GetFirstFileNameWithoutExtension(DragEventArgs e)
        {
            string path = DragUtils.GetFirstFilePath(e);
            return path == null ? null : Path.GetFileNameWithoutExtension(path);
        }
    }
}
