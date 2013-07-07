using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BDHeroGUI
{
    internal class SystemInfo
    {
        public static SystemInfo Instance
        {
            get { return _instance; }
        }

        private static readonly SystemInfo _instance = new SystemInfo();

        public readonly Version Version;

        public readonly string VersionString;

        public readonly bool Is64BitOS;

        public readonly bool Is64BitProcess;

        public readonly int ProcessorCount;

        public readonly long ProcessMemory;

        protected SystemInfo()
        {
            var os = Environment.OSVersion;
            Version = os.Version;
            VersionString = os.VersionString;
            Is64BitOS = Environment.Is64BitOperatingSystem;
            Is64BitProcess = Environment.Is64BitProcess;
            ProcessorCount = Environment.ProcessorCount;
            ProcessMemory = Environment.WorkingSet;
        }

        public override string ToString()
        {
            var fields = typeof (SystemInfo).GetFields(BindingFlags.Instance | BindingFlags.Public);
            return string.Join(Environment.NewLine,
                               fields.Select(info => string.Format("{0}: {1}", info.Name, info.GetValue(this))));
        }
    }
}
