using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace BDHeroGUI
{
    public class SystemInfo
    {
        public static readonly SystemInfo Instance = new SystemInfo();

        public readonly OS OS;

        public readonly Version OSVersionNumber;

        public readonly string OSVersionString;

        public readonly bool Is64BitOS;

        public readonly bool Is64BitProcess;

        public readonly int ProcessorCount;

        private SystemInfo()
        {
            var os = Environment.OSVersion;
            OS = GetOS();
            OSVersionNumber = os.Version;
            OSVersionString = os.VersionString;
            Is64BitOS = Environment.Is64BitOperatingSystem;
            Is64BitProcess = Environment.Is64BitProcess;
            ProcessorCount = Environment.ProcessorCount;
        }

        private static OS GetOS()
        {
            var id = Environment.OSVersion.Platform;
            var p = (int)id;
            if (PlatformID.Win32NT == id)
                return OS.Windows;
            if ((p == 4) || (p == 6) || (p == 128))
                return GetNixOS();
            return OS.Other;
        }

        // From Managed.Windows.Forms/XplatUI
        [DllImport("libc")]
        static extern int uname(IntPtr buf);

        /// <summary>
        /// On Unix-like systems, invokes <c>uname</c> using native interop to detect the operating system.
        /// </summary>
        /// <returns>The specific type of *Nix OS the application is running in</returns>
        /// <seealso cref="https://github.com/jpobst/Pinta/blob/master/Pinta.Core/Managers/SystemManager.cs"/>
        static OS GetNixOS()
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname()
                if (uname(buf) == 0)
                {
                    var os = Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return OS.Mac;
                    if (os == "Linux")
                        return OS.Linux;
                }
            }
            catch
            {
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }
            return OS.Unix;
        }

        public override string ToString()
        {
            var fields = typeof(SystemInfo).GetFields(BindingFlags.Instance | BindingFlags.Public);
            return string.Join(Environment.NewLine,
                               fields.Select(info => string.Format("{0}: {1}", info.Name, info.GetValue(this))));
        }
    }

    public enum OS
    {
        /// <summary>
        /// Any version of Windows supported by .NET 4.0, from Windows XP to Windows 8.1 and beyond.
        /// </summary>
        Windows,

        /// <summary>
        /// Any version of Mac OS (a.k.a. Darwin) supported by Mono 3.2.
        /// </summary>
        Mac,

        /// <summary>
        /// Any version of Linux supported by Mono 3.2.
        /// </summary>
        Linux,

        /// <summary>
        /// Any version of UNIX (other than Linux or Mac) supported by Mono 3.2.
        /// </summary>
        Unix,

        /// <summary>
        /// Any other operating system not specified in this enum.
        /// </summary>
        Other
    }
}
