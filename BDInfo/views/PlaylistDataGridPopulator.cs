using System;
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
            this.playlistDataGridView = dataGridView;
            this.playlists = playlists;
            this.languageCodes = languageCodes;

            foreach (string code in languageCodes)
            {
                languages.Add(Language.GetLanguage(code));
            }

            this.playlistDataGridView.AutoGenerateColumns = false;
            this.playlistDataGridView.AutoSize = true;

            CreateColumns();

            this.playlistDataGridView.CellClick += playlistDataGridView_CellClick;
            this.playlistDataGridView.SelectionChanged += dataGridView_SelectionChanged;

            //this.playlistDataGridView.CurrentCellChanged += dataGridView_CurrentCellChanged;
            //this.playlistDataGridView.CurrentCellDirtyStateChanged += dataGridView_CurrentCellDirtyStateChanged;

            foreach (TSPlaylistFile playlist in playlists)
            {
                PlaylistGridItem item = new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null);
                PlaylistGridItem clone = item.Clone();

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

        private int prevRowIndex = -1;

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Skip if nothing is selected
            if (playlistDataGridView.SelectedRows.Count == 0 &&
                playlistDataGridView.SelectedCells.Count == 0) return;

            int rowIndex = -1;

            // User selected entire row
            if (playlistDataGridView.SelectedRows.Count > 0)
                rowIndex = playlistDataGridView.SelectedRows[0].Index;
            // User selected a single cell
            else
                rowIndex = playlistDataGridView.SelectedCells[0].RowIndex;

            if (rowIndex == -1) return;

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
            playlistDataGridView.Columns.Add(CreatePlayButtonColumn());
            playlistDataGridView.Columns.Add(CreateIsMainMovieColumn());
            playlistDataGridView.Columns.Add(CreateFilenameColumn());
            playlistDataGridView.Columns.Add(CreateLengthColumn());
            playlistDataGridView.Columns.Add(CreateSizeColumn());
            playlistDataGridView.Columns.Add(CreateVideoLanguageColumn());
            playlistDataGridView.Columns.Add(CreateCutColumn());
            playlistDataGridView.Columns.Add(CreateHasCommentaryColumn());
        }

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
            this.isMainMovie = playlist.IsMainPlaylist;
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
