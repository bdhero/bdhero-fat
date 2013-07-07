using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BDHeroGUI
{
    internal class SystemInfo
    {
        public static readonly SystemInfo Instance = new SystemInfo();

        public readonly Version OSVersionNumber;

        public readonly string OSVersionString;

        public readonly bool Is64BitOS;

        public readonly bool Is64BitProcess;

        public readonly int ProcessorCount;

        private SystemInfo()
        {
            var os = Environment.OSVersion;
            OSVersionNumber = os.Version;
            OSVersionString = os.VersionString;
            Is64BitOS = Environment.Is64BitOperatingSystem;
            Is64BitProcess = Environment.Is64BitProcess;
            ProcessorCount = Environment.ProcessorCount;
        }

        public override string ToString()
        {
            var fields = typeof (SystemInfo).GetFields(BindingFlags.Instance | BindingFlags.Public);
            return string.Join(Environment.NewLine,
                               fields.Select(info => string.Format("{0}: {1}", info.Name, info.GetValue(this))));
        }
    }
}
