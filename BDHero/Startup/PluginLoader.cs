using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Exceptions;
using BDHero.Plugin;
using DotNetUtils.Annotations;

namespace BDHero.Startup
{
    public class PluginLoader
    {
        private readonly log4net.ILog _logger;
        private readonly PluginService _pluginService;
        private readonly IDirectoryLocator _directoryLocator;

        [UsedImplicitly]
        public PluginLoader(PluginService pluginService, IDirectoryLocator directoryLocator)
        {
            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _pluginService = pluginService;
            _directoryLocator = directoryLocator;
        }

        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        public void LoadPlugins()
        {
            LoadPluginsFromService();
            VerifyPlugins();
        }

        private void LoadPluginsFromService()
        {
            _pluginService.UnloadPlugins();
            _pluginService.LoadPlugins(_directoryLocator.RequiredPluginDir);
            _pluginService.LoadPlugins(_directoryLocator.CustomPluginDir);
        }

        /// <summary>Checks that all required plugin types are loaded.</summary>
        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        private void VerifyPlugins()
        {
            if (_pluginService.DiscReaderPlugins.Count == 0)
                throw new RequiredPluginNotFoundException<IDiscReaderPlugin>("A Disc Reader plugin is required");
            if (_pluginService.MuxerPlugins.Count == 0)
                throw new RequiredPluginNotFoundException<IMuxerPlugin>("A Muxer plugin is required");
        }

        public void LogPlugins()
        {
            _logger.InfoFormat("Loaded {0} plugins:", _pluginService.Plugins.Count);
            LogPlugins("Disc Readers", _pluginService.DiscReaderPlugins);
            LogPlugins("Metadata Providers", _pluginService.MetadataProviderPlugins);
            LogPlugins("Auto Detectors", _pluginService.AutoDetectorPlugins);
            LogPlugins("Name Providers", _pluginService.NameProviderPlugins);
            LogPlugins("Muxers", _pluginService.MuxerPlugins);
            LogPlugins("Post Processors", _pluginService.PostProcessorPlugins);
        }

        private void LogPlugins<T>(string name, IList<T> plugins) where T : IPlugin
        {
            _logger.InfoFormat("\t {0} ({1}){2}", name, plugins.Count, plugins.Any() ? ":" : "");
            foreach (var plugin in plugins)
            {
                _logger.InfoFormat("\t\t {0} v{1} - {2} - {3}", plugin.Name, plugin.AssemblyInfo.Version, plugin.AssemblyInfo.Guid, plugin.AssemblyInfo.Location);
            }
        }
    }
}
