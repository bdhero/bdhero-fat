using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using BDAutoMuxer.controllers;
using Microsoft.Win32;

namespace BDAutoMuxer.Win32
{
    static partial class Win32
    {
        private const string DOUBLE_QUOTE = "\"";
        private const string NOT_DOUBLE_QUOTE = "[^\"]";

        /// <see cref="http://bytes.com/topic/net/answers/668513-determining-system-default-web-browser-windows-forms-app"/>
        public static string DefaultBrowserPath
        {
            get
            {
                using (var openSubKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false))
                {
                    if (openSubKey != null)
                    {
                        var value = openSubKey.GetValue(null) as string;
                        if (value != null)
                        {
                            // Extract file path.  E.G.:
                            // "C:\Some\File.exe" /arg %1 --> C:\Some\File.exe
                            var pattern = string.Format("{0}*{1}({0}*){1}.*", NOT_DOUBLE_QUOTE, DOUBLE_QUOTE);
                            var replace = Regex.Replace(value, pattern, "$1");
                            return replace;
                        }
                    }
                }

                return null;
            }
        }

        public static Icon DefaultBrowserIcon
        {
            get
            {
                var path = DefaultBrowserPath;
                return path != null ? FileUtils.ExtractIcon(path) : null;
            }
        }

        public static Bitmap DefaultBrowserIconAsBitmap
        {
            get
            {
                var icon = DefaultBrowserIcon;
                return icon != null ? icon.ToBitmap() : null;
            }
        }
    }
}
