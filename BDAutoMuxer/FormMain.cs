//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.views;

namespace BDAutoMuxer
{
    public partial class FormMain : Form
    {
        private BDROM BDROM = null;
        private int CustomPlaylistCount = 0;
        private ScanBDROMResult ScanResult = new ScanBDROMResult();
        private ListViewColumnSorter PlaylistColumnSorter;

        #region Initialization

        public FormMain(string[] args)
        {
            InitializeComponent();

            FormUtils.TextBox_EnableSelectAll(this);

            PlaylistColumnSorter = new ListViewColumnSorter();
            listViewPlaylistFiles.ListViewItemSorter = PlaylistColumnSorter;
            if (args.Length > 0)
            {
                string path = args[0];
                textBoxSource.Text = path;
                InitBDROM(path);
            }
            else
            {
                textBoxSource.Text = BDAutoMuxerSettings.LastPath;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (BDAutoMuxerSettings.MainWindowMaximized)
                WindowState = FormWindowState.Maximized;

            if (BDAutoMuxerSettings.MainWindowLocation != Point.Empty)
                Location = BDAutoMuxerSettings.MainWindowLocation;

            if (BDAutoMuxerSettings.MainWindowSize != Size.Empty)
                Size = BDAutoMuxerSettings.MainWindowSize;

            this.Text = BDAutoMuxerSettings.AssemblyName + " v" + BDAutoMuxerSettings.AssemblyVersionDisplay;

            ResetColumnWidths();

            this.CheckForUpdates();
        }

        #endregion

        #region Event Handlers

        private void CheckForUpdates()
        {
            if (BDAutoMuxerSettings.CheckForUpdates)
            {
                UpdateNotifier.CheckForUpdate(this);
            }
        }

        private void textBoxSource_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSource.Text.Length > 0)
            {
                buttonRescan.Enabled = true;
            }
            else
            {
                buttonRescan.Enabled = false;
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] sources = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (sources.Length > 0)
            {
                string path = sources[0];
                textBoxSource.Text = path;
                InitBDROM(path);
            }
        }

        private void buttonBrowse_Click(
            object sender, 
            EventArgs e)
        {
            string path = null;
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "Select a Blu-ray BDMV Folder:";
                dialog.ShowNewFolderButton = false;
                if (!string.IsNullOrEmpty(textBoxSource.Text))
                {
                    dialog.SelectedPath = textBoxSource.Text;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.SelectedPath;
                    textBoxSource.Text = path;
                    InitBDROM(path);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Error opening path {0}: {1}{2}",
                    path,
                    ex.Message,
                    Environment.NewLine);

                MessageBox.Show(msg, "BDAutoMuxer Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRescan_Click(object sender, EventArgs e)
        {
            string path = textBoxSource.Text;
            try
            {
                InitBDROM(path);
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Error opening path {0}: {1}{2}",
                    path,
                    ex.Message,
                    Environment.NewLine);

                MessageBox.Show(msg, "BDAutoMuxer Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSettings_Click(
            object sender, 
            EventArgs e)
        {
            FormSettings settings = new FormSettings();
            settings.ShowDialog();
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewPlaylistFiles.Items)
            {
                item.Checked = true;
            }
        }

        private void buttonUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewPlaylistFiles.Items)
            {
                item.Checked = false;
            }
        }

        private void buttonCustomPlaylist_Click(
            object sender, 
            EventArgs e)
        {
            string name = string.Format(
                "USER.{0}", (++CustomPlaylistCount).ToString("D3"));

            FormPlaylist form = new FormPlaylist(name, BDROM, OnCustomPlaylistAdded);
            form.LoadPlaylists();
            form.Show();
        }

        public void OnCustomPlaylistAdded()
        {
            LoadPlaylists();
        }

        private void buttonScan_Click(
            object sender, 
            EventArgs e)
        {
            ScanBDROM();
        }

        private void buttonViewReport_Click(
            object sender, 
            EventArgs e)
        {
            GenerateReport();
        }

        private void listViewPlaylistFiles_SelectedIndexChanged(
            object sender, 
            EventArgs e)
        {
            LoadPlaylist();
        }

