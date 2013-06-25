using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils.TaskUtils;

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

        public Task<bool> CreateScanTask(CancellationToken cancellationToken, string bdromPath, string mkvPath = null)
        {
            return new TaskBuilder()
                .OnThread(_callbackScheduler)
                .CancelWith(cancellationToken)
                .BeforeStart(_ => ScanStart())
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        var fail = new Action(() => invoker.InvokeOnUIThreadAsync(_ => ScanFail()));

                        if (ReadBDROM(token, bdromPath))
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                fail();
                                return;
                            }

                            GetMetadata(token);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                fail();
                                return;
                            }

                            AutoDetect(token);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                fail();
                                return;
                            }

                            Rename(token, mkvPath);

                            if (cancellationToken.IsCancellationRequested)
                            {
                                fail();
                                return;
                            }

                            invoker.InvokeOnUIThreadAsync(_ => ScanSucceed());
                        }
                        else
                        {
                            fail();
                        }
                    })
                .Build()
            ;
        }

        public Task<bool> CreateConvertTask(CancellationToken cancellationToken, string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            return new TaskBuilder()
                .OnThread(_callbackScheduler)
                .CancelWith(cancellationToken)
                .BeforeStart(_ => ConvertStart())
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        if (Mux(token) && !token.IsCancellationRequested)
                        {
                            PostProcess(token);

                            invoker.InvokeOnUIThreadAsync(_ => ConvertSucceed());
                        }
                        else
                        {
                            invoker.InvokeOnUIThreadAsync(_ => ConvertFail());
                        }
                    })
                .Build()
            ;
        }

        private Task<bool> CreatePluginTask(CancellationToken cancellationToken, IPlugin plugin, ExecutePluginHandler pluginRunner)
        {
            return new TaskBuilder()
                .OnThread(_callbackScheduler)
                .CancelWith(cancellationToken)
                .BeforeStart(delegate(CancellationToken token)
                    {
                        var progressProvider = _pluginService.GetProgressProvider(plugin);

                        progressProvider.Updated -= ProgressProviderOnUpdated;
                        progressProvider.Updated += ProgressProviderOnUpdated;

                        progressProvider.Reset();
                        progressProvider.Start();
                    })
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        pluginRunner(token);
                    })
                .Fail(delegate(Exception exception)
                    {
                        var progressProvider = _pluginService.GetProgressProvider(plugin);
                        if (exception is OperationCanceledException)
                        {
                            progressProvider.Cancel();
                        }
                        else
                        {
                            progressProvider.Error(exception);
                            HandleUnhandledException(exception);
                        }
                    })
                .Succeed(delegate
                    {
                        var progressProvider = _pluginService.GetProgressProvider(plugin);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            progressProvider.Cancel();
                        }
                        else
                        {
                            progressProvider.Succeed();
                        }
                    })
                .Build()
            ;
        }

        #endregion

        #region Phases

        #region 1 - Disc Reader

        private bool ReadBDROM(CancellationToken cancellationToken, string bdromPath)
        {
            IDiscReaderPlugin discReader = _pluginService.DiscReaderPlugins.First();
            var pluginTask = CreatePluginTask(cancellationToken, discReader, token => Job = new Job(discReader.ReadBDROM(token, bdromPath)));
            pluginTask.RunSynchronously();
            return pluginTask.IsCompleted && pluginTask.Result;
        }

        #endregion

        #region 2 - Metadata API Search

        private void GetMetadata(CancellationToken cancellationToken)
        {
            foreach (var plugin in _pluginService.MetadataProviderPlugins)
            {
                if (cancellationToken.IsCancellationRequested) return;
                GetMetadata(cancellationToken, plugin);
            }
        }

        private void GetMetadata(CancellationToken cancellationToken, IMetadataProviderPlugin plugin)
        {
            CreatePluginTask(cancellationToken, plugin, token => plugin.GetMetadata(token, Job)).RunSynchronously();
        }

        #endregion

        #region 3 - Auto Detect

        private void AutoDetect(CancellationToken cancellationToken)
        {
            foreach (var plugin in _pluginService.AutoDetectorPlugins)
            {
                if (cancellationToken.IsCancellationRequested) return;
                AutoDetect(cancellationToken, plugin);
            }

            foreach (var playlist in Job.Disc.ValidMainFeaturePlaylists)
            {
                Logger.Info(playlist);
            }
        }

        private void AutoDetect(CancellationToken cancellationToken, IAutoDetectorPlugin plugin)
        {
            CreatePluginTask(cancellationToken, plugin, token => plugin.AutoDetect(token, Job)).RunSynchronously();
        }

        #endregion

        #region 4 - Name Providers

        private void Rename(CancellationToken cancellationToken, string mkvPath = null)
        {
            Job.OutputPath = mkvPath;
            foreach (var plugin in _pluginService.NameProviderPlugins)
            {
                if (cancellationToken.IsCancellationRequested) return;
                Rename(cancellationToken, plugin);
            }
        }

        private void Rename(CancellationToken cancellationToken, INameProviderPlugin plugin)
        {
            CreatePluginTask(cancellationToken, plugin, token => plugin.Rename(token, Job)).RunSynchronously();
        }

        #endregion

        #region 5 - Mux

        private bool Mux(CancellationToken cancellationToken)
        {
            if (_pluginService.MuxerPlugins.Any(muxer => !Mux(cancellationToken, muxer)))
            {
                return false;
            }
            return _pluginService.MuxerPlugins.Count > 0;
        }

        private bool Mux(CancellationToken cancellationToken, IMuxerPlugin plugin)
        {
            var pluginTask = CreatePluginTask(cancellationToken, plugin, token => plugin.Mux(token, Job));
            pluginTask.RunSynchronously();
            return pluginTask.IsCompleted && pluginTask.Result;
        }

        #endregion

        #region 6 - Post Process

        private void PostProcess(CancellationToken cancellationToken)
        {
            foreach (var plugin in _pluginService.PostProcessorPlugins)
            {
                if (cancellationToken.IsCancellationRequested) return;
                PostProcess(cancellationToken, plugin);
            }
        }

        private void PostProcess(CancellationToken cancellationToken, IPostProcessorPlugin plugin)
        {
            CreatePluginTask(cancellationToken, plugin, token => plugin.PostProcess(token, Job)).RunSynchronously();
        }

        #endregion

        #endregion

        #region Plugin runner

        private void ProgressProviderOnUpdated(ProgressProvider progressProvider)
        {
            if (PluginProgressUpdated != null)
            {
                // Marshal event back to UI thread
                new TaskBuilder().OnThread(_callbackScheduler)
                                 .BeforeStart(token => PluginProgressUpdated(progressProvider.Plugin, progressProvider))
                                 .Build()
                                 .Start();
            }
        }

        #endregion

        #region Exception handling

        private void HandleUnhandledException(Exception exception)
        {
            var message = string.Format("Unhandled exception was thrown by plugin");
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

    internal delegate void ExecutePluginHandler(CancellationToken token);
}
