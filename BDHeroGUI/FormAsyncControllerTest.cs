using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDHero;
using BDHero.Plugin;
using BDHero.Startup;
using DotNetUtils;
using OSUtils.TaskbarUtils;
using WindowsOSUtils.TaskbarUtils;

namespace BDHeroGUI
{
    public partial class FormAsyncControllerTest : Form
    {
        private readonly log4net.ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly PluginLoader _pluginLoader;
        private readonly IController _controller;
        private readonly ToolTip _progressBarToolTip;
        private readonly ITaskbarItem _taskbarItem;

        private CancellationTokenSource _scanCancellationTokenSource;
        private CancellationTokenSource _convertCancellationTokenSource;

        private bool _isRunning;

        private ProgressProviderState _state = ProgressProviderState.Ready;

        public FormAsyncControllerTest(IDirectoryLocator directoryLocator, PluginLoader pluginLoader, IController controller)
        {
            InitializeComponent();

            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _directoryLocator = directoryLocator;
            _pluginLoader = pluginLoader;
            _controller = controller;

            Load += OnLoad;

            _progressBarToolTip = new ToolTip();
            _progressBarToolTip.SetToolTip(progressBar, null);

            _taskbarItem = new WindowsTaskbarItemFactory().GetInstance(Handle);

            progressBar.UseCustomColors = true;
            progressBar.GenerateText = d => string.Format("{0}: {1:0.00}%", _state, d);
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            DoubleBuffered = true;
            LogDirectoryPaths();
            LoadPlugins();
            LogPlugins();
            InitController();
            EnableControls(true);
        }

        private void LogDirectoryPaths()
        {
            _logger.InfoFormat("IsPortable = {0}", _directoryLocator.IsPortable);
            _logger.InfoFormat("InstallDir = {0}", _directoryLocator.InstallDir);
            _logger.InfoFormat("ConfigDir = {0}", _directoryLocator.ConfigDir);
            _logger.InfoFormat("PluginDir = {0}", _directoryLocator.PluginDir);
            _logger.InfoFormat("LogDir = {0}", _directoryLocator.LogDir);
        }

        private void LoadPlugins()
        {
            _pluginLoader.LoadPlugins();
        }

        private void LogPlugins()
        {
            _pluginLoader.LogPlugins();
        }

        private void InitController()
        {
            _controller.ScanStarted += ControllerOnScanStarted;
            _controller.ScanSucceeded += ControllerOnScanSucceeded;
            _controller.ScanFailed += ControllerOnScanFailed;
            _controller.ScanCompleted += ControllerOnScanCompleted;

            _controller.ConvertStarted += ControllerOnConvertStarted;
            _controller.ConvertSucceeded += ControllerOnConvertSucceeded;
            _controller.ConvertFailed += ControllerOnConvertFailed;
            _controller.ConvertCompleted += ControllerOnConvertCompleted;

            _controller.PluginProgressUpdated += ControllerOnPluginProgressUpdated;
        }

        private void EnableControls(bool enabled)
        {
            textBoxInput.Enabled = enabled;
            textBoxOutput.Enabled = enabled;
            buttonMux.Enabled = enabled;
            buttonCancel.Enabled = !enabled;
            _isRunning = !enabled;
        }

        private void AppendStatus(string statusLine = null)
        {
            textBoxStatus.Text = (statusLine ?? string.Empty);
            _logger.Debug(statusLine);
        }

        private void ControllerOnScanStarted(object sender, EventArgs eventArgs)
        {
            textBoxStatus.Text = "Scan started...";
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnScanSucceeded(object sender, EventArgs eventArgs)
        {
            AppendStatus("Scan succeeded!");
            _taskbarItem.NoProgress();
        }

        private void ControllerOnScanFailed(object sender, EventArgs eventArgs)
        {
            AppendStatus("Scan failed!");
            _taskbarItem.Error();
        }

        private void ControllerOnScanCompleted(object sender, EventArgs eventArgs)
        {
            AppendStatus("Scan completed!");
            EnableControls(true);
        }

        private void ControllerOnConvertStarted(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert started...");
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnConvertSucceeded(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert succeeded!");
            _taskbarItem.NoProgress();
        }

        private void ControllerOnConvertFailed(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert failed!");
            _taskbarItem.Error();
        }

        private void ControllerOnConvertCompleted(object sender, EventArgs eventArgs)
        {
            AppendStatus("Convert completed!");
            EnableControls(true);
        }

        private void ControllerOnPluginProgressUpdated(IPlugin plugin, ProgressProvider progressProvider)
        {
            if (!_isRunning)
                return;

            _state = progressProvider.State;

            var percentCompleteStr = (progressProvider.PercentComplete/100.0).ToString("P");
            var line = string.Format("{0} is {1} - {2} complete - {3} - {4} elapsed, {5} remaining",
                                     plugin.Name, progressProvider.State, percentCompleteStr,
                                     progressProvider.Status,
                                     progressProvider.RunTime.ToStringShort(),
                                     progressProvider.TimeRemaining.ToStringShort());
            AppendStatus(line);

            progressBar.ValuePercent = progressProvider.PercentComplete;
            _progressBarToolTip.SetToolTip(progressBar, string.Format("{0}: {1}", progressProvider.State, percentCompleteStr));
            _taskbarItem.Progress = progressProvider.PercentComplete;

            switch (progressProvider.State)
            {
                case ProgressProviderState.Error:
                    progressBar.SetError();
                    _taskbarItem.Error();
                    break;
                case ProgressProviderState.Paused:
                    progressBar.SetPaused();
                    _taskbarItem.Pause();
                    break;
                case ProgressProviderState.Canceled:
                    progressBar.SetMuted();
                    _taskbarItem.NoProgress();
                    break;
                default:
                    progressBar.SetSuccess();
                    _taskbarItem.Normal();
                    break;
            }
        }

        private void buttonMux_Click(object sender, EventArgs e)
        {
            _scanCancellationTokenSource = new CancellationTokenSource();
            _convertCancellationTokenSource = new CancellationTokenSource();

            _controller.SetEventScheduler();

            var scanTask = _controller.CreateScanTask(_scanCancellationTokenSource.Token, textBoxInput.Text, textBoxOutput.Text);
            var convertTask = _controller.CreateConvertTask(_convertCancellationTokenSource.Token);

            scanTask.ContinueWith(delegate(Task task)
                {
                    if (task.IsCompleted && !_scanCancellationTokenSource.Token.IsCancellationRequested)
                        convertTask.Start();
                });
            scanTask.Start();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _scanCancellationTokenSource.Cancel();
            _convertCancellationTokenSource.Cancel();
        }
    }
}
