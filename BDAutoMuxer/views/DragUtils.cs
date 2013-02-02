using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils;
using DotNetUtils.Annotations;

// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace BDAutoMuxer.Views
{
    public static class DragUtils
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

        private static ICollection<string> NormalizeFileExtensions(ICollection<string> extensions)
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

        public static ICollection<string> GetPaths(DragEventArgs e)
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
            var enumerable = paths as List<string> ?? paths.ToList();
            return enumerable.Any() ? enumerable[0] : null;
        }

        public static ICollection<string> GetFilePaths(DragEventArgs e)
        {
            return GetPaths(e).Where(FileUtils.IsFile).ToArray();
        }

        public static string GetFirstFilePath(DragEventArgs e)
        {
            return GetPaths(e).FirstOrDefault(FileUtils.IsFile);
        }

        public static ICollection<string> GetDirectoryPaths(DragEventArgs e)
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
            var path = GetFirstFilePath(e);
            return path == null ? null : Path.GetFileNameWithoutExtension(path);
        }
    }

    public class ExternalDragProvider
    {
        private bool _leftMouseDown;
        private Point _startPos;
        private string _path;

        private readonly Control _dragSource;
        private readonly Control _droppableParent;

        private bool _droppableParentAllowDrop;

        /// <summary>
        /// Gets or sets a delegate that returns an absolute path to the control's currently selected file.
        /// </summary>
        public PathGetter PathGetter;

        /// <summary>
        /// Gets or sets the minimum number of pixels the mouse must move in either axis (X or Y) before a drag event is triggered.
        /// </summary>
        public uint Threshold = 2;

        public ExternalDragProvider([NotNull] Control dragSource, [CanBeNull] Control droppableParent)
        {
            _dragSource = dragSource;
            _dragSource.MouseDown += OnMouseDown;
            _dragSource.MouseMove += OnMouseMove;
            _dragSource.MouseUp += OnMouseUp;

            _droppableParent = droppableParent;
        }

        public static ExternalDragProvider Provide([NotNull] Control dragSource, [CanBeNull] Control droppableParent)
        {
            return new ExternalDragProvider(dragSource, droppableParent);
        }

        private bool HasPath
        {
            get { return (_path = PathGetter != null ? PathGetter(_dragSource) : null) != null; }
        }

        private void OnMouseDown(object sender, MouseEventArgs args)
        {
            _leftMouseDown = args.Button == MouseButtons.Left;

            if (!_leftMouseDown) return;
            if (!HasPath) return;

            _startPos = args.Location;
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (!_leftMouseDown) return;
            if (_path == null) return;
            if (Math.Abs(args.X - _startPos.X) < Threshold &&
                Math.Abs(args.Y - _startPos.Y) < Threshold) return;

            var paths = new StringCollection {_path};
            var dataObject = new DataObject();

            dataObject.SetFileDropList(paths);
            dataObject.SetText(_path);

            if (_droppableParent != null)
            {
                _droppableParentAllowDrop = _droppableParent.AllowDrop;
                if (_droppableParent.AllowDrop)
                    _droppableParent.AllowDrop = false;
            }

            _dragSource.DoDragDrop(dataObject, DragDropEffects.Copy);
        }

        private void OnMouseUp(object sender, MouseEventArgs args)
        {
            if (!_leftMouseDown) return;
            _leftMouseDown = false;
            if (_droppableParent == null) return;
            if (_droppableParentAllowDrop)
                _droppableParent.AllowDrop = true;
        }
    }

    public delegate string PathGetter(Control sender);
}
