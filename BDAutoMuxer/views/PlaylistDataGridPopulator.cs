﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using BDAutoMuxer.models;
using System.Drawing;

namespace BDAutoMuxer.views
{
    class PlaylistDataGridPopulator
    {
        private DataGridView dataGridView;
        private IList<TSPlaylistFile> playlists;
        private IList<Language> languages = new List<Language>();
        private IList<string> languageCodes;

        private BindingList<PlaylistGridItem> bindingList = new BindingList<PlaylistGridItem>();

        private bool showAllPlaylists = false;

        private IList<PlaylistGridItem> playlistGridItems = new List<PlaylistGridItem>();
        private IList<PlaylistGridItem> playlistGridItemsOriginal = new List<PlaylistGridItem>();

        public event EventHandler SelectionChanged;
        public event EventHandler ItemChanged;

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
            this.dataGridView = dataGridView;
            this.playlists = playlists;
            this.languageCodes = languageCodes;

            foreach (string code in languageCodes)
            {
                languages.Add(Language.GetLanguage(code));
            }

            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.AutoSize = true;

            CreateColumns();

            this.dataGridView.CellClick += playlistDataGridView_CellClick;
            this.dataGridView.SelectionChanged += dataGridView_SelectionChanged;

            foreach (TSPlaylistFile playlist in playlists)
            {
                PlaylistGridItem item = new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null);
                PlaylistGridItem clone = new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null);

                item.PropertyChanged += OnItemChange;

