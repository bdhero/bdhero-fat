using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Exceptions;
using BDHero.Plugin;

namespace BDHero.Startup
{
    public class Initializer
    {
        private static readonly IDictionary<string, Initializer> Instances
            = new Dictionary<string, Initializer>();

        public  DirectoryLocator DirectoryLocator { get; private set; }
        private LogInitializer   LogInitializer   { get; set; }
        public  PluginService    PluginService    { get; private set; }
        private PluginLoader     PluginLoader     { get; set; }

        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        public static Initializer GetInstance(string logConfigFileName)
        {
            if (Instances.ContainsKey(logConfigFileName))
                return Instances[logConfigFileName];

            var directoryLocator = DirectoryLocator.Instance;
            var logInitializer = new LogInitializer(logConfigFileName, directoryLocator);
            var pluginService = new PluginService();
            var pluginLoader = new PluginLoader(pluginService, directoryLocator);

            var initializer = new Initializer
                {
                    DirectoryLocator = directoryLocator,
                    LogInitializer = logInitializer,
                    PluginService = pluginService,
                    PluginLoader = pluginLoader
                };

            Instances[logConfigFileName] = initializer;

            return Instances[logConfigFileName];
        }
    }
}
