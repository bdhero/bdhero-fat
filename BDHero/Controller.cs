using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BDHero.Plugin;
using BDHero.JobQueue;

namespace BDHero
{
    public class Controller : IController
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly PluginService _pluginService;

        /// <summary>
        /// Needed for <see cref="ProgressProviderOnUpdated"/> to invoke progress update callbacks on the correct thread.
        /// </summary>
        private TaskScheduler _callbackScheduler;

        public Job Job { get; private set; }

        #region Events

        public event EventHandler ScanStarted;
        public event EventHandler ScanSucceeded;
        public event EventHandler ScanFailed;
        public event EventHandler ScanCompleted;

        public event EventHandler ConvertStarted;
        public event EventHandler ConvertSucceeded;
        public event EventHandler ConvertFailed;
        public event EventHandler ConvertCompleted;

        public event PluginProgressHandler PluginProgressUpdated;
        public event UnhandledExceptionEventHandler UnhandledException;

        #endregion

        public Controller(PluginService pluginService)
        {
            _pluginService = pluginService;
        }

        public void SetEventScheduler(TaskScheduler scheduler = null)
        {
            // Get the calling thread's context
            _callbackScheduler = scheduler ??
                                (SynchronizationContext.Current != null
                                     ? TaskScheduler.FromCurrentSynchronizationContext()
                                     : TaskScheduler.Default);
        }

        #region Stages

        public Task<bool> CreateScanTask(string bdromPath, string mkvPath = null)
        {
            return CreateStageTask(() => ReadBDROM(bdromPath), delegate
                {
                    GetMetadata();
                    AutoDetect();
                    Rename(mkvPath);
                }, ScanStart, ScanSucceed, ScanFail);
        }

        public Task<bool> CreateConvertTask(string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            return CreateStageTask(Mux, PostProcess, ConvertStart, ConvertSucceed, ConvertFail);
        }

        private Task<bool> CreateStageTask(Func<bool> criticalPhase, Action optionalPhases, Action start, Func<bool> succeed, Func<bool> fail)
        {
            var stageTask = new Task<bool>(delegate
            {
                var token = Task.Factory.CancellationToken;

                if (_callbackScheduler == null)
                {
                    throw new InvalidOperationException("No callback TaskScheduler has been set; set one with IController.SetEventScheduler().");
                }

                // It's possible to start a task directly on
                // the UI thread, but not common...
                var startTask = Task.Factory.StartNew(start, token, TaskCreationOptions.None, _callbackScheduler);
                startTask.Wait();

                if (!criticalPhase())
                {
                    // It's possible to start a task directly on
                    // the UI thread, but not common...
                    var failTask = Task.Factory.StartNew(fail, token, TaskCreationOptions.None, _callbackScheduler);

                    // Blocks until the task finishes executing
                    return failTask.Result;
                }

                optionalPhases();

                // It's possible to start a task directly on
                // the UI thread, but not common...
                var succeedTask = Task.Factory.StartNew(succeed, token, TaskCreationOptions.None, _callbackScheduler);

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
                Logger.Info(playlist);
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
                }, token, TaskCreationOptions.None, _callbackScheduler);
        }

        #endregion

        #region Exception handling

        private void HandleUnhandledException(IPlugin plugin, Exception exception)
        {
            var message = string.Format("Unhandled exception was thrown by plugin \"{0}\"", plugin.Name);
            Logger.Error(message, exception);
            if (UnhandledException != null)
                UnhandledException(this, new UnhandledExceptionEventArgs(exception, false));
        }

        #endregion

        #region Event calling methods

        private void ScanStart()
        {
            if (ScanStarted != null)
                ScanStarted(this, EventArgs.Empty);
        }

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

        private void ConvertStart()
        {
            if (ConvertStarted != null)
                ConvertStarted(this, EventArgs.Empty);
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
}
