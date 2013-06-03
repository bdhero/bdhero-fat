using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils;
using ProcessUtils;

namespace BDHero
{
    public class Controller
    {
        private static log4net.ILog _logger;

        private const string AppDataRootDirName = "BDHero";
        private const string ConfigDirName = "Config";
        private const string PluginDirName = "Plugins";
        private const string LogDirName = "Logs";

        private bool _isPortable;
        private string _installDir;
        private string _configDir;
        private string _pluginDir;
        private string _logDir;

        private readonly PluginService _pluginService = new PluginService();

        public event EventHandler JobBeforeStart;
        public event EventHandler JobCompleted;
        public event EventHandler JobFailed;
        public event PluginExceptionEventHandler PluginException;
        public event UnhandledExceptionEventHandler UnhandledException;

        public Job Job { get; private set; }

        /// <summary>
        /// IMPORTANT: CONSTRUCTOR MUST BE THE FIRST THING CALLED WHEN THE PROGRAM STARTS UP TO INITIALIZE LOGGING!!!
        /// </summary>
        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        public Controller(string logConfigFileName)
        {
            LocateDirectories();
            InitLogging(logConfigFileName);
        }

        private void LocateDirectories()
        {
            _installDir = AssemblyUtils.GetInstallDir(Assembly.GetEntryAssembly());
            _isPortable = Directory.Exists(Path.Combine(_installDir, ConfigDirName));

            if (_isPortable)
            {
                _configDir = Path.Combine(_installDir, ConfigDirName);
                _pluginDir = Path.Combine(_installDir, PluginDirName);
                _logDir = Path.Combine(_installDir, LogDirName);
            }
            else
            {
                var roamingAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataRootDirName);
                var localAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRootDirName);
                _configDir = Path.Combine(roamingAppData, ConfigDirName);
                _pluginDir = Path.Combine(roamingAppData, PluginDirName);
                _logDir = Path.Combine(localAppData, LogDirName);
            }

            if (!Directory.Exists(_logDir))
            {
                Directory.CreateDirectory(_logDir);
            }
        }

        private void InitLogging(string logConfigFileName)
        {
            var logConfigPath = Path.Combine(_configDir, logConfigFileName);
            log4net.GlobalContext.Properties["logdir"] = _logDir;
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigPath));
            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _logger.Info("Starting up");
            _logger.DebugFormat("Portable = {0}", _isPortable);
            _logger.DebugFormat("Config Dir = {0}", _configDir);
            _logger.DebugFormat("Plugin Dir = {0}", _pluginDir);
            _logger.DebugFormat("Log Dir = {0}", _logDir);
        }

        #region Plugins

        public void LoadPlugins()
        {
            LoadPluginsFromService();
            LogPlugins();
            VerifyPlugins();
        }

        private void LoadPluginsFromService()
        {
            _pluginService.LoadPlugins(_pluginDir);
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

        private void LogPlugins()
        {
            _logger.InfoFormat("Loaded {0} plugins:", _pluginService.Plugins.Count);
            LogPlugins("Disc Readers", _pluginService.DiscReaderPlugins);
            LogPlugins("Metadata Providers", _pluginService.MetadataProviderPlugins);
            LogPlugins("Auto Detectors", _pluginService.AutoDetectorPlugins);
            LogPlugins("Name Providers", _pluginService.NameProviderPlugins);
            LogPlugins("Muxers", _pluginService.MuxerPlugins);
            LogPlugins("Post Processors", _pluginService.PostProcessorPlugins);
        }

        private static void LogPlugins<T>(string name, IList<T> plugins) where T : IPlugin
        {
            _logger.InfoFormat("\t {0} ({1}):", name, plugins.Count);
            foreach (var plugin in plugins)
            {
                _logger.InfoFormat("\t\t {0} v{1} - {2} - {3}", plugin.Name, plugin.AssemblyInfo.Version, plugin.AssemblyInfo.Guid, plugin.AssemblyInfo.Location);
            }
        }

        #endregion

        /// <summary>
        /// Scans a BD-ROM, retrieves metadata, auto-detects the type of each playlist and track, and renames tracks and output file names.
        /// </summary>
        /// <param name="bdromPath">Path to the BD-ROM directory</param>
        /// <returns><code>true</code> if the scan succeeded; otherwise <code>false</code></returns>
        public bool Scan(string bdromPath)
        {
            if (JobBeforeStart != null)
                JobBeforeStart(this, EventArgs.Empty);

            if (!ReadBDROM(bdromPath))
                return Fail();

            GetMetadata();
            AutoDetect();
            Rename();

            return true;
        }

        /// <summary>
        /// Muxes the BD-ROM to MKV and runs any post-processing plugins.
        /// </summary>
        /// <param name="mkvPath">Optional path to the output MKV file (if overridden by the user)</param>
        /// <returns><code>true</code> if all muxing plugins succeeded; otherwise <code>false</code></returns>
        public bool Convert(string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            if (!Mux())
                return Fail();

            PostProcess();

            if (JobCompleted != null)
                JobCompleted(this, EventArgs.Empty);

            return true;
        }

        #region 1 - Disc Reader

        private bool ReadBDROM(string bdromPath)
        {
            Console.WriteLine("Scanning {0}...", bdromPath);

            IDiscReaderPlugin discReader = _pluginService.DiscReaderPlugins.First();
            discReader.ProgressUpdated -= DiscReaderOnProgressUpdated;
            discReader.ProgressUpdated += DiscReaderOnProgressUpdated;

            try
            {
                var disc = discReader.ReadBDROM(bdromPath);
                Job = new Job(disc);
                return true;
            }
            catch (PluginException e)
            {
                HandlePluginException(discReader, e);
                return false;
            }
            catch (Exception e)
            {
                HandleUnhandledException(e);
                return false;
            }
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

        private bool Mux()
        {
            foreach (var plugin in _pluginService.MuxerPlugins)
            {
                try
                {
                    plugin.Mux(Job);
                }
                catch (PluginException e)
                {
                    HandlePluginException(plugin, e);
                    return false;
                }
                catch (Exception e)
                {
                    HandleUnhandledException(e);
                    return false;
                }
            }
            return _pluginService.MuxerPlugins.Count > 0;
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

        #region Exception handling

        private void HandlePluginException(IPlugin plugin, PluginException exception)
        {
            if (PluginException != null)
                PluginException(plugin, exception);
        }

        private void HandleUnhandledException(Exception exception)
        {
            if (UnhandledException != null)
                UnhandledException(this, new UnhandledExceptionEventArgs(exception, false));
        }

        private bool Fail()
        {
            if (JobFailed != null)
                JobFailed(this, EventArgs.Empty);
            return false;
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
