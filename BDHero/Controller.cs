using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BDHero.Exceptions;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils;

namespace BDHero
{
    public class Controller : IController
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

        #region Events

        public event EventHandler ScanStart;
        public event EventHandler ScanSucceeded;
        public event EventHandler ScanFailed;
        public event EventHandler ScanCompleted;

        public event EventHandler ConvertStart;
        public event EventHandler ConvertSucceeded;
        public event EventHandler ConvertFailed;
        public event EventHandler ConvertCompleted;

        public event PluginProgressHandler PluginProgressUpdated;
        public event UnhandledExceptionEventHandler UnhandledException;

        private ProgressProviderUpdatedHandler _progressProviderUpdated;
        private TaskScheduler _callingScheduler;

        #endregion

        public Job Job { get; private set; }

        #region Constructors and initialization

        /// <summary>
        /// IMPORTANT: CONSTRUCTOR MUST BE THE FIRST THING CALLED WHEN THE PROGRAM STARTS UP TO INITIALIZE LOGGING!!!
        /// </summary>
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
            var entryAssembly = Assembly.GetEntryAssembly();
            var logConfigPath = Path.Combine(_configDir, logConfigFileName);
            log4net.GlobalContext.Properties["logdir"] = _logDir;
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigPath));
            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _logger.InfoFormat("{0} v{1} starting up", entryAssembly.GetName().Name, entryAssembly.GetName().Version);
            _logger.DebugFormat("Portable = {0}", _isPortable);
            _logger.DebugFormat("Config Dir = {0}", _configDir);
            _logger.DebugFormat("Plugin Dir = {0}", _pluginDir);
            _logger.DebugFormat("Log Dir = {0}", _logDir);
        }

        #endregion

        #region Plugin loading and logging

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
            _logger.InfoFormat("\t {0} ({1}){2}", name, plugins.Count, plugins.Any() ? ":" : "");
            foreach (var plugin in plugins)
            {
                _logger.InfoFormat("\t\t {0} v{1} - {2} - {3}", plugin.Name, plugin.AssemblyInfo.Version, plugin.AssemblyInfo.Guid, plugin.AssemblyInfo.Location);
            }
        }

        #endregion

        #region Stages

        public Task<bool> Scan(string bdromPath, string mkvPath = null)
        {
            if (ScanStart != null)
                ScanStart(this, EventArgs.Empty);

            return CreateStageTask(() => ReadBDROM(bdromPath), delegate
                {
                    GetMetadata();
                    AutoDetect();
                    Rename(mkvPath);
                }, ScanSucceed, ScanFail);
        }

        public Task<bool> Convert(string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            return CreateStageTask(Mux, PostProcess, ConvertSucceed, ConvertFail);
        }

        private Task<bool> CreateStageTask(Func<bool> criticalPhase, Action optionalPhases, Func<bool> succeed, Func<bool> fail)
        {
            // Get the calling thread's context
            _callingScheduler = SynchronizationContext.Current != null
                                    ? TaskScheduler.FromCurrentSynchronizationContext()
                                    : TaskScheduler.Default;

            var stageTask = new Task<bool>(delegate
            {
                var token = Task.Factory.CancellationToken;

                if (!criticalPhase())
                {
                    // It's possible to start a task directly on
                    // the UI thread, but not common...
                    var failTask = Task.Factory.StartNew(fail, token, TaskCreationOptions.None, _callingScheduler);

                    // Blocks until the task finishes executing
                    return failTask.Result;
                }

                optionalPhases();

                // It's possible to start a task directly on
                // the UI thread, but not common...
                var succeedTask = Task.Factory.StartNew(succeed, token, TaskCreationOptions.None, _callingScheduler);

                // Blocks until the task finishes executing
                return succeedTask.Result;
            });

            return stageTask;
        }

        #endregion

        #region Phases

        #region 1 - Disc Reader

        private bool ReadBDROM(string bdromPath)
        {
            IDiscReaderPlugin discReader = _pluginService.DiscReaderPlugins.First();
            return ExecutePlugin(discReader, () => Job = new Job(discReader.ReadBDROM(bdromPath)));
        }

        #endregion

        #region 2 - Metadata API Search

        private void GetMetadata()
        {
            foreach (var plugin in _pluginService.MetadataProviderPlugins)
            {
                GetMetadata(plugin);
            }
        }

        private void GetMetadata(IMetadataProviderPlugin plugin)
        {
            ExecutePlugin(plugin, () => plugin.GetMetadata(Job));
        }

        #endregion

        #region 3 - Auto Detect

        private void AutoDetect()
        {
            foreach (var plugin in _pluginService.AutoDetectorPlugins)
            {
                AutoDetect(plugin);
            }

            foreach (var playlist in Job.Disc.ValidMainFeaturePlaylists)
            {
                _logger.Info(playlist);
            }
        }

        private void AutoDetect(IAutoDetectorPlugin plugin)
        {
            ExecutePlugin(plugin, () => plugin.AutoDetect(Job));
        }

        #endregion

        #region 4 - Name Providers

        private void Rename(string mkvPath = null)
        {
            Job.OutputPath = mkvPath;
            foreach (var plugin in _pluginService.NameProviderPlugins)
            {
                Rename(plugin);
            }
        }

        private void Rename(INameProviderPlugin plugin)
        {
            ExecutePlugin(plugin, () => plugin.Rename(Job));
        }

        #endregion

        #region 5 - Mux

        private bool Mux()
        {
            if (_pluginService.MuxerPlugins.Any(muxer => !Mux(muxer)))
            {
                return false;
            }
            return _pluginService.MuxerPlugins.Count > 0;
        }

        private bool Mux(IMuxerPlugin muxer)
        {
            return ExecutePlugin(muxer, () => muxer.Mux(Job));
        }

        #endregion

        #region 6 - Post Process

        private void PostProcess()
        {
            foreach (var plugin in _pluginService.PostProcessorPlugins)
            {
                PostProcess(plugin);
            }
        }

        private void PostProcess(IPostProcessorPlugin plugin)
        {
            ExecutePlugin(plugin, () => plugin.PostProcess(Job));
        }

        #endregion

        #endregion

        #region Plugin runner

        /// <summary>
        /// Runs a plugin.  Reports the plugin's state and progress and handles any exceptions that it throws.
        /// </summary>
        /// <param name="plugin">Plugin to execute</param>
        /// <param name="handler">Delegate that will invoke the plugin's functionality (and may potentially throw an exception)</param>
        /// <returns>
        /// <code>true</code> if the plugin completed successfully;
        /// <code>false</code> if the plugin threw an exception, terminated abnormally, or was canceled.
        /// </returns>
        private bool ExecutePlugin(IPlugin plugin, ExecutePluginHandler handler)
        {
            var progressProvider = _pluginService.GetProgressProvider(plugin);

            progressProvider.Updated -= ProgressProviderOnUpdated;
            progressProvider.Updated += ProgressProviderOnUpdated;

            progressProvider.Reset();
            progressProvider.Start();

            try
            {
                handler();
                progressProvider.Succeed();
                return true;
            }
            catch (Exception e)
            {
                progressProvider.Error(e);
                HandleUnhandledException(plugin, e);
                return false;
            }
        }

        private void ProgressProviderOnUpdated(ProgressProvider progressProvider)
        {
            var token = Task.Factory.CancellationToken;
            Task.Factory.StartNew(delegate
                {
                    if (PluginProgressUpdated != null)
                        PluginProgressUpdated(progressProvider.Plugin, progressProvider);
                }, token, TaskCreationOptions.None, _callingScheduler);
        }

        #endregion

        #region Exception handling

        private void HandleUnhandledException(IPlugin plugin, Exception exception)
        {
            var message = string.Format("Unhandled exception was thrown by plugin \"{0}\"", plugin.Name);
            _logger.Error(message, exception);
            if (UnhandledException != null)
                UnhandledException(this, new UnhandledExceptionEventArgs(exception, false));
        }

        #endregion

        #region Event calling methods

        private bool ScanFail()
        {
            if (ScanFailed != null)
                ScanFailed(this, EventArgs.Empty);

            ScanComplete();

            return false;
        }

        private bool ScanSucceed()
        {
            if (ScanSucceeded != null)
                ScanSucceeded(this, EventArgs.Empty);

            ScanComplete();

            return true;
        }

        private void ScanComplete()
        {
            if (ScanCompleted != null)
                ScanCompleted(this, EventArgs.Empty);
        }

        private bool ConvertFail()
        {
            if (ConvertFailed != null)
                ConvertFailed(this, EventArgs.Empty);

            ConvertComplete();

            return false;
        }

        private bool ConvertSucceed()
        {
            if (ConvertSucceeded != null)
                ConvertSucceeded(this, EventArgs.Empty);

            ConvertComplete();

            return true;
        }

        private void ConvertComplete()
        {
            if (ConvertCompleted != null)
                ConvertCompleted(this, EventArgs.Empty);
        }

        #endregion
    }

    internal delegate void ExecutePluginHandler();

    internal delegate void ProgressProviderUpdatedHandler(ProgressProvider progressProvider);
}
