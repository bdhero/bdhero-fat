using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<string, int> _progressMap = new ConcurrentDictionary<string, int>();

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
            var token = cancellationToken;
            var readBDROMAction = new Func<bool>(() => ReadBDROM(token, bdromPath));
            var optionalPhases = new[] { CreateGetMetadataAction(token), CreateAutoDetectAction(token), CreateRenameAction(token, mkvPath) };
            return CreateStageTask(
                token,
                ScanStart,
                readBDROMAction,
                optionalPhases,
                ScanFail,
                ScanSucceed
            );
        }

        private Action CreateGetMetadataAction(CancellationToken cancellationToken)
        {
            return () => GetMetadata(cancellationToken);
        }

        private Action CreateAutoDetectAction(CancellationToken cancellationToken)
        {
            return () => AutoDetect(cancellationToken);
        }

        private Action CreateRenameAction(CancellationToken cancellationToken, string mkvPath = null)
        {
            return () => Rename(cancellationToken, mkvPath);
        }

        public Task<bool> CreateConvertTask(CancellationToken cancellationToken, string mkvPath = null)
        {
            if (!string.IsNullOrWhiteSpace(mkvPath))
                Job.OutputPath = mkvPath;

            var token = cancellationToken;
            var muxAction = new Func<bool>(() => Mux(token));
            var optionalpPhases = new Action[] { () => PostProcess(token) };
            return CreateStageTask(
                token,
                ConvertStart,
                muxAction,
                optionalpPhases,
                ConvertFail,
                ConvertSucceed
            );
        }

        private Task<bool> CreateStageTask(CancellationToken cancellationToken, Action beforeStart, Func<bool> criticalPhase, IEnumerable<Action> optionalphases, Action fail, Action succeed)
        {
            var canContinue = new Func<bool>(() => !cancellationToken.IsCancellationRequested);
            cancellationToken.Register(() => Logger.Warn("User canceled current operation"));
            return new TaskBuilder()
                .OnThread(_callbackScheduler)
                .CancelWith(cancellationToken)
                .BeforeStart(_ => beforeStart())
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        if (criticalPhase())
                        {
                            foreach (var phase in optionalphases.TakeWhile(phase => canContinue()))
                            {
                                phase();
                            }

                            if (canContinue())
                            {
                                invoker.InvokeOnUIThreadAsync(_ => succeed());
                                return;
                            }
                        }

                        invoker.InvokeOnUIThreadAsync(_ => fail());
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

        #region Plugin progress reporting

        private void ProgressProviderOnUpdated(ProgressProvider progressProvider)
        {
            if (PluginProgressUpdated != null)
            {
                // Marshal event back to UI thread
                new TaskBuilder().OnThread(_callbackScheduler)
                                 .BeforeStart(delegate(CancellationToken token)
                                     {
                                         var guid = progressProvider.Plugin.AssemblyInfo.Guid;
                                         var hashCode = progressProvider.GetHashCode();

                                         // Progress hasn't changed since last update
                                         if (_progressMap.ContainsKey(guid) && _progressMap[guid] == hashCode)
                                             return;

                                         _progressMap[guid] = hashCode;

                                         PluginProgressUpdated(progressProvider.Plugin, progressProvider);
                                     })
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

        private void ScanFail()
        {
            if (ScanFailed != null)
                ScanFailed(this, EventArgs.Empty);

            ScanComplete();
        }

        private void ScanSucceed()
        {
            if (ScanSucceeded != null)
                ScanSucceeded(this, EventArgs.Empty);

            ScanComplete();
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

        private void ConvertFail()
        {
            if (ConvertFailed != null)
                ConvertFailed(this, EventArgs.Empty);

            ConvertComplete();
        }

        private void ConvertSucceed()
        {
            if (ConvertSucceeded != null)
                ConvertSucceeded(this, EventArgs.Empty);

            ConvertComplete();
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
