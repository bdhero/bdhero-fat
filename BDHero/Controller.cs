using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.Serialization;
using System.Text;
using BDHero.Plugin;
using BDHero.Queue;
using ProcessUtils;

namespace BDHero
{
    public class Controller
    {
        private readonly PluginService _pluginService = new PluginService();

        public Job Job { get; private set; }

        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        public Controller()
        {
            LoadPlugins();
            VerifyPlugins();
        }

        #region Plugins

        private void LoadPlugins()
        {
            _pluginService.LoadPlugins();

            Console.WriteLine("Loaded {0} plugins:", _pluginService.Plugins.Count);
            ListPlugins("Disc Readers", _pluginService.DiscReaderPlugins);
            ListPlugins("Metadata Providers", _pluginService.MetadataProviderPlugins);
            ListPlugins("Auto Detectors", _pluginService.AutoDetectorPlugins);
            ListPlugins("Name Providers", _pluginService.NameProviderPlugins);
            ListPlugins("Muxers", _pluginService.MuxerPlugins);
            ListPlugins("Post Processors", _pluginService.PostProcessorPlugins);
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

        private static void ListPlugins<T>(string name, IList<T> plugins) where T : IPlugin
        {
            Console.WriteLine("\t {0} ({1}):", name, plugins.Count);
            foreach (var plugin in plugins)
            {
                Console.WriteLine("\t\t {0} - {1}", plugin.Name, plugin.GetType().Assembly.Location);
            }
        }

        #endregion

        public void Scan(string bdromPath)
        {
            IDiscReaderPlugin discReader = _pluginService.DiscReaderPlugins.First();
            discReader.ProgressUpdated += DiscReaderOnProgressUpdated;
            var disc = discReader.ReadBDROM(bdromPath);
            Job = new Job {Disc = disc};
        }

        private void DiscReaderOnProgressUpdated(IPlugin plugin, ProgressState progressState)
        {
            Console.WriteLine("Plugin {0} is {1}, {2}% complete", plugin.Name, progressState.ProcessState, progressState.PercentComplete.ToString("0.00"));
        }
    }

    /// <summary>
    /// Thrown when no instances of a required plugin could be found.
    /// </summary>
    public class RequiredPluginNotFoundException<T> : Exception where T : IPlugin
    {
        public RequiredPluginNotFoundException() : base("Required plugin " + typeof(T).Name + " not found")
        {
        }

        public RequiredPluginNotFoundException(string message) : base(message)
        {
        }

        public RequiredPluginNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
