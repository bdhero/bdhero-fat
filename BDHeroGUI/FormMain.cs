﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BDHero;
using BDHero.Plugin;
using BDHero.Startup;
using BDHero.Utils;
using BDHeroGUI.Annotations;
using BDHeroGUI.Forms;
using DotNetUtils;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;
using DotNetUtils.TaskUtils;
using OSUtils.DriveDetector;
using OSUtils.TaskbarUtils;
using WindowsOSUtils.TaskbarUtils;

namespace BDHeroGUI
{
    [UsedImplicitly]
    public partial class FormMain : Form
    {
        private readonly log4net.ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly PluginLoader _pluginLoader;
        private readonly IController _controller;
        private readonly IDriveDetector _driveDetector;

        private readonly ToolTip _progressBarToolTip;
        private readonly ITaskbarItem _taskbarItem;

        private bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        private ProgressProviderState _state = ProgressProviderState.Ready;

        #region Constructor and OnLoad

        public FormMain(IDirectoryLocator directoryLocator, PluginLoader pluginLoader, IController controller, IDriveDetector driveDetector)
        {
            InitializeComponent();

            Load += OnLoad;

            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _directoryLocator = directoryLocator;
            _pluginLoader = pluginLoader;
            _controller = controller;
            _driveDetector = driveDetector;

            _progressBarToolTip = new ToolTip();
            _progressBarToolTip.SetToolTip(progressBar, null);

            _taskbarItem = new WindowsTaskbarItemFactory().GetInstance(Handle);

            progressBar.UseCustomColors = true;
            progressBar.GenerateText = percentComplete => string.Format("{0}: {1:0.00}%", _state, percentComplete);

            playlistListView.ItemSelectionChanged += PlaylistListViewOnItemSelectionChanged;
            playlistListView.ShowAllChanged += PlaylistListViewOnShowAllChanged;

            mediaPanel.SelectedMediaChanged += MediaPanelOnSelectedMediaChanged;
            mediaPanel.Search = ShowMetadataSearchWindow;

            InitDriveDetector();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            Text += " v" + AppUtils.AppVersion;

            LogDirectoryPaths();
            LoadPlugins();
            LogPlugins();
            InitController();
            InitPluginMenu();

            EnableControls(true);
            splitContainerTop.Enabled = false;
            splitContainerMain.Enabled = false;

            this.EnableSelectAll();

            textBoxOutput.FileExtensions = new[]
                {
                    new FileExtension
                        {
                            Description = "Matroska video file",
                            Extensions = new[] {".mkv"}
                        }
                };

            // DriveDetector creates a NativeWindow in order to receive WM_DEVICECHANGE events from Windows.
            // Unfortunately that steals focus away from this window and causes it to appear in the background.
            // We need to explicitly activate this window to give it focus.
            Activate();
        }

        #endregion

        #region Initialization

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
            _controller.UnhandledException += ControllerOnUnhandledException;
        }

        private void InitPluginMenu()
        {
            Type prevPluginType = null;
            _controller.PluginsByType.ForEach(delegate(IPlugin plugin)
                {
                    var ifaces = plugin.GetType().GetInterfaces().Except(new[] {typeof(IPlugin)});
                    var curPluginType = ifaces.FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IPlugin)));
                    if (prevPluginType != null && prevPluginType != curPluginType)
                    {
                        pluginsToolStripMenuItem.DropDownItems.Add("-");
                    }

                    var iconImage16 = plugin.Icon != null ? new Icon(plugin.Icon, new Size(16, 16)).ToBitmap() : null;
                    var pluginItem = new ToolStripMenuItem(plugin.Name) { Image = iconImage16 };

                    pluginItem.DropDownItems.Add(
                        new ToolStripMenuItem("Enabled") { Enabled = false, Checked = plugin.Enabled });
                    pluginItem.DropDownItems.Add("-");
                    pluginItem.DropDownItems.Add(
                        new ToolStripMenuItem(string.Format("Version {0}", plugin.AssemblyInfo.Version)) { Enabled = false });
                    pluginItem.DropDownItems.Add(
                        new ToolStripMenuItem(string.Format("Built on {0}", plugin.AssemblyInfo.BuildDate)) { Enabled = false });

                    if (plugin.EditPreferences != null)
                    {
                        pluginItem.DropDownItems.Add(new ToolStripMenuItem("Preferences", null,
                                                                           (sender, args) =>
                                                                           plugin.EditPreferences(this)));
                    }

                    pluginsToolStripMenuItem.DropDownItems.Add(pluginItem);

