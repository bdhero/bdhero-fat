﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using BDInfo.models;

namespace BDInfo.views
{
    class PlaylistDataGridPopulator
    {
        private DataGridView playlistDataGridView;
        private IList<TSPlaylistFile> playlists;
        private IList<string> languageCodes;

        private BindingList<PlaylistGridItem> bindingList = new BindingList<PlaylistGridItem>();

        private bool showAllPlaylists = false;

        private IList<PlaylistGridItem> playlistGridItems = new List<PlaylistGridItem>();
        private IList<PlaylistGridItem> playlistGridItemsOriginal = new List<PlaylistGridItem>();

        public event EventHandler SelectionChanged;

        private TSPlaylistFile selectedPlaylist = null;
        public TSPlaylistFile SelectedPlaylist
        {
            get { return selectedPlaylist; }
        }

        public string MainLanguageCode
        {
            set
            {
                if (value == null) return;

                for (int i = 0; i < playlistGridItems.Count; i++)
                {
                    PlaylistGridItem item1 = playlistGridItems[i];
                    PlaylistGridItem item2 = playlistGridItemsOriginal[i];
                    if (! item1.VideoLanguageHasChanged)
                    {
                        item1.VideoLanguageAuto = value;
                    }
                    if (!item2.VideoLanguageHasChanged)
                    {
                        item2.VideoLanguageAuto = value;
                    }
                }
            }
        }

        public PlaylistDataGridPopulator(DataGridView dataGridView, IList<TSPlaylistFile> playlists, IList<string> languageCodes)
        {
            this.playlistDataGridView = dataGridView;
            this.playlists = playlists;
            this.languageCodes = languageCodes;

            this.playlistDataGridView.AutoGenerateColumns = false;
            this.playlistDataGridView.AutoSize = true;

            CreateColumns();

            this.playlistDataGridView.CellClick += playlistDataGridView_CellClick;
            this.playlistDataGridView.SelectionChanged += dataGridView_SelectionChanged;

            foreach (TSPlaylistFile playlist in playlists)
            {
                playlistGridItems.Add(new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null));
                playlistGridItemsOriginal.Add(new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null));
            }

