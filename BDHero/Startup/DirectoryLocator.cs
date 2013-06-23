using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetUtils;

namespace BDHero.Startup
{
    public class DirectoryLocator
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

        DirectoryLocator()
        {
            InstallDir = AssemblyUtils.GetInstallDir(Assembly.GetEntryAssembly());
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

        public static DirectoryLocator Instance
        {
            get { return _instance ?? (_instance = new DirectoryLocator()); }
        }

        private static DirectoryLocator _instance;
    }
}
