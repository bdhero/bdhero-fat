using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BDHero.Startup
{
    class LogInitializer
    {
        private readonly log4net.ILog _logger;

        public LogInitializer(string logConfigFileName, DirectoryLocator directoryLocator)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var logConfigPath = Path.Combine(directoryLocator.ConfigDir, logConfigFileName);

            log4net.GlobalContext.Properties["logdir"] = directoryLocator.LogDir;
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigPath));

            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            _logger.InfoFormat("{0} v{1} starting up", entryAssembly.GetName().Name, entryAssembly.GetName().Version);
        }
    }
}
