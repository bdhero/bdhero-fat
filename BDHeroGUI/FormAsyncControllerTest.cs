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
using BDHero.BDROM;
using BDHero.Plugin;
using BDHero.Startup;
using BDHeroGUI.Forms;
using DotNetUtils;
using DotNetUtils.Extensions;
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

        private bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        private ProgressProviderState _state = ProgressProviderState.Ready;

        private readonly PlaylistFilter _filter;

        private bool _showAllPlaylists;

        public FormAsyncControllerTest(IDirectoryLocator directoryLocator, PluginLoader pluginLoader, IController controller)
        {
            InitializeComponent();

            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _directoryLocator = directoryLocator;
            _pluginLoader = pluginLoader;
            _controller = controller;

            _filter = new PlaylistFilter();

            Load += OnLoad;

            _progressBarToolTip = new ToolTip();
            _progressBarToolTip.SetToolTip(progressBar, null);

            _taskbarItem = new WindowsTaskbarItemFactory().GetInstance(Handle);

            progressBar.UseCustomColors = true;
            progressBar.GenerateText = d => string.Format("{0}: {1:0.00}%", _state, d);

            playlistListView.Filter = playlist => _filter.Show(playlist) || _showAllPlaylists;
            playlistListView.ItemSelectionChanged +=
                (sender, args) => buttonConvert.Enabled = playlistListView.SelectedPlaylist != null;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            LogDirectoryPaths();
            LoadPlugins();
            LogPlugins();
            InitController();
            EnableControls(true);
            this.EnableSelectAll();
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
            buttonScan.Enabled = enabled;
            buttonCancel.Enabled = !enabled;
            buttonConvert.Enabled = enabled && playlistListView.SelectedPlaylist != null;
            buttonCancelConvert.Enabled = !enabled && playlistListView.SelectedPlaylist != null;
            playlistListView.Enabled = enabled;
            _isRunning = !enabled;
        }

        private void AppendStatus(string statusLine = null)
        {
            textBoxStatus.Text = (statusLine ?? string.Empty);
            _logger.Debug(statusLine);
        }

        #region Scan stage - event handling

        private void ControllerOnScanStarted(object sender, EventArgs eventArgs)
        {
            buttonScan.Text = "Scanning...";
            textBoxStatus.Text = "Scan started...";
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnScanSucceeded(object sender, EventArgs eventArgs)
        {
            textBoxOutput.Text = _controller.Job.OutputPath;
            AppendStatus("Scan succeeded!");
            _taskbarItem.NoProgress();
            RefreshPlaylists();
        }

        private void ControllerOnScanFailed(object sender, EventArgs eventArgs)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                AppendStatus("Scan canceled!");
                _taskbarItem.NoProgress();
            }
            else
            {
                AppendStatus("Scan failed!");
                _taskbarItem.Error();
            }
        }

        private void ControllerOnScanCompleted(object sender, EventArgs eventArgs)
        {
            buttonScan.Text = "Scan";
            AppendStatus("Scan completed!");
            EnableControls(true);
        }

        #endregion

        #region Convert stage - event handling

        private void ControllerOnConvertStarted(object sender, EventArgs eventArgs)
        {
            buttonConvert.Text = "Converting...";
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
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                AppendStatus("Convert canceled!");
                _taskbarItem.NoProgress();
            }
            else
            {
                AppendStatus("Convert failed!");
                _taskbarItem.Error();
            }
        }

        private void ControllerOnConvertCompleted(object sender, EventArgs eventArgs)
        {
            buttonConvert.Text = "Convert";
            AppendStatus("Convert completed!");
            EnableControls(true);
        }

        #endregion

        #region Progress event handling

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

        #endregion

        private void RefreshPlaylists()
        {
            if (_controller.Job != null)
                playlistListView.Playlists = _controller.Job.Disc.Playlists;
        }

        private void Scan()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _controller.SetEventScheduler();

            _controller
                .CreateScanTask(_cancellationTokenSource.Token, textBoxInput.Text, textBoxOutput.Text)
                .Start();
        }

        private void Convert()
        {
            var selectedPlaylist = playlistListView.SelectedPlaylist;
            if (selectedPlaylist == null)
            {
                MessageBox.Show(this, "Please select a playlist to mux", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                return;
            }

            _controller.Job.SelectedPlaylistIndex = _controller.Job.Disc.Playlists.IndexOf(selectedPlaylist);

            _cancellationTokenSource = new CancellationTokenSource();

            _controller.SetEventScheduler();

            _controller
                .CreateConvertTask(_cancellationTokenSource.Token, textBoxOutput.Text)
                .Start();
        }

        private void buttonMux_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            Convert();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void buttonCancelConvert_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void linkLabelShowFilterWindow_Click(object sender, EventArgs e)
        {
            var result = new FormPlaylistFilter(_filter).ShowDialog(this);

            if (result == DialogResult.OK)
            {
                RefreshPlaylists();
            }
        }

        private void checkBoxShowAllPlaylists_CheckedChanged(object sender, EventArgs e)
        {
            _showAllPlaylists = checkBoxShowAllPlaylists.Checked;
            RefreshPlaylists();
        }
    }

    enum Stage
    {
        Ready,
        Scanning,
        Converting,
        Finished
    }
}