        private void listViewPlaylistFiles_ColumnClick(
            object sender, 
            ColumnClickEventArgs e)
        {
            if (e.Column == PlaylistColumnSorter.SortColumn)
            {
                if (PlaylistColumnSorter.Order == SortOrder.Ascending)
                {
                    PlaylistColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    PlaylistColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                PlaylistColumnSorter.SortColumn = e.Column;
                PlaylistColumnSorter.Order = SortOrder.Ascending;
            }
            listViewPlaylistFiles.Sort();
        }

        private void ResetColumnWidths()
        {
            listViewPlaylistFiles.Columns[0].Width =
                (int)(listViewPlaylistFiles.ClientSize.Width * 0.23);
            listViewPlaylistFiles.Columns[1].Width =
                (int)(listViewPlaylistFiles.ClientSize.Width * 0.08);
            listViewPlaylistFiles.Columns[2].Width =
                (int)(listViewPlaylistFiles.ClientSize.Width * 0.23);
            listViewPlaylistFiles.Columns[3].Width =
                (int)(listViewPlaylistFiles.ClientSize.Width * 0.23);
            listViewPlaylistFiles.Columns[4].Width =
                (int)(listViewPlaylistFiles.ClientSize.Width * 0.23);

            listViewStreamFiles.Columns[0].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.23);
            listViewStreamFiles.Columns[1].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.08);
            listViewStreamFiles.Columns[2].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.23);
            listViewStreamFiles.Columns[3].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.23);
            listViewStreamFiles.Columns[4].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.23);

            listViewStreams.Columns[0].Width =
                (int)(listViewStreams.ClientSize.Width * 0.25);
            listViewStreams.Columns[1].Width =
                (int)(listViewStreams.ClientSize.Width * 0.15);
            listViewStreams.Columns[2].Width =
                (int)(listViewStreams.ClientSize.Width * 0.15);
            listViewStreams.Columns[3].Width =
                (int)(listViewStreams.ClientSize.Width * 0.45);
        }

        private void buttonRip_Click(object sender, EventArgs e)
        {
            FindMainPlaylist();
        }

        private void buttonRemux_Click(object sender, EventArgs e)
        {
            new FormRemux().Show(this);
        }

        private void FormMain_FormClosing(
            object sender, 
            FormClosingEventArgs e)
        {
            BDAutoMuxerSettings.LastPath = textBoxSource.Text;
            if (WindowState == FormWindowState.Maximized)
            {
                BDAutoMuxerSettings.MainWindowLocation = RestoreBounds.Location;
                BDAutoMuxerSettings.MainWindowSize = RestoreBounds.Size;
                BDAutoMuxerSettings.MainWindowMaximized = true;
            }
            else
            {
                BDAutoMuxerSettings.MainWindowLocation = Location;
                BDAutoMuxerSettings.MainWindowSize = Size;
                BDAutoMuxerSettings.MainWindowMaximized = false;
            }
            BDAutoMuxerSettings.SaveSettings();

            if (InitBDROMWorker != null &&
                InitBDROMWorker.IsBusy)
            {
                InitBDROMWorker.CancelAsync();
            }
            if (ScanBDROMWorker != null &&
                ScanBDROMWorker.IsBusy)
            {
                ScanBDROMWorker.CancelAsync();
            }
            if (ReportWorker != null &&
                ReportWorker.IsBusy)
            {
                ReportWorker.CancelAsync();
            }
        }

        #endregion

        #region BDROM Initialization Worker

        private BackgroundWorker InitBDROMWorker = null;

        private void InitBDROM(
            string path)
        {
            ShowNotification("Please wait while we scan the disc...");

            CustomPlaylistCount = 0;
            buttonBrowse.Enabled = false;
            buttonRescan.Enabled = false;
            buttonSelectAll.Enabled = false;
            buttonUnselectAll.Enabled = false;
            buttonCustomPlaylist.Enabled = false;
            buttonScan.Enabled = false;
            buttonViewReport.Enabled = false;
            buttonRip.Enabled = false;
            textBoxDetails.Enabled = false;
            listViewPlaylistFiles.Enabled = false;
            listViewStreamFiles.Enabled = false;
            listViewStreams.Enabled = false;
            textBoxDetails.Clear();
            listViewPlaylistFiles.Items.Clear();
            listViewStreamFiles.Items.Clear();
            listViewStreams.Items.Clear();

            InitBDROMWorker = new BackgroundWorker();
            InitBDROMWorker.WorkerReportsProgress = true;
            InitBDROMWorker.WorkerSupportsCancellation = true;
            InitBDROMWorker.DoWork += InitBDROMWork;
            InitBDROMWorker.ProgressChanged += InitBDROMProgress;
            InitBDROMWorker.RunWorkerCompleted += InitBDROMCompleted;
            InitBDROMWorker.RunWorkerAsync(path);
        }

        private void InitBDROMWork(
            object sender, 
            DoWorkEventArgs e)
        {
            try
            {
                BDROM = new BDROM((string)e.Argument);
                BDROM.StreamClipFileScanError += new BDROM.OnStreamClipFileScanError(BDROM_StreamClipFileScanError);
                BDROM.StreamFileScanError += new BDROM.OnStreamFileScanError(BDROM_StreamFileScanError);
                BDROM.PlaylistFileScanError += new BDROM.OnPlaylistFileScanError(BDROM_PlaylistFileScanError);
                BDROM.Scan();
                e.Result = null;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        protected bool BDROM_PlaylistFileScanError(TSPlaylistFile playlistFile, Exception ex)
        {
            DialogResult result = MessageBox.Show(string.Format(
                "An error occurred while scanning the playlist file {0}.\n\nThe disc may be copy-protected or damaged.\n\nDo you want to continue scanning the playlist files?", playlistFile.Name), 
                "BDAutoMuxer Scan Error", MessageBoxButtons.YesNo);
            
            if (result == DialogResult.Yes) return true;
            else return false;
        }

        protected bool BDROM_StreamFileScanError(TSStreamFile streamFile, Exception ex)
        {
            DialogResult result = MessageBox.Show(string.Format(
                "An error occurred while scanning the stream file {0}.\n\nThe disc may be copy-protected or damaged.\n\nDo you want to continue scanning the stream files?", streamFile.Name),
                "BDAutoMuxer Scan Error", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) return true;
            else return false;
        }

        protected bool BDROM_StreamClipFileScanError(TSStreamClipFile streamClipFile, Exception ex)
        {
            DialogResult result = MessageBox.Show(string.Format(
                "An error occurred while scanning the stream clip file {0}.\n\nThe disc may be copy-protected or damaged.\n\nDo you want to continue scanning the stream clip files?", streamClipFile.Name),
                "BDAutoMuxer Scan Error", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) return true;
            else return false;
        }

        private void InitBDROMProgress(
            object sender, 
            ProgressChangedEventArgs e)
        {
        }

        private void InitBDROMCompleted(
            object sender, 
            RunWorkerCompletedEventArgs e)
        {
            HideNotification();

            if (e.Result != null)
            {
                string msg = string.Format(
                    "{0}", ((Exception)e.Result).Message);
                MessageBox.Show(msg, "BDAutoMuxer Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonBrowse.Enabled = true;
                buttonRescan.Enabled = true;
                return;
            }

            buttonBrowse.Enabled = true;
            buttonRescan.Enabled = true;
            buttonScan.Enabled = true;
            buttonSelectAll.Enabled = true;
            buttonUnselectAll.Enabled = true;
            buttonCustomPlaylist.Enabled = true;
            buttonViewReport.Enabled = true;
            buttonRip.Enabled = true;
            textBoxDetails.Enabled = true;
            listViewPlaylistFiles.Enabled = true;
            listViewStreamFiles.Enabled = true;
            listViewStreams.Enabled = true;
            progressBarScan.Value = 0;
            labelProgress.Text = "";
            labelTimeElapsed.Text = "00:00:00";
            labelTimeRemaining.Text = "00:00:00";

            textBoxSource.Text = BDROM.DirectoryRoot.FullName;

            textBoxDetails.Text += string.Format(
                "Detected BDMV Folder: {0} ({1}) {2}",
                BDROM.DirectoryBDMV.FullName,
                BDROM.VolumeLabel,
                Environment.NewLine);

            List<string> features = new List<string>();
            if (BDROM.Is50Hz)
            {
                features.Add("50Hz Content");
            }
            if (BDROM.IsBDPlus)
            {
                features.Add("BD+ Copy Protection");
            }
            if (BDROM.IsBDJava)
            {
                features.Add("BD-Java");
            }
            if (BDROM.Is3D)
            {
                features.Add("Blu-ray 3D");
            }
            if (BDROM.IsDBOX)
            {
                features.Add("D-BOX Motion Code");
            }
            if (BDROM.IsPSP)
            {
                features.Add("PSP Digital Copy");
            }
            if (features.Count > 0)
            {
                textBoxDetails.Text += "Detected Features: " + string.Join(", ", features.ToArray()) + Environment.NewLine;
            }

            textBoxDetails.Text += string.Format(
                "Disc Size: {0:N0} bytes{1}",
                BDROM.Size,
                Environment.NewLine);

            LoadPlaylists();
        }

        #endregion

        #region File/Stream Lists

        private void LoadPlaylists()
        {
            listViewPlaylistFiles.Items.Clear();
            listViewStreamFiles.Items.Clear();
            listViewStreams.Items.Clear();

            sortedPlaylists.Clear();

            if (BDROM == null) return;

            bool hasHiddenTracks = false;

            //Dictionary<string, int> playlistGroup = new Dictionary<string, int>();
            List<List<TSPlaylistFile>> groups = new List<List<TSPlaylistFile>>();

            TSPlaylistFile[] sortedPlaylistFiles = new TSPlaylistFile[BDROM.PlaylistFiles.Count];
            BDROM.PlaylistFiles.Values.CopyTo(sortedPlaylistFiles, 0);
            Array.Sort(sortedPlaylistFiles, ComparePlaylistFiles);

            foreach (TSPlaylistFile playlist1
                in sortedPlaylistFiles)
            {
                if (!playlist1.IsValid) continue;

                int matchingGroupIndex = 0;
                for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
                {
                    List<TSPlaylistFile> group = groups[groupIndex];
                    foreach (TSPlaylistFile playlist2 in group)
                    {
                        if (!playlist2.IsValid) continue;

                        foreach (TSStreamClip clip1 in playlist1.StreamClips)
                        {
                            foreach (TSStreamClip clip2 in playlist2.StreamClips)
                            {
                                if (clip1.Name == clip2.Name)
                                {
                                    matchingGroupIndex = groupIndex + 1;
                                    break;
                                }
                            }
                            if (matchingGroupIndex > 0) break;
                        }
                        if (matchingGroupIndex > 0) break;
                    }
                    if (matchingGroupIndex > 0) break;
                }
                if (matchingGroupIndex > 0)
                {
                    groups[matchingGroupIndex - 1].Add(playlist1);
                }
                else
                {
                    groups.Add(new List<TSPlaylistFile> { playlist1 });
                    //matchingGroupIndex = groups.Count;
                }
                //playlistGroup[playlist1.Name] = matchingGroupIndex;
            }

            for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
            {
                List<TSPlaylistFile> group = groups[groupIndex];
                group.Sort(ComparePlaylistFiles);

                foreach (TSPlaylistFile playlist in group)
                    //in BDROM.PlaylistFiles.Values)
                {
                    if (!playlist.IsValid) continue;

                    if (playlist.HasHiddenTracks)
                    {
                        hasHiddenTracks = true;
                    }

                    ListViewItem.ListViewSubItem playlistIndex =
                        new ListViewItem.ListViewSubItem();
                    playlistIndex.Text = (groupIndex + 1).ToString();
                    playlistIndex.Tag = groupIndex;

                    ListViewItem.ListViewSubItem playlistName =
                        new ListViewItem.ListViewSubItem();
                    playlistName.Text = playlist.Name;
                    playlistName.Tag = playlist.Name;

                    TimeSpan playlistLengthSpan =
                        new TimeSpan((long)(playlist.TotalLength * 10000000));
                    ListViewItem.ListViewSubItem playlistLength =
                        new ListViewItem.ListViewSubItem();
                    playlistLength.Text = string.Format(
                        "{0:D2}:{1:D2}:{2:D2}",
                        playlistLengthSpan.Hours,
                        playlistLengthSpan.Minutes,
                        playlistLengthSpan.Seconds);
                    playlistLength.Tag = playlist.TotalLength;

                    ListViewItem.ListViewSubItem playlistSize =
                        new ListViewItem.ListViewSubItem();
                    if (BDAutoMuxerSettings.EnableSSIF &&
                        playlist.InterleavedFileSize > 0)
                    {
                        playlistSize.Text = playlist.InterleavedFileSize.ToString("N0");
                        playlistSize.Tag = playlist.InterleavedFileSize;
                    }
                    else if (playlist.FileSize > 0)
                    {
                        playlistSize.Text = playlist.FileSize.ToString("N0");
                        playlistSize.Tag = playlist.FileSize;
                    }
                    else
                    {
                        playlistSize.Text = "-";
                        playlistSize.Tag = playlist.FileSize;
                    }                    

                    ListViewItem.ListViewSubItem playlistSize2 =
                        new ListViewItem.ListViewSubItem();
                    if (playlist.TotalAngleSize > 0)
                    {
                        playlistSize2.Text = (playlist.TotalAngleSize).ToString("N0");
                    }
                    else
                    {
                        playlistSize2.Text = "-";
                    }
                    playlistSize2.Tag = playlist.TotalAngleSize;

                    ListViewItem.ListViewSubItem[] playlistSubItems =
                        new ListViewItem.ListViewSubItem[]
                        {
                            playlistName,
                            playlistIndex,
                            playlistLength,
                            playlistSize,
                            playlistSize2
                        };

                    ListViewItem playlistItem =
                        new ListViewItem(playlistSubItems, 0);
                    listViewPlaylistFiles.Items.Add(playlistItem);

                    sortedPlaylists.Add(playlist);
                }
            }

            foreach (TSPlaylistFile playlistFile in sortedPlaylists)
            {
                foreach (TSStream stream in playlistFile.SortedStreams)
                {
                    if (stream.LanguageCode != null)
                    {
                        Language lang = Language.GetLanguage(stream.LanguageCode);
                        Languages.Add(lang);
                    }
                }
            }

            if (hasHiddenTracks)
            {
                textBoxDetails.Text += "(*) Some playlists on this disc have hidden tracks. These tracks are marked with an asterisk.";
            }

            ResetColumnWidths();

            FindMainPlaylist();
        }

        private ISet<Language> Languages = new HashSet<Language>();

        private List<TSPlaylistFile> sortedPlaylists = new List<TSPlaylistFile>();
        private List<TSPlaylistFile> mainPlaylists = new List<TSPlaylistFile>();

        private void FindMainPlaylist()
        {
            mainPlaylists.Clear();

            if (sortedPlaylists.Count == 0) return;

            double maxlength = sortedPlaylists[0].TotalLength;

            foreach (TSPlaylistFile playlist in sortedPlaylists)
            {
                if (playlist.TotalLength > maxlength * 0.9)
                {
                    playlist.IsMainPlaylist = true;
                    mainPlaylists.Add(playlist);
                }
            }

            foreach (TSPlaylistFile mainPlaylist in mainPlaylists)
            {
                int idx = sortedPlaylists.FindIndex(delegate(TSPlaylistFile sortedPlaylist)
                {
                    return sortedPlaylist == mainPlaylist;
                });
                if (idx != -1)
                {
                    listViewPlaylistFiles.Items[idx].Checked = true;
                }
            }

            FormDetails formDetails = new FormDetails(BDROM, sortedPlaylists, Languages);
            formDetails.ShowDialog(this);
            // TODO: Fix FormDetails initialization so that it doesn't need to be disposed every time
            formDetails.Dispose();
        }

        private bool FindMainPlaylistIndex(TSPlaylistFile playlist)
        {
            return mainPlaylists.Count > 0 && playlist != null && mainPlaylists[0] != null && playlist == mainPlaylists[0];
        }

        private void LoadPlaylist()
        {
            if (BDROM == null) return;
            if (listViewPlaylistFiles.SelectedItems.Count == 0) return;

            ListViewItem playlistItem = listViewPlaylistFiles.SelectedItems[0];

            if (playlistItem == null) return;

            string playlistFileName = playlistItem.Text;

            TSPlaylistFile playlist = null;
            if (BDROM.PlaylistFiles.ContainsKey(playlistFileName))
            {
                playlist = BDROM.PlaylistFiles[playlistFileName];
            }
            if (playlist == null) return;

            StreamTrackListViewPopulator.Populate(playlist, listViewStreamFiles, listViewStreams);
            
            ResetColumnWidths();
        }

        private void UpdatePlaylistBitrates()
        {
            foreach (ListViewItem item in listViewPlaylistFiles.Items)
            {
                string playlistName = (string)item.SubItems[0].Tag;
                if (BDROM.PlaylistFiles.ContainsKey(playlistName))
                {
                    TSPlaylistFile playlist = 
                        BDROM.PlaylistFiles[playlistName];
                    item.SubItems[4].Text = string.Format(
                        "{0}", (playlist.TotalAngleSize).ToString("N0"));
                    item.SubItems[4].Tag = playlist.TotalAngleSize;
                }
            }

            if (listViewPlaylistFiles.SelectedItems.Count == 0)
            {
                return;
            }

            ListViewItem selectedPlaylistItem =
                listViewPlaylistFiles.SelectedItems[0];
            if (selectedPlaylistItem == null)
            {
                return;
            }

            string selectedPlaylistName = (string)selectedPlaylistItem.SubItems[0].Tag;
            TSPlaylistFile selectedPlaylist = null;
            if (BDROM.PlaylistFiles.ContainsKey(selectedPlaylistName))
            {
                selectedPlaylist = BDROM.PlaylistFiles[selectedPlaylistName];
            }
            if (selectedPlaylist == null)
            {
                return;
            }

            for (int i = 0; i < listViewStreamFiles.Items.Count; i++)
            {
                ListViewItem item = listViewStreamFiles.Items[i];
                if (selectedPlaylist.StreamClips.Count > i &&
                    selectedPlaylist.StreamClips[i].Name == (string)item.SubItems[0].Tag)
                {
                    item.SubItems[4].Text = string.Format(
                         "{0}", (selectedPlaylist.StreamClips[i].PacketSize).ToString("N0"));
                    item.Tag = selectedPlaylist.StreamClips[i].PacketSize;

                }
            }

            for (int i = 0; i < listViewStreams.Items.Count; i++)
            {
                ListViewItem item = listViewStreams.Items[i];
                if (i < selectedPlaylist.SortedStreams.Count &&
                    selectedPlaylist.SortedStreams[i].PID == (ushort)item.Tag)
                {
                    TSStream stream = selectedPlaylist.SortedStreams[i];
                    int kbps = 0;
                    if (stream.AngleIndex > 0)
                    {
                        kbps = (int)Math.Round((double)stream.ActiveBitRate / 1000);
                    }
                    else
                    {
                        kbps = (int)Math.Round((double)stream.BitRate / 1000);
                    }
                    item.SubItems[2].Text = string.Format(
                        "{0} kbps", kbps);
                    item.SubItems[3].Text =
                        stream.Description;
                }
            }
        }

        #endregion

        #region Scan BDROM

        private BackgroundWorker ScanBDROMWorker = null;

        private class ScanBDROMState
        {
            public long TotalBytes = 0;
            public long FinishedBytes = 0;
            public DateTime TimeStarted = DateTime.Now;
            public TSStreamFile StreamFile = null;
            public Dictionary<string, List<TSPlaylistFile>> PlaylistMap = 
                new Dictionary<string, List<TSPlaylistFile>>();
            public Exception Exception = null;
        }

        private void ScanBDROM()
        {
            if (ScanBDROMWorker != null &&
                ScanBDROMWorker.IsBusy)
            {
                ScanBDROMWorker.CancelAsync();
                return;
            }

            buttonScan.Text = "Cancel Scan";
            progressBarScan.Value = 0;
            progressBarScan.Minimum = 0;
            progressBarScan.Maximum = 100;
            labelProgress.Text = "Scanning disc...";
            labelTimeElapsed.Text = "00:00:00";
            labelTimeRemaining.Text = "00:00:00";
            buttonBrowse.Enabled = false;
            buttonRescan.Enabled = false;

            List<TSStreamFile> streamFiles = new List<TSStreamFile>();
            if (listViewPlaylistFiles.CheckedItems == null ||
                listViewPlaylistFiles.CheckedItems.Count == 0)
            {
                foreach (TSStreamFile streamFile
                    in BDROM.StreamFiles.Values)
                {
                    streamFiles.Add(streamFile);
                }
            }
            else
            {
                foreach (ListViewItem item
                    in listViewPlaylistFiles.CheckedItems)
                {
                    if (BDROM.PlaylistFiles.ContainsKey(item.Text))
                    {
                        TSPlaylistFile playlist = 
                            BDROM.PlaylistFiles[item.Text];

                        foreach (TSStreamClip clip
                            in playlist.StreamClips)
                        {
                            if (!streamFiles.Contains(clip.StreamFile))
                            {
                                streamFiles.Add(clip.StreamFile);
                            }
                        }
                    }
                }
            }

            ScanBDROMWorker = new BackgroundWorker();
            ScanBDROMWorker.WorkerReportsProgress = true;
            ScanBDROMWorker.WorkerSupportsCancellation = true;
            ScanBDROMWorker.DoWork += ScanBDROMWork;
            ScanBDROMWorker.ProgressChanged += ScanBDROMProgress;
            ScanBDROMWorker.RunWorkerCompleted += ScanBDROMCompleted;
            ScanBDROMWorker.RunWorkerAsync(streamFiles);
        }

        private void ScanBDROMWork(
            object sender, 
            DoWorkEventArgs e)
        {
            ScanResult = new ScanBDROMResult();
            ScanResult.ScanException = new Exception("Scan is still running.");

            System.Threading.Timer timer = null;
            try
            {
                List<TSStreamFile> streamFiles =
                    (List<TSStreamFile>)e.Argument;

                ScanBDROMState scanState = new ScanBDROMState();
                foreach (TSStreamFile streamFile in streamFiles)
                {
                    if (BDAutoMuxerSettings.EnableSSIF &&
                        streamFile.InterleavedFile != null)
                    {
                        scanState.TotalBytes += streamFile.InterleavedFile.FileInfo.Length;
                    }
                    else
                    {
                        scanState.TotalBytes += streamFile.FileInfo.Length;
                    }
                    
                    if (!scanState.PlaylistMap.ContainsKey(streamFile.Name))
                    {
                        scanState.PlaylistMap[streamFile.Name] = new List<TSPlaylistFile>();
                    }

                    foreach (TSPlaylistFile playlist
                        in BDROM.PlaylistFiles.Values)
                    {
                        playlist.ClearBitrates();

                        foreach (TSStreamClip clip in playlist.StreamClips)
                        {
                            if (clip.Name == streamFile.Name)
                            {
                                if (!scanState.PlaylistMap[streamFile.Name].Contains(playlist))
                                {
                                    scanState.PlaylistMap[streamFile.Name].Add(playlist);
                                }
                            }
                        }
                    }
                }

                timer = new System.Threading.Timer(
                    ScanBDROMEvent, scanState, 1000, 1000);

                foreach (TSStreamFile streamFile in streamFiles)
                {
                    scanState.StreamFile = streamFile;
                    
                    Thread thread = new Thread(ScanBDROMThread);
                    thread.Start(scanState);
                    while (thread.IsAlive)
                    {
                        if (ScanBDROMWorker.CancellationPending)
                        {
                            ScanResult.ScanException = new Exception("Scan was cancelled.");
                            thread.Abort();
                            return;
                        }
                        Thread.Sleep(0);
                    }
                    scanState.FinishedBytes += streamFile.FileInfo.Length;
                    if (scanState.Exception != null)
                    {
                        ScanResult.FileExceptions[streamFile.Name] = scanState.Exception;
                    }
                }
                ScanResult.ScanException = null;
            }
            catch (Exception ex)
            {
                ScanResult.ScanException = ex;
            }
            finally
            {
                if (timer != null)
                {
                    timer.Dispose();
                }
            }
        }

        private void ScanBDROMThread(
            object parameter)
        {
            ScanBDROMState scanState = (ScanBDROMState)parameter;
            try
            {
                TSStreamFile streamFile = scanState.StreamFile;
                List<TSPlaylistFile> playlists = scanState.PlaylistMap[streamFile.Name];
                streamFile.Scan(playlists, true);
            }
            catch (Exception ex)
            {
                scanState.Exception = ex;
            }
        }

        private void ScanBDROMEvent(
            object state)
        {
            try
            {
                if (ScanBDROMWorker.IsBusy && 
                    !ScanBDROMWorker.CancellationPending)
                {
                    ScanBDROMWorker.ReportProgress(0, state);
                }
            }
            catch { }
        }

        private void ScanBDROMProgress(
            object sender, 
            ProgressChangedEventArgs e)
        {
            ScanBDROMState scanState = (ScanBDROMState)e.UserState;

            try
            {
                if (scanState.StreamFile != null)
                {
                    labelProgress.Text = string.Format(
                        "Scanning {0}...\r\n",
                        scanState.StreamFile.DisplayName);
                }

                long finishedBytes = scanState.FinishedBytes;
                if (scanState.StreamFile != null)
                {
                    finishedBytes += scanState.StreamFile.Size;
                }

                double progress = ((double)finishedBytes / scanState.TotalBytes);
                int progressValue = (int)Math.Round(progress * 100);
                if (progressValue < 0) progressValue = 0;
                if (progressValue > 100) progressValue = 100;
                progressBarScan.Value = progressValue;

                TimeSpan elapsedTime = DateTime.Now.Subtract(scanState.TimeStarted);
                TimeSpan remainingTime;
                if (progress > 0 && progress < 1)
                {
                    remainingTime = new TimeSpan(
                        (long)((double)elapsedTime.Ticks / progress) - elapsedTime.Ticks);
                }
                else
                {
                    remainingTime = new TimeSpan(0);
                }

                labelTimeElapsed.Text = string.Format(
                    "{0:D2}:{1:D2}:{2:D2}",
                    elapsedTime.Hours,
                    elapsedTime.Minutes,
                    elapsedTime.Seconds);

                labelTimeRemaining.Text = string.Format(
                    "{0:D2}:{1:D2}:{2:D2}",
                    remainingTime.Hours,
                    remainingTime.Minutes,
                    remainingTime.Seconds);

                UpdatePlaylistBitrates();
            }
            catch { }
        }

        private void ScanBDROMCompleted(
            object sender, 
            RunWorkerCompletedEventArgs e)
        {
            buttonScan.Enabled = false;

            UpdatePlaylistBitrates();

            labelProgress.Text = "Scan complete.";
            progressBarScan.Value = 100;
            labelTimeRemaining.Text = "00:00:00";

            if (ScanResult.ScanException != null)
            {
                string msg = string.Format(
                    "{0}", ScanResult.ScanException.Message);

                MessageBox.Show(msg, "BDAutoMuxer Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (BDAutoMuxerSettings.AutosaveReport)
                {
                    GenerateReport();
                }
                else if (ScanResult.FileExceptions.Count > 0)
                {
                    MessageBox.Show(
                        "Scan completed with errors (see report).", "BDAutoMuxer Scan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Scan completed successfully.", "BDAutoMuxer Scan",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            buttonBrowse.Enabled = true;
            buttonRescan.Enabled = true;
            buttonScan.Enabled = true;
            buttonScan.Text = "Scan Bitrates";
        }

        #endregion

        #region Report Generation

        private BackgroundWorker ReportWorker = null;

        private void GenerateReport()
        {
            ShowNotification("Please wait while we generate the report...");
            buttonViewReport.Enabled = false;

            List<TSPlaylistFile> playlists = new List<TSPlaylistFile>();
            if (listViewPlaylistFiles.CheckedItems == null ||
                listViewPlaylistFiles.CheckedItems.Count == 0)
            {
                foreach (ListViewItem item
                    in listViewPlaylistFiles.Items)
                {
                    if (BDROM.PlaylistFiles.ContainsKey(item.Text))
                    {
                        playlists.Add(BDROM.PlaylistFiles[item.Text]);
                    }
                }
            }
            else
            {
                foreach (ListViewItem item
                    in listViewPlaylistFiles.CheckedItems)
                {
                    if (BDROM.PlaylistFiles.ContainsKey(item.Text))
                    {
                        playlists.Add(BDROM.PlaylistFiles[item.Text]);
                    }
                }
            }

            ReportWorker = new BackgroundWorker();
            ReportWorker.WorkerReportsProgress = true;
            ReportWorker.WorkerSupportsCancellation = true;
            ReportWorker.DoWork += GenerateReportWork;
            ReportWorker.ProgressChanged += GenerateReportProgress;
            ReportWorker.RunWorkerCompleted += GenerateReportCompleted;
            ReportWorker.RunWorkerAsync(playlists);
        }

        private void GenerateReportWork(
            object sender, 
            DoWorkEventArgs e)
        {
            try
            {
                List<TSPlaylistFile> playlists = (List<TSPlaylistFile>)e.Argument;
                FormReport report = new FormReport();
                report.Generate(BDROM, playlists, ScanResult);
                e.Result = report;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void GenerateReportProgress(
            object sender, 
            ProgressChangedEventArgs e)
        {
        }

        private void GenerateReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HideNotification();
            if (e.Result != null)
            {
                if (e.Result.GetType().Name == "FormReport")
                {
                    ((Form)e.Result).Show();
                }
                else if (e.Result.GetType().Name == "Exception")
                {
                    string msg = string.Format(
                        "{0}", ((Exception)e.Result).Message);

                    MessageBox.Show(msg, "BDAutoMuxer Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            buttonViewReport.Enabled = true;
        }

        #endregion

        #region Notification Display

        private Form FormNotification = null;

        private void ShowNotification(
            string text)
        {
            HideNotification();

            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font(Font.SystemFontName, 12);
            label.Text = text;

            FormNotification = new Form();
            FormNotification.ControlBox = false;
            FormNotification.ShowInTaskbar = false;
            FormNotification.ShowIcon = false;
            FormNotification.FormBorderStyle = FormBorderStyle.Fixed3D;
            FormNotification.Controls.Add(label);
            FormNotification.Size = new Size(label.Width + 10, 18);
            FormNotification.Show(this);
            FormNotification.Location = new Point(
                this.Location.X + this.Width / 2 - FormNotification.Width / 2,
                this.Location.Y + this.Height / 2 - FormNotification.Height / 2);
        }

        private void HideNotification()
        {
            if (FormNotification != null &&
                !FormNotification.IsDisposed)
            {
                FormNotification.Close();
                FormNotification = null;
            }
        }

        private void UpdateNotification()
        {
            if (FormNotification != null &&
                !FormNotification.IsDisposed &&
                FormNotification.Visible)
            {
                FormNotification.Location = new Point(
                    this.Location.X + this.Width / 2 - FormNotification.Width / 2,
                    this.Location.Y + this.Height / 2 - FormNotification.Height / 2);
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            ResetColumnWidths();
            UpdateNotification();
        }

        private void FormMain_LocationChanged(object sender, EventArgs e)
        {
            UpdateNotification();
        }

        #endregion

        #region Comparison

        public static int ComparePlaylistFiles(
            TSPlaylistFile x,
            TSPlaylistFile y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return 1;
            }
            else if (x != null && y == null)
            {
                return -1;
            }
            else
            {
                if (x.TotalLength > y.TotalLength)
                {
                    return -1;
                }
                else if (y.TotalLength > x.TotalLength)
                {
                    return 1;
                }
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
        }

        public static int ComparePlaylistFilesForMainMovie(
            TSPlaylistFile x,
            TSPlaylistFile y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return 1;
            }
            else if (x != null && y == null)
            {
                return -1;
            }
            else
            {
                if (x.HiddenTrackCount < y.HiddenTrackCount)
                {
                    return -1;
                }
                else if (y.HiddenTrackCount < x.HiddenTrackCount)
                {
                    return 1;
                }
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
        }

        #endregion
    }

    #region Sorting

    public class ListViewColumnSorter : IComparer
    {
        private int ColumnToSort;
        private SortOrder OrderOfSort;
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            ColumnToSort = 0;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(
            object x, 
            object y)
        {
            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;
            
            int compareResult = ObjectCompare.Compare(
                listviewX.SubItems[ColumnToSort].Tag, 
                listviewY.SubItems[ColumnToSort].Tag);
            
            if (OrderOfSort == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }

        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }

    #endregion

    public class ScanBDROMResult
    {
        public Exception ScanException = new Exception("Scan has not been run.");
        public Dictionary<string, Exception> FileExceptions = new Dictionary<string, Exception>();
    }
}
