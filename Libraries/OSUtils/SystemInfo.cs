using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DotNetUtils;
using DotNetUtils.Annotations;

namespace OSUtils
{
// ReSharper disable MemberCanBePrivate.Global
    public class SystemInfo
    {
        public static readonly SystemInfo Instance = new SystemInfo();

        /// <summary>
        /// Gets information about the operating system.
        /// </summary>
        [UsedImplicitly]
        public readonly OS OS;

        /// <summary>
        /// Gets the width of memory addresses in bits (e.g., 32, 64).
        /// </summary>
        [UsedImplicitly]
        public readonly int MemoryWidth;

        /// <summary>
        /// Gets whether the current process is using 64-bit instructions and memory addresses.
        /// </summary>
        [UsedImplicitly]
        public readonly bool Is64BitProcess;

        /// <summary>
        /// Gets the number of logical processors on the CPU.  On Intel processors with hyperthreading,
        /// this value will be the number of cores multiplied by 2 (e.g., a quad core Intel Core i7
        /// would return 8).
        /// </summary>
        [UsedImplicitly]
        public readonly int ProcessorCount;

        private SystemInfo()
        {
            OS = GetOS();
            MemoryWidth = IntPtr.Size * 8;
            Is64BitProcess = Environment.Is64BitProcess;
            ProcessorCount = Environment.ProcessorCount;
        }

        private static OS GetOS()
        {
            var os = Environment.OSVersion;
            return new OS(GetOSType(), os.Version, os.VersionString, Environment.Is64BitOperatingSystem);
        }

        private static OSType GetOSType()
        {
            var id = Environment.OSVersion.Platform;
            var p = (int)id;
            if (PlatformID.Win32NT == id)
                return OSType.Windows;
            if ((p == 4) || (p == 6) || (p == 128))
                return GetNixOSType();
            return OSType.Other;
        }

        // From Managed.Windows.Forms/XplatUI
        [DllImport("libc")]
        private static extern int uname(IntPtr buf);

        /// <summary>
        /// On Unix-like systems, invokes the <c>uname()</c> function using native interop to detect the operating system type.
        /// </summary>
        /// <returns>The specific type of *Nix OS the application is running on</returns>
        /// <seealso cref="https://github.com/jpobst/Pinta/blob/master/Pinta.Core/Managers/SystemManager.cs"/>
        private static OSType GetNixOSType()
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
                        return OSType.Mac;
                    if (os == "Linux")
                        return OSType.Linux;
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
            return OSType.Unix;
        }

        public override string ToString()
        {
            return ReflectionUtils.ToString(this);
        }
    }
// ReSharper restore MemberCanBePrivate.Global
}
