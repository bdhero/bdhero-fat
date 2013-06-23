using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

// ReSharper disable ClassNeverInstantiated.Global
namespace BDHero.Startup
{
    public sealed class DirectoryLocator : IDirectoryLocator
    {
        private const string AppDataRootDirName = "BDHero";
        private const string ConfigDirName = "Config";
        private const string PluginDirName = "Plugins";
        private const string LogDirName = "Logs";

        public bool   IsPortable { get; private set; }
        public string InstallDir { get; private set; }
        public string ConfigDir  { get; private set; }
        public string PluginDir  { get; private set; }
        public string LogDir     { get; private set; }

        public DirectoryLocator()
        {
            InstallDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Debug.Assert(InstallDir != null, "InstallDir != null");

            IsPortable = Directory.Exists(Path.Combine(InstallDir, ConfigDirName));

            if (IsPortable)
            {
                ConfigDir = Path.Combine(InstallDir, ConfigDirName);
                PluginDir = Path.Combine(InstallDir, PluginDirName);
                LogDir = Path.Combine(InstallDir, LogDirName);
            }
            else
            {
                var roamingAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataRootDirName);
                var localAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRootDirName);
                ConfigDir = Path.Combine(roamingAppData, ConfigDirName);
                PluginDir = Path.Combine(roamingAppData, PluginDirName);
                LogDir = Path.Combine(localAppData, LogDirName);
            }

            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
        }
    }
}