                    prevPluginType = curPluginType;
                });
        }

        #endregion

        #region Disk Detection

        private void InitDriveDetector()
        {
            // TODO: Detect all drives on startup
            // TODO: Do this in background thread
            // TODO: Handle exceptions
            _driveDetector.DeviceArrived += DriveDetectorOnDeviceArrived;
            _driveDetector.DeviceRemoved += DriveDetectorOnDeviceRemoved;

            var bdromDrives = new DriveInfo[0];

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(delegate(IThreadInvoker invoker, CancellationToken token)
                    {
                        bdromDrives = DriveInfo.GetDrives().Where(BDFileUtils.IsBDROM).ToArray();
                    })
                .Succeed(delegate
                    {
                        bdromDrives.ForEach(AddBDROMMenuItem);
                    })
                .Build()
                .Start();
        }

        private void DriveDetectorOnDeviceArrived(object sender, DriveDetectorEventArgs args)
        {
            if (BDFileUtils.IsBDROM(args.DriveInfo))
            {
                AddBDROMMenuItem(args.DriveInfo);
            }
        }

        private void DriveDetectorOnDeviceRemoved(object sender, DriveDetectorEventArgs args)
        {
            RemoveBDROMMenuItem(args.DriveInfo);
        }

        private void AddBDROMMenuItem(DriveInfo driveInfo)
        {
            var text = string.Format("{0} {1}", driveInfo.Name, driveInfo.VolumeLabel);
            var item = new ToolStripMenuItem(text) {Tag = driveInfo.Name};
            item.Click += (o, e) => Scan(driveInfo.Name);

            openDiscToolStripMenuItem.DropDownItems.Add(item);
            noBlurayDiscsFoundToolStripMenuItem.Visible = false;
        }

        private void RemoveBDROMMenuItem(DriveInfo driveInfo)
        {
            var items = openDiscToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>().ToArray();
            var item = items.FirstOrDefault(menuItem => driveInfo.Name.Equals(menuItem.Tag));

            noBlurayDiscsFoundToolStripMenuItem.Visible = true;

            if (item != null)
            {
                openDiscToolStripMenuItem.DropDownItems.Remove(item);
            }

            var hasVisibleItems = items
                .Except(new[] { noBlurayDiscsFoundToolStripMenuItem })
                .Any(menuItem => menuItem.Visible);

            noBlurayDiscsFoundToolStripMenuItem.Visible = !hasVisibleItems;
        }

        #endregion

        #region Cancellation

        private CancellationTokenSource CreateCancellationTokenSource()
        {
            return _cancellationTokenSource = new CancellationTokenSource();
        }

        private bool IsCancellationRequested
        {
            get { return _cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested; }
        }

        private void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        #endregion

        #region Metadata search

        private void ShowMetadataSearchWindow()
        {
            var job = _controller.Job;

            if (job == null) return;

            var form = new FormMetadataSearch(job.SearchQuery);

            if (DialogResult.OK != form.ShowDialog(this)) return;

            job.SearchQuery = form.SearchQuery;

            _controller
                .CreateMetadataTask(CreateCancellationTokenSource().Token, GetMetadataStart, GetMetadataFail, GetMetadataSucceed)
                .Start()
                ;
        }

        private void GetMetadataStart()
        {
            buttonScan.Text = "Searching...";
            textBoxStatus.Text = "Searching for metadata...";
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void GetMetadataSucceed()
        {
            // TODO: Centralize button text
            buttonScan.Text = "Scan";
            textBoxOutput.Text = _controller.Job.OutputPath;
            AppendStatus("Metadata search completed!");
            _taskbarItem.NoProgress();
            RefreshUI();
            EnableControls(true);
        }

        private void GetMetadataFail()
        {
            // TODO: Centralize button text
            buttonScan.Text = "Scan";
            if (IsCancellationRequested)
            {
                AppendStatus("Metadata search canceled!");
                _taskbarItem.NoProgress();
            }
            else
            {
                AppendStatus("Metadata search failed!");
                _taskbarItem.Error();
            }
            EnableControls(true);
        }

        #endregion

        #region UI

        private void RefreshUI()
        {
            RefreshPlaylists();
            playlistListView.SelectFirstPlaylist();
            mediaPanel.Job = _controller.Job;
        }

        private void RefreshPlaylists()
        {
            if (_controller.Job != null)
                playlistListView.Playlists = _controller.Job.Disc.Playlists;
        }

        private void EnableControls(bool enabled)
        {
            var isPlaylistSelected = playlistListView.SelectedPlaylist != null;
            var hasJob = _controller.Job != null;

            openBDROMFolderToolStripMenuItem.Enabled = enabled;
            openDiscToolStripMenuItem.Enabled = enabled;
            searchForMetadataToolStripMenuItem.Enabled = enabled && hasJob;

            filterPlaylistsToolStripMenuItem.Enabled = enabled;
            showAllPlaylistsToolStripMenuItem.Enabled = enabled;
            filterTracksToolStripMenuItem.Enabled = enabled;
            showAllTracksToolStripMenuItem.Enabled = enabled;

//            optionsToolStripMenuItem.Enabled = enabled;

            textBoxInput.Enabled = enabled;
            buttonScan.Enabled = enabled;
            buttonCancelScan.Enabled = !enabled;
            rescanToolStripMenuItem.Enabled = enabled;

            textBoxOutput.Enabled = enabled;
            buttonConvert.Enabled = enabled && isPlaylistSelected;
            buttonCancelConvert.Enabled = !enabled && isPlaylistSelected;

            splitContainerTop.Enabled = enabled;
            splitContainerMain.Enabled = enabled;

            _isRunning = !enabled;
        }

        private void AppendStatus(string statusLine = null)
        {
            textBoxStatus.Text = (statusLine ?? string.Empty);
            _logger.Debug(statusLine);
        }

        #endregion

        #region Scan & Convert stages

        /// <summary>
        /// Asynchronously scans the selected BD-ROM folder.
        /// </summary>
        /// <param name="path">Optional path to the root BD-ROM folder.  If specified, the "Source BD-ROM" textbox will be populated with this path.</param>
        private void Scan(string path = null)
        {
            if (path != null)
                textBoxInput.Text = path;

            _controller.SetEventScheduler();

            // TODO: Let File Namer plugin handle this
            var outputDirectory = FileUtils.ContainsFileName(textBoxOutput.Text)
                                      ? Path.GetDirectoryName(textBoxOutput.Text)
                                      : textBoxOutput.Text;
            _controller
                .CreateScanTask(CreateCancellationTokenSource().Token, textBoxInput.Text, outputDirectory)
                .Start();
        }

        /// <summary>
        /// Asynchronously converts the BD-ROM to an MKV file according to the user's settings and selections.
        /// </summary>
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

            _controller.SetEventScheduler();

            _controller
                .CreateConvertTask(CreateCancellationTokenSource().Token, textBoxOutput.Text)
                .Start();
        }

        #endregion

        #region Scan events

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
            RefreshUI();
        }

        private void ControllerOnScanFailed(object sender, EventArgs eventArgs)
        {
            if (IsCancellationRequested)
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

        #region Convert events

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
            if (IsCancellationRequested)
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

        #region Progress events

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

        #region Exception handling

        private void ControllerOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var caption = string.Format("{0} Error", AppUtils.AppName);
            MessageBox.Show(this, args.ExceptionObject.ToString(), caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region UI events - ToolStrip MenuItems

        private void openBDROMFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxInput.Browse();
        }

        private void rescanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void searchForMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMetadataSearchWindow();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showAllPlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistListView.ShowAll = !playlistListView.ShowAll;
        }

        private void filterPlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistListView.ShowFilterWindow();
        }

        private void filterTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tracksPanel.ShowFilterWindow();
        }

        private void showAllTracksToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            tracksPanel.ShowAll = showAllTracksToolStripMenuItem.Checked;
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppUtils.ProjectHomepage);
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppUtils.DocumentationUrl);
        }

        private void submitABugReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppUtils.BugReportUrl);
        }

        private void suggestAFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppUtils.SuggestFeatureUrl);
        }

        private void showLogFileInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenFolder(_directoryLocator.LogDir);
        }

        private void aboutBDHeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        #endregion

        #region UI events - button clicks

        private void buttonScan_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            Convert();
        }

        private void buttonCancelConvert_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        #endregion

        #region UI events - selection change

        private void textBoxInput_SelectedPathChanged(object sender, EventArgs e)
        {
            Scan();
        }

        private void MediaPanelOnSelectedMediaChanged(object sender, EventArgs eventArgs)
        {
            _controller.RenameSync(textBoxOutput.Text);
            textBoxOutput.Text = _controller.Job.OutputPath;
        }

        private void PlaylistListViewOnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs listViewItemSelectionChangedEventArgs)
        {
            buttonConvert.Enabled = playlistListView.SelectedPlaylist != null;
            tracksPanel.Playlist = playlistListView.SelectedPlaylist;
        }

        private void PlaylistListViewOnShowAllChanged(object sender, EventArgs eventArgs)
        {
            showAllPlaylistsToolStripMenuItem.Checked = playlistListView.ShowAll;
        }

        #endregion

        #region Drag and Drop

        private static string GetFirstBDROMDirectory(DragEventArgs e)
        {
            return DragUtils.GetPaths(e).Select(BDFileUtils.GetBDROMDirectory).FirstOrDefault(s => s != null);
        }

        private bool AcceptBDROMDrop(DragEventArgs e)
        {
            return _state != ProgressProviderState.Running
                && _state != ProgressProviderState.Paused
                && GetFirstBDROMDirectory(e) != null
                ;
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (AcceptBDROMDrop(e))
            {
                e.Effect = DragDropEffects.All;
                textBoxInput.Highlight();
            }
            else
            {
                e.Effect = DragDropEffects.None;
                textBoxInput.UnHighlight();
            }
        }

        private void FormMain_DragLeave(object sender, EventArgs e)
        {
            textBoxInput.UnHighlight();
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            if (!AcceptBDROMDrop(e)) return;
            Scan(GetFirstBDROMDirectory(e));
        }

        #endregion
    }
}
