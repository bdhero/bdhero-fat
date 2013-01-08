using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.tools;
using BDAutoMuxer.views;
using BrightIdeasSoftware;
using MediaInfoWrapper;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace BDAutoMuxer
{
    public partial class FormRemux : Form
    {
        private static readonly Padding LabelMargin = new Padding(3, 3, 3, 3);

        private MkvMerge _mkvMerge;

        private MediaInfo _inputM2TS;
        private MediaInfo _inputMKV;

        private readonly List<MediaInfo> _inputLPCM = new List<MediaInfo>();
        private readonly List<MediaInfo> _inputSubtitles = new List<MediaInfo>();

        private List<MIAudioTrack> _audioTracks = new List<MIAudioTrack>();
        private List<MISubtitleTrack> _subtitleTracks = new List<MISubtitleTrack>();

        private string _lastLPCMPath;
        private string _lastSubtitlePath;

        public FormRemux()
        {
            InitializeComponent();

            FormUtils.TextBox_EnableSelectAll(this);

            PopulateLPCM();
            PopulateSubtitles();

            objectListViewAudioTracks.DragSource = new SimpleDragSource();
            objectListViewAudioTracks.DropSink = new RearrangingDropSink(false);

            objectListViewSubtitleTracks.DragSource = new SimpleDragSource();
            objectListViewSubtitleTracks.DropSink = new RearrangingDropSink(false);

            /*
            .sup = S_HDMV/PGS  - Blu-ray
            .idx = S_VOBSUB    - DVD      (.sub = companion file)
            .srt = S_TEXT/UTF8 - Matroska
             */
        }

        ~FormRemux()
        {
            CancelRemux();
        }

        private void FormRemux_Load(object sender, EventArgs e)
        {
            progressLabel.Text = "";
            statusStripProgressBar.Visible = false;

            InitToolTips();
            InitAudioListView();
            InitSubtitleListView();
            PopulateCheckboxes();
            PopulateTracks();
        }

        private void InitToolTips()
        {
            new ToolTip().SetToolTip(linkLabelAddLPCM, "Add one or more external LPCM audio tracks (.WAV files)");
            new ToolTip().SetToolTip(linkLabelClearLPCM, "Remove all external LPCM audio tracks");

            new ToolTip().SetToolTip(linkLabelAddSubtitles, "Add one or more external subtitle files (.sup, .idx/.sub, .srt)");
            new ToolTip().SetToolTip(linkLabelClearSubtitles, "Remove all external subtitles");
        }

        private void InitAudioListView()
        {
            var tlist = new TypedObjectListView<MIAVSTrack>(objectListViewAudioTracks);

            tlist.BooleanCheckStateGetter = x => x.IsSelected;
            tlist.BooleanCheckStatePutter = (x, newValue) => x.IsSelected = newValue;

            // Title
            tlist.GetColumn(0).AspectGetter = track => track.Title;
            tlist.GetColumn(0).AspectPutter = (track, value) => track.Title = value as string;

            // Codec
            tlist.GetColumn(1).AspectGetter = track => track.Codec.FullName;

            // Channels
            tlist.GetColumn(2).AspectGetter = ResolutionGetter;

            // Source
            tlist.GetColumn(3).AspectGetter =
                track =>
                _inputM2TS != null && _inputM2TS.Tracks.Contains(track)
                    ? "M2TS"
                    : _inputMKV != null && _inputMKV.Tracks.Contains(track)
                          ? "MKV"
                          : _inputLPCM.SelectMany(info => info.Tracks).Contains(track)
                                ? "LPCM"
                                : _inputSubtitles.SelectMany(info => info.Tracks).Contains(track) ? "Subs" : "Unknown";

            // Language
            tlist.GetColumn(4).AspectGetter = track => track.Language;

            // Default
            tlist.GetColumn(5).AspectGetter = track => track.IsDefault;
            tlist.GetColumn(5).AspectPutter = (track, value) => track.IsDefault = value as bool?;

            // Forced
            tlist.GetColumn(6).AspectGetter = track => track.IsForced;
            tlist.GetColumn(6).AspectPutter = (track, value) => track.IsForced = value as bool?;

            objectListViewAudioTracks_SelectedIndexChanged();
        }

        private void InitSubtitleListView()
        {
            var tlist = new TypedObjectListView<MIAVSTrack>(objectListViewSubtitleTracks);

            tlist.BooleanCheckStateGetter = x => x.IsSelected;
            tlist.BooleanCheckStatePutter = (x, newValue) => x.IsSelected = newValue;

            // Title
            tlist.GetColumn(0).AspectGetter = track => track.Title;
            tlist.GetColumn(0).AspectPutter = (track, value) => track.Title = value as string;

            // Codec
            tlist.GetColumn(1).AspectGetter = track => track.Codec.FullName;

            // Source
            tlist.GetColumn(2).AspectGetter =
                track =>
                _inputM2TS != null && _inputM2TS.Tracks.Contains(track)
                    ? "M2TS"
                    : _inputMKV != null && _inputMKV.Tracks.Contains(track)
                          ? "MKV"
                          : _inputLPCM.SelectMany(info => info.Tracks).Contains(track)
                                ? "LPCM"
                                : _inputSubtitles.SelectMany(info => info.Tracks).Contains(track) ? "Subs" : "Unknown";

            // Language
            tlist.GetColumn(3).AspectGetter = track => track.Language;

            // Default
            tlist.GetColumn(4).AspectGetter = track => track.IsDefault;
            tlist.GetColumn(4).AspectPutter = (track, value) => track.IsDefault = value as bool?;

            // Forced
            tlist.GetColumn(5).AspectGetter = track => track.IsForced;
            tlist.GetColumn(5).AspectPutter = (track, value) => track.IsForced = value as bool?;

            objectListViewSubtitleTracks_SelectedIndexChanged();
        }

        private static object ResolutionGetter(MIAVSTrack track)
        {
            var videoTrack = track as MIVideoTrack;
            var audioTrack = track as MIAudioTrack;
            if (videoTrack != null)
                return videoTrack.DisplayResolution;
            if (audioTrack != null)
                return audioTrack.Channels.ToString("0.0");
            return null;
        }

        #region LPCM UI

        private void AddLPCM(IEnumerable<string> paths)
        {
            ScanFiles(paths, _inputLPCM, (sender, args) => PopulateLPCM());
        }

        private void PopulateLPCM()
        {
            panelInputLPCM.Controls.Clear();
            if (_inputLPCM.Any() && _inputLPCM.SelectMany(info => info.AudioTracks).Any())
            {
                var count = _inputLPCM.Count;
                var plural = count == 1 ? "" : "s";
                var channelSet = new HashSet<double>(_inputLPCM.SelectMany(info => info.AudioTracks).Select(track => track.Channels));
                var strChannels = string.Join(", ", channelSet.OrderByDescending(x => x).Select(x => string.Format("{0} ch", x)));

                var label = new Label();
                label.AutoSize = true;
                label.Text = string.Format("{0} track{1} ({2})", count, plural, strChannels);
                label.Margin = LabelMargin;

                panelInputLPCM.Controls.Add(label);
                panelInputLPCM.Controls.Add(linkLabelAddLPCM);
                panelInputLPCM.Controls.Add(linkLabelEditLPCM);
                panelInputLPCM.Controls.Add(linkLabelClearLPCM);

                _lastLPCMPath = _inputLPCM.Last().FilePaths.First();
            }
            else
            {
                panelInputLPCM.Controls.Add(labelInputLPCMNone);
                panelInputLPCM.Controls.Add(linkLabelAddLPCM);
            }
        }

        #endregion

        #region Subtitles UI

        private static readonly Regex SubExtensionRegex = new Regex(@"\.sub", RegexOptions.IgnoreCase);

        private void AddSubtitles(IEnumerable<string> paths)
        {
            paths = paths.Select(path => SubExtensionRegex.Replace(path, ".idx"));
            ScanFiles(paths, _inputSubtitles, (sender, args) => PopulateSubtitles());
        }

        private void PopulateSubtitles()
        {
            panelInputSubtitles.Controls.Clear();
            if (_inputSubtitles.Any())
            {
                var count = _inputSubtitles.Count;
                var plural = count == 1 ? "" : "s";
                var extensionSet = new HashSet<string>(_inputSubtitles.SelectMany(info => info.FilePaths).Select(Path.GetExtension));
                var strExtensions = string.Join(", ", extensionSet);

                var label = new Label();
                label.AutoSize = true;
                label.Text = string.Format("{0} track{1} ({2})", count, plural, strExtensions);
                label.Margin = LabelMargin;

                panelInputSubtitles.Controls.Add(label);
                panelInputSubtitles.Controls.Add(linkLabelAddSubtitles);
                panelInputSubtitles.Controls.Add(linkLabelEditSubtitles);
                panelInputSubtitles.Controls.Add(linkLabelClearSubtitles);

                _lastSubtitlePath = _inputSubtitles.Last().FilePaths.Last();
            }
            else
            {
                panelInputSubtitles.Controls.Add(labelInputSubtitlesNone);
                panelInputSubtitles.Controls.Add(linkLabelAddSubtitles);
            }
        }

        #endregion

        #region File Browse Dialogs

        private List<string> BrowseFiles(string directory, string title, string fileTypeName, string fileExt, bool checkFileExists = true)
        {
            var paths = new List<string>();
            try
            {
                // Normalize "ext", ".ext", and "*.ext" to "*.ext"
                fileExt = Regex.Replace(fileExt.Trim(), @"\*?\.?(\w+)", "*.$1");

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.Filter = string.Format("{0} ({1})|{1}", fileTypeName, fileExt);
                dialog.Title = title;
                dialog.CheckFileExists = checkFileExists;
                dialog.InitialDirectory = directory;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    paths = dialog.FileNames.ToList();
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Error opening path(s) {0}: {1}{2}",
                    string.Join(", ", paths),
                    ex.Message,
                    Environment.NewLine);

                MessageBox.Show(this, msg, "BDAutoMuxer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return paths;
        }

        private void BrowseFile(Control textBox, string title, string fileTypeName, string fileExt, bool checkFileExists = true)
        {
            string path = null;
            try
            {
                // Normalize "ext", ".ext", and "*.ext" to "*.ext"
                fileExt = Regex.Replace(fileExt.Trim(), @"\*?\.?(\w+)", "*.$1");

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = string.Format("{0} ({1})|{1}", fileTypeName, fileExt);
                dialog.Title = title;
                dialog.CheckFileExists = checkFileExists;

                string cur = textBox.Text;
                if (!string.IsNullOrEmpty(cur))
                {
                    dialog.FileName = cur;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.FileName;
                    textBox.Text = path;
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Error opening path {0}: {1}{2}",
                    path,
                    ex.Message,
                    Environment.NewLine);

                MessageBox.Show(this, msg, "BDAutoMuxer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Background Worker

        private void mkvMerge_Started()
        {
            buttonRemux.Text = "Pause";
            buttonClose.Text = "Stop";
        }

        private CancelEventArgs PauseOrResume()
        {
            var e = new CancelEventArgs();

            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                mkvMerge_Started();

                if (_mkvMerge.IsPaused)
                {
                    _mkvMerge.Resume();
                }
                else
                {
                    _mkvMerge.Pause();
                    buttonRemux.Text = "Resume";
                }

                mkvMerge_ProgressChanged(this, null);

                e.Cancel = true;
            }

            return e;
        }

        private CancelEventArgs OverwriteExistingFile()
        {
            var e = new CancelEventArgs();

            if (File.Exists(textBoxOutputMkv.Text))
            {
                if (DialogResult.Yes != MessageBox.Show(this,
                    string.Format("Overwrite \"{0}\"?", textBoxOutputMkv.Text),
                    "File already exists",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    e.Cancel = true;
                }
            }

            return e;
        }

        private CancelEventArgs CheckInvalidFilePaths()
        {
            var e = new CancelEventArgs();

            if (!ValidFilePaths)
            {
                string msg = "Whoops!  The programmer made a boo-boo :-(";
                TextBox tb = null;

                if (!IsInputM2tsFileValid)
                {
                    msg = "Please select a valid input M2TS file.";
                    tb = textBoxInputM2ts;
                }
                else if (!IsInputMkvFileValid)
                {
                    msg = "Please select a valid input MKV file.";
                    tb = textBoxInputMkv;
                }
                else if (!IsInputChapterFileValid)
                {
                    msg = "Please select a valid input chapter file.";
                    tb = textBoxInputChapters;
                }
                else if (!IsOutputMkvFileValid)
                {
                    msg = "Please enter a valid output MKV file.";
                    tb = textBoxOutputMkv;
                }

                MessageBox.Show(this, msg, BDAutoMuxerSettings.AssemblyName + " Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (tb != null)
                {
                    tb.Focus();
                    tb.SelectAll();
                }

                e.Cancel = true;
            }

            return e;
        }

        private void Remux()
        {
            if (PauseOrResume().Cancel)
                return;

            if (CheckInvalidFilePaths().Cancel)
                return;

            if (OverwriteExistingFile().Cancel)
                return;

            statusStripProgressBar.Visible = true;

            mkvMerge_Started();

            _mkvMerge = new MkvMerge(textBoxInputM2ts.Text, textBoxInputMkv.Text, textBoxInputChapters.Text, textBoxOutputMkv.Text /* , radioButtonUseM2tsAudio.Checked */)
                            {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            _mkvMerge.ProgressChanged += mkvMerge_ProgressChanged;
            _mkvMerge.RunWorkerCompleted += mkvMerge_RunWorkerCompleted;
            _mkvMerge.RunWorkerAsync();

            mkvMerge_ProgressChanged(this, null);
        }

        private bool ValidFilePaths
        {
            get
            {
                return
                    IsInputM2tsFileValid &&
                    IsInputMkvFileValid &&
                    IsInputChapterFileValid &&
                    IsOutputMkvFileValid;
            }
        }

        private bool IsInputM2tsFileValid
        {
            get { return File.Exists(textBoxInputM2ts.Text); }
        }

        private bool IsInputMkvFileValid
        {
            get { return File.Exists(textBoxInputMkv.Text); }
        }

        private bool IsInputChapterFileValid
        {
            // Allow chapter file to be optional, but if specified, it must be a valid file
            get { return File.Exists(textBoxInputChapters.Text) || string.IsNullOrEmpty(textBoxInputChapters.Text); }
        }

        private bool IsOutputMkvFileValid
        {
            get { return !string.IsNullOrEmpty(textBoxOutputMkv.Text); }
        }

        private void CancelRemux()
        {
            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                _mkvMerge.Resume();
                _mkvMerge.CancelAsync();
            }
        }

        private void mkvMerge_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_mkvMerge == null) return;

            var progress = (int)(10 * _mkvMerge.Progress);

            try
            {
                // TODO: This throws a NPE if the window is closed while muxing is in progress
                statusStripProgressBar.Value = progress;

                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Normal, Handle);
                TaskbarProgress.SetProgressValue(progress, 1000, Handle);
            }
            catch
            {
            }

            statusStripLabel.Text = _mkvMerge.Progress.ToString("##0.0") + "%";

            if (_mkvMerge.IsPaused)
                statusStripLabel.Text += " (paused)";
            if (_mkvMerge.IsError)
                statusStripLabel.Text += " (error)";
            if (_mkvMerge.IsCanceled)
                statusStripLabel.Text += " (canceled)";
            if (_mkvMerge.IsSuccess)
                statusStripLabel.Text += " (completed)";
        }

        private void mkvMerge_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonRemux.Text = "Mux!";
            buttonClose.Text = "Close";

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.NoProgress, Handle);

            statusStripProgressBar.Visible = false;

            if (_mkvMerge != null)
            {
                if (_mkvMerge.IsError)
                {
                    TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                    MessageBox.Show(this, _mkvMerge.ErrorMessage, "mkvmerge Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Event Handlers

        private void FormRemux_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && DragUtils.HasFileExtension(e, new[] { "m2ts", "mkv", "xml", "txt", "wav", "sup", "sub", "idx", "srt" }))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FormRemux_DragDrop(object sender, DragEventArgs e)
        {
            if (DragUtils.HasFileExtension(e, "m2ts"))
                SetInputM2TS(DragUtils.GetFirstFileWithExtension(e, "m2ts"));
            if (DragUtils.HasFileExtension(e, "mkv"))
                SetInputMKV(DragUtils.GetFirstFileWithExtension(e, "mkv"));
            if (DragUtils.HasFileExtension(e, new[] { "xml", "txt" }))
                SetInputChapters(DragUtils.GetFirstFileWithExtension(e, new[] { "xml", "txt" }));
            if (DragUtils.HasFileExtension(e, "wav"))
                AddLPCM(DragUtils.GetFilesWithExtension(e, "wav"));
            if (DragUtils.HasFileExtension(e, new[] { "sup", "sub", "idx", "srt" }))
                AddSubtitles(DragUtils.GetFilesWithExtension(e, new[] { "sup", "sub", "idx", "srt" }));
        }

        private bool FormEnabled
        {
            set { groupBoxInput.Enabled = groupBoxOutput.Enabled = buttonRemux.Enabled = value; }
        }

        private void PopulateCheckboxes()
        {
            panelKeepAudioFrom.Controls.Clear();
            panelKeepSubtitlesFrom.Controls.Clear();

            var hasM2TS = _inputM2TS != null;
            var hasMKV = _inputMKV != null;
            var hasLPCM = _inputLPCM.Any();
            var hasSubtitles = _inputSubtitles.Any();

            if (hasM2TS)
            {
                if (_inputM2TS.AudioTracks.Any())
                    panelKeepAudioFrom.Controls.Add(checkBoxKeepAudioFromM2TS);
                if (_inputM2TS.SubtitleTracks.Any())
                    panelKeepSubtitlesFrom.Controls.Add(checkBoxKeepSubtitlesFromM2TS);
            }
            if (hasMKV)
            {
                if (_inputMKV.AudioTracks.Any())
                    panelKeepAudioFrom.Controls.Add(checkBoxKeepAudioFromMKV);
                if (_inputMKV.SubtitleTracks.Any())
                    panelKeepSubtitlesFrom.Controls.Add(checkBoxKeepSubtitlesFromMKV);
            }
            if (hasLPCM)
            {
                panelKeepAudioFrom.Controls.Add(checkBoxKeepAudioFromLPCM);
            }
            if (hasSubtitles)
            {
                panelKeepSubtitlesFrom.Controls.Add(checkBoxKeepSubtitlesFromExternal);
            }
            if (panelKeepAudioFrom.Controls.Count == 0)
            {
                panelKeepAudioFrom.Controls.Add(labelKeepAudioFromNone);
            }
            if (panelKeepSubtitlesFrom.Controls.Count == 0)
            {
                panelKeepSubtitlesFrom.Controls.Add(labelKeepSubtitlesFromNone);
            }
        }

        private void PopulateTracks()
        {
            _audioTracks.Clear();
            _subtitleTracks.Clear();

            var hasM2TS = _inputM2TS != null;
            var hasMKV = _inputMKV != null;
            var hasLPCM = _inputLPCM.Any();
            var hasSubtitles = _inputSubtitles.Any();

            var keepAudioFromM2TS = checkBoxKeepAudioFromM2TS.Checked && panelKeepAudioFrom.Contains(checkBoxKeepAudioFromM2TS);
            var keepSubtitlesFromM2TS = checkBoxKeepSubtitlesFromM2TS.Checked && panelKeepSubtitlesFrom.Contains(checkBoxKeepSubtitlesFromM2TS);
            
            var keepAudioFromMKV = checkBoxKeepAudioFromMKV.Checked && panelKeepAudioFrom.Contains(checkBoxKeepAudioFromM2TS);
            var keepSubtitlesFromMKV = checkBoxKeepSubtitlesFromM2TS.Checked && panelKeepSubtitlesFrom.Contains(checkBoxKeepSubtitlesFromM2TS);
            
            var keepAudioFromLPCM = checkBoxKeepAudioFromLPCM.Checked && panelKeepAudioFrom.Contains(checkBoxKeepAudioFromLPCM);
            var keepSubtitlesFromExternal = checkBoxKeepSubtitlesFromExternal.Checked && panelKeepSubtitlesFrom.Contains(checkBoxKeepSubtitlesFromExternal);

            if (hasM2TS)
            {
                if (keepAudioFromM2TS)
                    _audioTracks.AddRange(_inputM2TS.AudioTracks);
                if (keepSubtitlesFromM2TS)
                    _subtitleTracks.AddRange(_inputM2TS.SubtitleTracks);
            }
            if (hasMKV)
            {
                if (keepAudioFromMKV)
                    _audioTracks.AddRange(_inputMKV.AudioTracks);
                if (keepSubtitlesFromMKV)
                    _subtitleTracks.AddRange(_inputMKV.SubtitleTracks);
            }
            if (hasLPCM && keepAudioFromLPCM)
            {
                _audioTracks.AddRange(_inputLPCM.SelectMany(info => info.AudioTracks));
            }
            if (hasSubtitles && keepSubtitlesFromExternal)
            {
                _subtitleTracks.AddRange(_inputSubtitles.SelectMany(info => info.SubtitleTracks));
            }

            objectListViewAudioTracks.SetObjects(_audioTracks);
            objectListViewSubtitleTracks.SetObjects(_subtitleTracks);
        }

        private void SetInputM2TS(string path)
        {
            textBoxInputM2ts.Text = path;
            ScanFile(path, out _inputM2TS, (sender, e) => PopulateInputLabels(_inputM2TS, panelInputM2tsAudio));
        }

        private void SetInputMKV(string path)
        {
            textBoxInputMkv.Text = path;
            ScanFile(path, out _inputMKV, (sender, e) => PopulateInputLabels(_inputMKV, panelInputMkvAudio));
        }

        private void SetInputChapters(string path)
        {
            textBoxInputChapters.Text = path;
        }

        private static void PopulateInputLabels(MediaInfo mediaInfo, Panel panel)
        {
            panel.Controls.Clear();

            if (mediaInfo.VideoTracks.Any())
            {
                var videoLabels = mediaInfo.VideoTracks.Select(FormatVideoLabel);
                var label = new Label();
                label.AutoSize = true;
                label.Text = string.Format("{0}", string.Join(", ", new HashSet<string>(videoLabels)));
                label.Margin = LabelMargin;
                panel.Controls.Add(label);
            }

            if (mediaInfo.AudioTracks.Any())
            {
                var audiolabels = mediaInfo.AudioTracks.Select(FormatAudioLabel);
                var label = new Label();
                label.AutoSize = true;
                label.Text = string.Format("{0}", string.Join(", ", new HashSet<string>(audiolabels)));
                label.Margin = LabelMargin;
                panel.Controls.Add(label);
            }

            if (mediaInfo.SubtitleTracks.Any())
            {
                var subtitleLabels = mediaInfo.SubtitleTracks.Select(FormatSubtitleLabel);
                var label = new Label();
                label.AutoSize = true;
                label.Text = string.Format("{0}", string.Join(", ", new HashSet<string>(subtitleLabels)));
                label.Margin = LabelMargin;
                panel.Controls.Add(label);
            }
        }

        private static string FormatVideoLabel(MIVideoTrack track)
        {
            return string.Format("{0} {1}", track.DisplayResolution, track.Codec.ShortName);
        }

        private static string FormatAudioLabel(MIAudioTrack track)
        {
            return string.Format("{0} ({1} ch)", track.Codec.ShortName, track.Channels);
        }

        private static string FormatSubtitleLabel(MISubtitleTrack track)
        {
            return string.Format("{0}", track.Codec.ShortName);
        }

        private void ScanFile(string path, out MediaInfo mediaInfo, RunWorkerCompletedEventHandler completedHandler)
        {
            FormEnabled = false;
            var mediaInfo2 = mediaInfo = new MediaInfo(path);
            var bgworker = new BackgroundWorker();
            bgworker.DoWork += (sender, args) => mediaInfo2.Scan();
            bgworker.RunWorkerCompleted += (sender, args) => { FormEnabled = true; PopulateCheckboxes(); PopulateTracks(); completedHandler(sender, args); };
            bgworker.RunWorkerAsync();
        }
        private void ScanFiles(IEnumerable<string> paths, ICollection<MediaInfo> mediaInfos, RunWorkerCompletedEventHandler completedHandler)
        {
            FormEnabled = false;
            var pathSet = new HashSet<string>(paths);
            pathSet.ExceptWith(mediaInfos.SelectMany(info => info.FilePaths));
            var bgworker = new BackgroundWorker();
            bgworker.DoWork += (sender, args) => mediaInfos.AddRange(pathSet.Select(path => new MediaInfo(path).Scan()));
            bgworker.RunWorkerCompleted += (sender, args) => { FormEnabled = true; PopulateCheckboxes(); PopulateTracks(); completedHandler(sender, args); };
            bgworker.RunWorkerAsync();
        }

        private void buttonInputM2tsBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxInputM2ts, "Select an Input M2TS File:", "BDAV Transport Stream", "m2ts");
        }

        private void buttonInputMkvBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxInputMkv, "Select an Input MKV File:", "Matroska Video", "mkv");
        }

        private void buttonInputChaptersBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxInputChapters, "Select an Input Chapter File:", "Matroska XML or OGM text", "txt;xml");
        }

        private void buttonOutputMkvBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxOutputMkv, "Select an Output MKV File:", "Matroska Video", "mkv", false);
        }

        private void linkLabelAddLPCM_Click(object sender, EventArgs e)
        {
            AddLPCM(BrowseFiles(Path.GetDirectoryName(_lastLPCMPath), "Select LPCM (WAVE) Files:", "LPCM (WAVE) Audio", "wav"));
        }

        private void linkLabelClearLPCM_Click(object sender, EventArgs e)
        {
            _inputLPCM.Clear();
            PopulateLPCM();
        }

        private void linkLabelAddSubtitles_Click(object sender, EventArgs e)
        {
            AddSubtitles(BrowseFiles(Path.GetDirectoryName(_lastSubtitlePath), "Select Subtitle Files:", "Subtitle File", "sup;idx;srt"));
        }

        private void linkLabelClearSubtitles_Click(object sender, EventArgs e)
        {
            _inputSubtitles.Clear();
            PopulateSubtitles();
        }

        #region ObjectListView track moving

        private void objectListViewAudioTracks_SelectedIndexChanged(object sender = null, EventArgs e = null)
        {
            ListViewSelectedIndexChanged(objectListViewAudioTracks, buttonMoveUpAudio, buttonMoveDownAudio);
        }

        private void objectListViewSubtitleTracks_SelectedIndexChanged(object sender = null, EventArgs e = null)
        {
            ListViewSelectedIndexChanged(objectListViewSubtitleTracks, buttonMoveUpSubtitles, buttonMoveDownSubtitles);
        }

        private void ListViewSelectedIndexChanged(ObjectListView listView, Button upButton, Button downButton)
        {
            var hasSelection = listView.SelectedIndices.Count > 0;
            var selectedIndex = listView.SelectedIndex;
            var itemCount = listView.GetItemCount();

            upButton.Enabled = hasSelection && selectedIndex > 0;
            downButton.Enabled = hasSelection && selectedIndex < itemCount - 1;
        }

        private void buttonMoveUpAudio_Click(object sender, EventArgs e)
        {
            MoveTrack(objectListViewAudioTracks, -1, 0);
        }

        private void buttonMoveDownAudio_Click(object sender, EventArgs e)
        {
            MoveTrack(objectListViewAudioTracks, +2, -1);
        }

        private void buttonMoveUpSubtitles_Click(object sender, EventArgs e)
        {
            MoveTrack(objectListViewSubtitleTracks, -1, 0);
        }

        private void buttonMoveDownSubtitles_Click(object sender, EventArgs e)
        {
            MoveTrack(objectListViewSubtitleTracks, +2, -1);
        }

        private static void MoveTrack(ObjectListView listView, int positionDelta, int selectionDelta)
        {
            var selectedIndex = listView.SelectedIndex;
            var selectedObjects = listView.SelectedObjects;
            var newIndex = selectedIndex + positionDelta;
            listView.MoveObjects(newIndex, selectedObjects);
            listView.SelectedIndex = newIndex + selectionDelta;
            listView.Focus();
        }

        #endregion

        #region Mux / Close Buttons

        private void buttonRemux_Click(object sender, EventArgs e)
        {
            Remux();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                if (DialogResult.Yes != MessageBox.Show(this, "Are you sure you want to cancal the remux?", "Cancel remux?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;

                CancelRemux();
            }
            else
            {
                Close();
            }
        }

        #endregion

        private void FormRemux_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                if (DialogResult.Yes != MessageBox.Show(this, "Are you sure you want to cancal the remux?", "Cancel remux?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    e.Cancel = true;
                    return;
                }

                CancelRemux();
            }
        }

        #endregion
    }

}