                playlistGridItems.Add(item);
                playlistGridItemsOriginal.Add(clone);
            }

            ShowAllPlaylists = false;
        }

        private void OnItemChange(object sender, PropertyChangedEventArgs e)
        {
            ItemChanged.Invoke(this, EventArgs.Empty);
        }

        public bool HasChanged
        {
            get
            {
                bool hasChanged = false;
                for (int i = 0; i < playlistGridItems.Count && !hasChanged; i++)
                {
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

                dataGridView.DataSource = null;
                bindingList.Clear();

                int i = 0;
                IList<int> enabledRowIndexes = new List<int>();
                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (showAllPlaylists || item.Playlist.IsMainPlaylist)
                    {
                        bindingList.Add(item);

                        if (item.Playlist.IsMainPlaylist)
                            enabledRowIndexes.Add(i);

                        i++;
                    }
                }

                dataGridView.DataSource = bindingList;

                for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
                {
                    enableRow(dataGridView.Rows[rowIndex], enabledRowIndexes.Contains(rowIndex));
                }
            }
        }

        public TSPlaylistFile PlaylistAt(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView.Rows.Count)
            {
                throw new IndexOutOfRangeException("Playlist row index " + rowIndex + " is out of range [0, " + dataGridView.Rows.Count + "]");
            }
            
            object item = dataGridView.Rows[rowIndex].DataBoundItem;
            
            if (!(item is PlaylistGridItem))
                return null;

            return (item as PlaylistGridItem).Playlist;
        }

        private void enableRow(DataGridViewRow row, bool enabled)
        {
            row.ReadOnly = !enabled;
            for (int colIndex = 0; colIndex < dataGridView.Columns.Count; colIndex++)
            {
                DataGridViewCell cell = dataGridView[colIndex, row.Index];
                enableCell(cell, enabled);
            }
        }

        /// <summary>
        /// Toggles the "enabled" status of a cell in a DataGridView. There is no native
        /// support for disabling a cell, hence the need for this method. The disabled state
        /// means that the cell is read-only and grayed out.
        /// </summary>
        /// <param name="dc">Cell to enable/disable</param>
        /// <param name="enabled">Whether the cell is enabled or disabled</param>
        /// <see cref="http://stackoverflow.com/a/5291514/467582"/>
        private void enableCell(DataGridViewCell dc, bool enabled)
        {
            // toggle read-only state
            // TODO: Find a better method of disabling cells that disables their controls as well
            dc.ReadOnly = !enabled;
            if (enabled)
            {
                // restore cell style to the default value
                dc.Style.BackColor = dc.OwningColumn.DefaultCellStyle.BackColor;
                dc.Style.ForeColor = dc.OwningColumn.DefaultCellStyle.ForeColor;
            }
            else
            {
                // gray out the cell
                // TODO: Use system colors instead of hard-coded ones
                dc.Style.BackColor = Color.LightGray;
                dc.Style.ForeColor = Color.DarkGray;
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

        public ISet<Language> SelectedVideoLanguages
        {
            get
            {
                ISet<Language> selectedVideoLanguages = new HashSet<Language>();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (item.Playlist.IsMainPlaylist)
                    {
                        selectedVideoLanguages.Add(Language.GetLanguage(item.VideoLanguage));
                    }
                }

                return selectedVideoLanguages;
            }
        }

        public ISet<Cut> SelectedCuts
        {
            get
            {
                ISet<Cut> selectedCuts = new HashSet<Cut>();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (item.Playlist.IsMainPlaylist)
                    {
                        selectedCuts.Add(item.Cut);
                    }
                }

                return selectedCuts;
            }
        }

        public IList<CommentaryOption> SelectedCommentaryOptions
        {
            get
            {
                ISet<CommentaryOption> selectedCommentaryOptionsSet = new HashSet<CommentaryOption>();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (item.Playlist.IsMainPlaylist)
                    {
                        selectedCommentaryOptionsSet.Add(item.HasCommentary ? CommentaryOption.Yes : CommentaryOption.No);
                    }
                }

                IList<CommentaryOption> selectedCommentaryOptionsList = selectedCommentaryOptionsSet.ToList();

                if (selectedCommentaryOptionsList.Count > 1)
                {
                    selectedCommentaryOptionsList.Insert(0, CommentaryOption.Any);
                }

                return selectedCommentaryOptionsList;
            }
        }

        public void AutoConfigure(IList<JsonPlaylist> jsonPlaylists)
        {
            Dictionary<string, PlaylistGridItem> mainPlaylistGridItems = new Dictionary<string, PlaylistGridItem>();
            Dictionary<string, PlaylistGridItem> mainPlaylistGridItemsOriginal = new Dictionary<string, PlaylistGridItem>();

            for (int i = 0; i < playlistGridItems.Count; i++)
            {
                PlaylistGridItem item = playlistGridItems[i];
                PlaylistGridItem itemOriginal = playlistGridItemsOriginal[i];

                if (item.Playlist.IsMainPlaylist)
                {
                    mainPlaylistGridItems.Add(item.Playlist.Name.ToUpper(), item);
                    mainPlaylistGridItemsOriginal.Add(item.Playlist.Name.ToUpper(), itemOriginal);
                }
            }

            foreach (JsonPlaylist jsonPlaylist in jsonPlaylists)
            {
                AutoConfigure(jsonPlaylist, mainPlaylistGridItems);
                AutoConfigure(jsonPlaylist, mainPlaylistGridItemsOriginal);
            }
        }

        public ISet<TSPlaylistFile> GetPlaylistsWithMainMovie(bool mainMovie)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (PlaylistGridItem item in playlistGridItems)
            {
                if (item.Playlist.IsMainPlaylist && item.IsMainMovie == mainMovie)
                    files.Add(item.Playlist);
            }

            return files;
        }

        public ISet<TSPlaylistFile> GetPlaylistsWithVideoLanguage(Language lang)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (PlaylistGridItem item in playlistGridItems)
            {
                if (item.Playlist.IsMainPlaylist && Language.GetLanguage(item.VideoLanguage) == lang)
                    files.Add(item.Playlist);
            }

            return files;
        }

        public ISet<TSPlaylistFile> GetPlaylistsWithCut(Cut cut)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (PlaylistGridItem item in playlistGridItems)
            {
                if (item.Playlist.IsMainPlaylist && item.Cut == cut)
                    files.Add(item.Playlist);
            }

            return files;
        }

        public ISet<TSPlaylistFile> GetPlaylistsWithCommentaryOption(CommentaryOption commentaryOption)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (PlaylistGridItem item in playlistGridItems)
            {
                bool isAny = commentaryOption == CommentaryOption.Any;
                bool isYes = item.HasCommentary == true && commentaryOption == CommentaryOption.Yes;
                bool isNo = item.HasCommentary == false && commentaryOption == CommentaryOption.No;

                if (item.Playlist.IsMainPlaylist && (isAny || isYes || isNo))
                    files.Add(item.Playlist);
            }

            return files;
        }

        public ISet<TSPlaylistFile> GetPlaylistsWithAudioLanguages(ICollection<Language> audioLanguages)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (PlaylistGridItem item in playlistGridItems)
            {
                if (item.Playlist.IsMainPlaylist)
                {
                    foreach (TSAudioStream audioStream in item.Playlist.AudioStreams)
                    {
                        if (audioLanguages.Contains(Language.GetLanguage(audioStream.LanguageCode)))
                            files.Add(item.Playlist);
                    }
                }
            }

            return files;
        }

        public ISet<TSPlaylistFile> GetPlaylistsWithSubtitleLanguages(ICollection<Language> subtitleLanguages)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (PlaylistGridItem item in playlistGridItems)
            {
                if (item.Playlist.IsMainPlaylist)
                {
                    foreach (TSGraphicsStream graphicsStream in item.Playlist.GraphicsStreams)
                    {
                        if (subtitleLanguages.Contains(Language.GetLanguage(graphicsStream.LanguageCode)))
                            files.Add(item.Playlist);
                    }
                    foreach (TSTextStream textStream in item.Playlist.TextStreams)
                    {
                        if (subtitleLanguages.Contains(Language.GetLanguage(textStream.LanguageCode)))
                            files.Add(item.Playlist);
                    }
                }
            }

            return files;
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
            if (e.RowIndex < 0 || e.ColumnIndex != playButtonColumn.Index)
                return;

            PlaylistGridItem item = bindingList[e.RowIndex];

            System.Diagnostics.Process.Start(item.Playlist.FullName);
        }

        /// <see cref="http://stackoverflow.com/a/242760/467582"/>
        private void playlistDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks on the header cells
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Ignore readonly (i.e., disabled) cells
            if (dataGridView[e.ColumnIndex, e.RowIndex].ReadOnly)
                return;

            var column = dataGridView.Columns[e.ColumnIndex];
            if (column is DataGridViewComboBoxColumn)
            {
                dataGridView.BeginEdit(true);
                if (dataGridView[e.ColumnIndex, e.RowIndex].Selected)
                {
                    var control = (DataGridViewComboBoxEditingControl)dataGridView.EditingControl;
                    control.DroppedDown = true;
                }
            }
        }

        private int prevRowIndex = -1;

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Skip if nothing is selected
            if (dataGridView.SelectedRows.Count == 0 &&
                dataGridView.SelectedCells.Count == 0) return;

            int rowIndex = -1;

            // User selected entire row
            if (dataGridView.SelectedRows.Count > 0)
                rowIndex = dataGridView.SelectedRows[0].Index;
            // User selected a single cell
            else
                rowIndex = dataGridView.SelectedCells[0].RowIndex;

            if (rowIndex == -1) return;

            // Ignore readonly (i.e., disabled) rows
            if (dataGridView.Rows[rowIndex].ReadOnly)
                return;

            PlaylistGridItem playlistItem = bindingList[rowIndex];

            if (playlistItem == null) return;

            selectedPlaylist = playlistItem.Playlist;

            if (selectedPlaylist == null) return;

            if (rowIndex != prevRowIndex)
            {
                prevRowIndex = rowIndex;
                SelectionChanged.Invoke(this, EventArgs.Empty);
            }
        }

        public bool SelectAll
        {
            set
            {
                foreach (PlaylistGridItem gridItem in playlistGridItems)
                {
                    gridItem.IsMainMovie = value;
                }
            }
        }

        private DataGridViewButtonColumn playButtonColumn;
        private DataGridViewCheckBoxColumn isMainMovieColumn;
        private DataGridViewTextBoxColumn filenameColumn;
        private DataGridViewTextBoxColumn lengthColumn;
        private DataGridViewTextBoxColumn sizeColumn;
        private DataGridViewComboBoxColumn videoLanguageColumn;
        private DataGridViewComboBoxColumn cutColumn;
        private DataGridViewCheckBoxColumn hasCommentaryColumn;

        private void CreateColumns()
        {
            dataGridView.Columns.Add(CreatePlayButtonColumn());
            dataGridView.Columns.Add(CreateIsMainMovieColumn());
            dataGridView.Columns.Add(CreateFilenameColumn());
            dataGridView.Columns.Add(CreateLengthColumn());
            dataGridView.Columns.Add(CreateSizeColumn());
            dataGridView.Columns.Add(CreateVideoLanguageColumn());
            dataGridView.Columns.Add(CreateCutColumn());
            dataGridView.Columns.Add(CreateHasCommentaryColumn());
        }

        private DataGridViewButtonColumn CreatePlayButtonColumn()
        {
            playButtonColumn = new DataGridViewButtonColumn();
            //column.Name = "Preview";
            playButtonColumn.Text = "Play";
            //column.ToolTipText = "Open this file in the default application";
            playButtonColumn.UseColumnTextForButtonValue = true;

            // Add a CellClick handler to handle clicks in the button column.
            dataGridView.CellClick += new DataGridViewCellEventHandler(playButton_CellClick);

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
            videoLanguageColumn.DataSource = languageCodes.ToArray();
            videoLanguageColumn.DataPropertyName = "VideoLanguage";
            //videoLanguageColumn.DisplayMember = "VideoLanguageName";
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

    public enum CommentaryOption
    {
        Any, Yes, No
    }

    public class PlaylistGridItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PlaylistGridItem savedState;

        private TSPlaylistFile playlist;
        private bool isMainMovie;
        private string filename;
        private double length;
        private string size;
        private string ISO_639_2;
        private bool ISO_639_2_hasChanged = false;
        private Cut cut;
        private bool hasCommentary;

        public PlaylistGridItem(TSPlaylistFile playlist, string ISO_639_2) : this(playlist, ISO_639_2, true)
        {
        }

        private PlaylistGridItem(TSPlaylistFile playlist, string ISO_639_2, bool clone)
        {
            this.playlist = playlist;
            this.isMainMovie = playlist.IsMainPlaylist && !playlist.HasDuplicateClips;
            this.filename = playlist.Name;
            this.length = playlist.TotalLength;
            this.size = playlist.FileSize.ToString("N0");
            this.ISO_639_2 = ISO_639_2;
            this.cut = Cut.Theatrical;
            this.hasCommentary = false;

            if ( clone )
                this.savedState = Clone();
        }

        public PlaylistGridItem Clone()
        {
            PlaylistGridItem clone = new PlaylistGridItem(playlist, ISO_639_2, false);
            this.CopyTo(clone);
            return clone;
        }

        public void CopyTo(PlaylistGridItem that)
        {
            that.playlist = this.playlist;
            that.isMainMovie = this.isMainMovie;
            that.filename = this.filename;
            that.length = this.length;
            that.size = this.size;
            that.ISO_639_2 = this.ISO_639_2;
            that.cut = this.cut;
            that.hasCommentary = this.hasCommentary;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public TSPlaylistFile Playlist
        {
            get { return playlist; }
        }

        public bool IsMainMovie
        {
            get { return isMainMovie; }
            set
            {
                if (isMainMovie != value)
                {
                    isMainMovie = value;
                    OnPropertyChanged("IsMainMovie");
                }
            }
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
            set
            {
                if (ISO_639_2 != value)
                {
                    ISO_639_2 = value;
                    ISO_639_2_hasChanged = true;
                    OnPropertyChanged("VideoLanguage");
                }
            }
        }

        public string VideoLanguageAuto
        {
            get { return ISO_639_2; }
            set { ISO_639_2 = value; }
        }

        public string VideoLanguageName
        {
            get { return Language.GetName(ISO_639_2); }
            set { }
        }

        public bool VideoLanguageHasChanged
        {
            get { return ISO_639_2_hasChanged; }
        }

        public Cut Cut
        {
            get { return cut; }
            set
            {
                if (cut != value)
                {
                    cut = value;
                    OnPropertyChanged("Cut");
                }
            }
        }

        public bool HasCommentary
        {
            get { return hasCommentary; }
            set
            {
                if (hasCommentary != value)
                {
                    hasCommentary = value;
                    OnPropertyChanged("HasCommentary");
                }
            }
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

        public override int GetHashCode()
        {
            return ("" + this.Playlist.GetHashCode() + this.IsMainMovie + this.Filename + this.Length + this.Size + this.ISO_639_2 + this.Cut + this.HasCommentary).GetHashCode();
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