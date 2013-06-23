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
    public class LogInitializer
    {
        private readonly IDirectoryLocator _directoryLocator;

        public LogInitializer(IDirectoryLocator directoryLocator)
        {
            _directoryLocator = directoryLocator;
        }

        public LogInitializer Initialize(string logConfigFileName)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var logConfigPath = Path.Combine(_directoryLocator.ConfigDir, logConfigFileName);

            log4net.GlobalContext.Properties["logdir"] = _directoryLocator.LogDir;
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigPath));

            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var assemblyMeta = entryAssembly.GetName();

            logger.InfoFormat("{0} v{1} starting up", assemblyMeta.Name, assemblyMeta.Version);

            return this;
        }
    }
}
