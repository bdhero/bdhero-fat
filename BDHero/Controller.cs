using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Plugin;
using BDHero.JobQueue;
using ProcessUtils;

namespace BDHero
{
    public class Controller
    {
        private readonly PluginService _pluginService = new PluginService();

        public event EventHandler JobBeforeStart;
        public event EventHandler JobCompleted;

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
            if (JobBeforeStart != null)
                JobBeforeStart(this, EventArgs.Empty);

            ReadBDROM(bdromPath);
            GetMetadata();
            AutoDetect();
            Rename();
        }

        public void Convert(string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            Mux();
            PostProcess();

            if (JobCompleted != null)
                JobCompleted(this, EventArgs.Empty);
        }

        #region 1 - Disc Reader

        private void ReadBDROM(string bdromPath)
        {
            Console.WriteLine("Scanning {0}...", bdromPath);
            IDiscReaderPlugin discReader = _pluginService.DiscReaderPlugins.First();
            discReader.ProgressUpdated -= DiscReaderOnProgressUpdated;
            discReader.ProgressUpdated += DiscReaderOnProgressUpdated;
            var disc = discReader.ReadBDROM(bdromPath);
            Job = new Job(disc);
        }

        private void DiscReaderOnProgressUpdated(IPlugin plugin, ProgressState progressState)
        {
            Console.Write("\rPlugin {0} is {1}, {2}% complete", plugin.Name, progressState.ProcessState, progressState.PercentComplete.ToString("0.00"));
            if (progressState.ProcessState == NonInteractiveProcessState.Completed)
                Console.WriteLine();
        }

        #endregion

        #region 2 - Metadata API Search

        private void GetMetadata()
        {
            foreach (var plugin in _pluginService.MetadataProviderPlugins)
            {
                plugin.GetMetadata(Job);
            }
        }

        #endregion

        #region 3 - Auto Detect

        private void AutoDetect()
        {
            foreach (var plugin in _pluginService.AutoDetectorPlugins)
            {
                plugin.AutoDetect(Job);
            }

            foreach (var playlist in Job.Disc.ValidMainFeaturePlaylists)
            {
                Console.WriteLine(playlist);
            }
        }

        #endregion

        #region 4 - Name Providers

        private void Rename()
        {
            foreach (var plugin in _pluginService.NameProviderPlugins)
            {
                plugin.Rename(Job);
            }
        }

        #endregion

        #region 5 - Mux

        private void Mux()
        {
            _pluginService.MuxerPlugins.First().Mux(Job);
        }

        #endregion

        #region 6 - Post Process

        private void PostProcess()
        {
            foreach (var plugin in _pluginService.PostProcessorPlugins)
            {
                plugin.PostProcess(Job);
            }
        }

        #endregion
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