            ShowAllPlaylists = false;
        }

        private void CreateColumns()
        {
            playlistDataGridView.Columns.Add(CreatePlayButtonColumn());
            playlistDataGridView.Columns.Add(CreateIsMainMovieColumn());
            playlistDataGridView.Columns.Add(CreateFilenameColumn());
            playlistDataGridView.Columns.Add(CreateLengthColumn());
            playlistDataGridView.Columns.Add(CreateSizeColumn());
            playlistDataGridView.Columns.Add(CreateVideoLanguageColumn());
            playlistDataGridView.Columns.Add(CreateCutColumn());
            playlistDataGridView.Columns.Add(CreateHasCommentaryColumn());
        }

        public bool HasChanged
        {
            get
            {
                bool hasChanged = false;
                for (int i = 0; i < playlistGridItems.Count && !hasChanged; i++)
                {
                    // TODO: Items are never equal.
                    // playlistGridItems[i] language == "fra" && playlistGridItemsOriginal[i] language == "eng"
                    // on Toy Story 3.
                    // One is getting updated correctly but not the other.
                    if (! playlistGridItems[i].Equals(playlistGridItemsOriginal[i]))
                    {
                        hasChanged = true;
                    }
                }
                return hasChanged;
            }
        }

        public bool ShowAllPlaylists
        {
            get { return showAllPlaylists; }
            set
            {
                showAllPlaylists = value;

                playlistDataGridView.DataSource = null;
                bindingList.Clear();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (showAllPlaylists || item.Playlist.IsMainPlaylist)
                    {
                        bindingList.Add(item);
                    }
                }
                
                playlistDataGridView.DataSource = bindingList;
            }
        }

        public IList<JsonPlaylist> JsonPlaylists
        {
            get
            {
                IList<JsonPlaylist> jsonPlaylists = new List<JsonPlaylist>();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (item.Playlist.IsMainPlaylist)
                    {
                        jsonPlaylists.Add(item.JsonPlaylist);
                    }
                }

                return jsonPlaylists;
            }
        }

        public void AutoConfigure(IList<JsonPlaylist> jsonPlaylists)
        {
            Dictionary<string, PlaylistGridItem> mainPlaylistGridItems = new Dictionary<string, PlaylistGridItem>();
            Dictionary<string, PlaylistGridItem> mainPlaylistGridItemsOriginal = new Dictionary<string, PlaylistGridItem>();
            
            foreach (PlaylistGridItem item in playlistGridItems)
            {
                if (item.Playlist.IsMainPlaylist)
                {
                    mainPlaylistGridItems.Add(item.Playlist.Name.ToUpper(), item);
                    mainPlaylistGridItemsOriginal.Add(item.Playlist.Name.ToUpper(), item);
                }
            }

            foreach (JsonPlaylist jsonPlaylist in jsonPlaylists)
            {
                AutoConfigure(jsonPlaylist, mainPlaylistGridItems);
                AutoConfigure(jsonPlaylist, mainPlaylistGridItemsOriginal);
            }
        }

        private void AutoConfigure(JsonPlaylist jsonPlaylist, Dictionary<string, PlaylistGridItem> gridItems)
        {
            PlaylistGridItem item = gridItems[jsonPlaylist.filename.ToUpper()];

            if (item == null) return;

            item.VideoLanguageAuto = jsonPlaylist.ISO_639_2;
            item.IsMainMovie = jsonPlaylist.is_main;
            if (jsonPlaylist.is_theatrical) item.Cut = Cut.Theatrical;
            else if (jsonPlaylist.is_special) item.Cut = Cut.Special;
            else if (jsonPlaylist.is_extended) item.Cut = Cut.Extended;
            else if (jsonPlaylist.is_unrated) item.Cut = Cut.Unrated;
            item.HasCommentary = jsonPlaylist.has_commentary;
        }

        private void playButton_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells.
            //if (e.RowIndex < 0 || e.ColumnIndex != playlistDataGridView.Columns["Preview"].Index)
            if (e.RowIndex < 0 || e.ColumnIndex != 0)
                return;

            PlaylistGridItem item = bindingList[e.RowIndex];

            System.Diagnostics.Process.Start(item.Playlist.FullName);
        }

        /// <see cref="http://stackoverflow.com/a/242760/467582"/>
        private void playlistDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var column = playlistDataGridView.Columns[e.ColumnIndex];
            if (column is DataGridViewComboBoxColumn)
            {
                playlistDataGridView.BeginEdit(true);
                if (playlistDataGridView[e.ColumnIndex, e.RowIndex].Selected)
                {
                    var control = (DataGridViewComboBoxEditingControl)playlistDataGridView.EditingControl;
                    control.DroppedDown = true;
                }
            }

        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Skip if nothing is selected
            if (playlistDataGridView.SelectedRows.Count == 0 &&
                playlistDataGridView.SelectedCells.Count == 0) return;

            int rowIndex = -1;

            if (playlistDataGridView.SelectedRows.Count > 0)
                rowIndex = playlistDataGridView.SelectedRows[0].Index;
            else
                rowIndex = playlistDataGridView.SelectedCells[0].RowIndex;

            if (rowIndex == -1) return;

            PlaylistGridItem playlistItem = bindingList[rowIndex];

            if (playlistItem == null) return;

            selectedPlaylist = playlistItem.Playlist;

            if (selectedPlaylist == null) return;

            string playlistFileName = selectedPlaylist.Name;

            SelectionChanged.Invoke(this, EventArgs.Empty);
        }

        private DataGridViewButtonColumn playButtonColumn;
        private DataGridViewCheckBoxColumn isMainMovieColumn;
        private DataGridViewTextBoxColumn filenameColumn;
        private DataGridViewTextBoxColumn lengthColumn;
        private DataGridViewTextBoxColumn sizeColumn;
        private DataGridViewComboBoxColumn videoLanguageColumn;
        private DataGridViewComboBoxColumn cutColumn;
        private DataGridViewCheckBoxColumn hasCommentaryColumn;

        private DataGridViewButtonColumn CreatePlayButtonColumn()
        {
            playButtonColumn = new DataGridViewButtonColumn();
            //column.Name = "Preview";
            playButtonColumn.Text = "Play";
            //column.ToolTipText = "Open this file in the default application";
            playButtonColumn.UseColumnTextForButtonValue = true;

            // Add a CellClick handler to handle clicks in the button column.
            playlistDataGridView.CellClick += new DataGridViewCellEventHandler(playButton_CellClick);

            return playButtonColumn;
        }

        private DataGridViewCheckBoxColumn CreateIsMainMovieColumn()
        {
            isMainMovieColumn = new DataGridViewCheckBoxColumn();
            isMainMovieColumn.Name = "Main Movie";
            isMainMovieColumn.DataPropertyName = "IsMainMovie";
            return isMainMovieColumn;
        }

        private DataGridViewTextBoxColumn CreateFilenameColumn()
        {
            filenameColumn = new DataGridViewTextBoxColumn();
            filenameColumn.Name = "Filename";
            filenameColumn.DataPropertyName = "Filename";
            filenameColumn.ReadOnly = true;
            return filenameColumn;
        }

        private DataGridViewTextBoxColumn CreateLengthColumn()
        {
            lengthColumn = new DataGridViewTextBoxColumn();
            lengthColumn.Name = "Length";
            lengthColumn.DataPropertyName = "Length";
            lengthColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            lengthColumn.ReadOnly = true;
            return lengthColumn;
        }

        private DataGridViewTextBoxColumn CreateSizeColumn()
        {
            sizeColumn = new DataGridViewTextBoxColumn();
            sizeColumn.Name = "Size";
            sizeColumn.DataPropertyName = "Size";
            sizeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            sizeColumn.ReadOnly = true;
            return sizeColumn;
        }

        private DataGridViewComboBoxColumn CreateVideoLanguageColumn()
        {
            videoLanguageColumn = new DataGridViewComboBoxColumn();
            videoLanguageColumn.Name = "Video Language";
            videoLanguageColumn.DataSource = languageCodes;
            videoLanguageColumn.DataPropertyName = "VideoLanguage";
            return videoLanguageColumn;
        }

        private DataGridViewComboBoxColumn CreateCutColumn()
        {
            cutColumn = new DataGridViewComboBoxColumn();
            cutColumn.Name = "Cut";
            cutColumn.DataSource = Enum.GetValues(typeof(Cut));
            cutColumn.DataPropertyName = "Cut";
            return cutColumn;
        }

        private DataGridViewCheckBoxColumn CreateHasCommentaryColumn()
        {
            hasCommentaryColumn = new DataGridViewCheckBoxColumn();
            hasCommentaryColumn.Name = "Commentary Available";
            hasCommentaryColumn.DataPropertyName = "HasCommentary";
            return hasCommentaryColumn;
        }
    }

    public enum Cut
    {
        Theatrical,
        Special,
        Extended,
        Unrated
    }

    public class PlaylistGridItem
    {
        private TSPlaylistFile playlist;
        private bool isMainMovie;
        private string filename;
        private double length;
        private string size;
        private string ISO_639_2;
        private bool ISO_639_2_hasChanged = false;
        private Cut cut;
        private bool hasCommentary;

        public PlaylistGridItem(TSPlaylistFile playlist, string ISO_639_2)
        {
            this.playlist = playlist;
            this.isMainMovie = playlist.IsMainPlaylist;
            this.filename = playlist.Name;
            this.length = playlist.TotalLength;
            this.size = playlist.FileSize.ToString("N0");
            this.ISO_639_2 = ISO_639_2;
            this.cut = Cut.Theatrical;
            this.hasCommentary = false;
        }

        public TSPlaylistFile Playlist
        {
            get { return playlist; }
        }

        public bool IsMainMovie
        {
            get { return isMainMovie; }
            set { isMainMovie = value; }
        }

        public string Filename
        {
            get { return filename; }
        }

        public string Length
        {
            get { return HumanFriendlyLength(length); }
        }

        public string Size
        {
            get { return size; }
        }

        public string VideoLanguage
        {
            get { return ISO_639_2; }
            set { ISO_639_2 = value; ISO_639_2_hasChanged = true; }
        }

        public string VideoLanguageAuto
        {
            get { return ISO_639_2; }
            set { ISO_639_2 = value; }
        }

        public bool VideoLanguageHasChanged
        {
            get { return ISO_639_2_hasChanged; }
        }

        public Cut Cut
        {
            get { return cut; }
            set { cut = value; }
        }

        public bool HasCommentary
        {
            get { return hasCommentary; }
            set { hasCommentary = value; }
        }

        public JsonPlaylist JsonPlaylist
        {
            get
            {
                JsonPlaylist jsonPlaylist = new JsonPlaylist();

                jsonPlaylist.filename = Filename;
                jsonPlaylist.filesize = Playlist.FileSize;
                jsonPlaylist.length_sec = (int) Playlist.TotalLength;
                jsonPlaylist.ISO_639_2 = VideoLanguage;

                jsonPlaylist.is_main = IsMainMovie;
                jsonPlaylist.is_theatrical = Cut.Equals(Cut.Theatrical);
                jsonPlaylist.is_special = Cut.Equals(Cut.Special);
                jsonPlaylist.is_extended = Cut.Equals(Cut.Extended);
                jsonPlaylist.is_unrated = Cut.Equals(Cut.Unrated);
                jsonPlaylist.has_commentary = HasCommentary;

                return jsonPlaylist;
            }
        }

        private static string HumanFriendlyLength(double length)
        {
            TimeSpan playlistLengthSpan = new TimeSpan((long)(length * 10000000));
            return string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                playlistLengthSpan.Hours,
                playlistLengthSpan.Minutes,
                playlistLengthSpan.Seconds);
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            PlaylistGridItem that = obj as PlaylistGridItem;
            if ((System.Object)that == null)
            {
                return false;
            }

            return
                this.playlist == that.playlist &&
                this.isMainMovie == that.isMainMovie &&
                this.filename == that.filename &&
                this.length == that.length &&
                this.size == that.size &&
                this.ISO_639_2 == that.ISO_639_2 &&
                this.cut == that.cut &&
                this.hasCommentary == that.hasCommentary;
        }
    }
}
