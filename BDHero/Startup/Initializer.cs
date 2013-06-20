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

        public DirectoryLocator DirectoryLocator { get; private set; }
        private LogInitializer LogInitializer { get; set; }
        public PluginService PluginService { get; private set; }
        private PluginLoader PluginLoader { get; set; }

        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        public static Initializer GetInstance(string logConfigFileName)
        {
            if (Instances.ContainsKey(logConfigFileName))
                return Instances[logConfigFileName];

            var initializer = new Initializer();
            initializer.DirectoryLocator = DirectoryLocator.Instance;
            initializer.LogInitializer = new LogInitializer(logConfigFileName, initializer.DirectoryLocator);
            initializer.PluginService = new PluginService();
            initializer.PluginLoader = new PluginLoader(initializer.PluginService, initializer.DirectoryLocator);

            Instances[logConfigFileName] = initializer;

            return Instances[logConfigFileName];
        }
    }
}
