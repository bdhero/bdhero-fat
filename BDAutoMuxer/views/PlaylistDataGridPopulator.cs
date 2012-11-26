using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BDAutoMuxer.models;

namespace BDAutoMuxer.views
{
    class PlaylistDataGridPopulator
    {
        private readonly DataGridView _dataGridView;
        private readonly IList<Language> _languages = new List<Language>();
        private readonly IList<string> _languageCodes;

        private readonly BindingList<PlaylistGridItem> _bindingList = new BindingList<PlaylistGridItem>();

        private bool _showAllPlaylists;

        private readonly IList<PlaylistGridItem> _playlistGridItems = new List<PlaylistGridItem>();
        private readonly IList<PlaylistGridItem> _playlistGridItemsOriginal = new List<PlaylistGridItem>();

        public event EventHandler SelectionChanged;
        public event EventHandler ItemChanged;

        public TSPlaylistFile SelectedPlaylist { get; private set; }

        private int _prevRowIndex = -1;

        public string MainLanguageCode
        {
            set
            {
                if (value == null) return;

                _dataGridView.EndEdit();

                for (var i = 0; i < _playlistGridItems.Count; i++)
                {
                    var item1 = _playlistGridItems[i];
                    var item2 = _playlistGridItemsOriginal[i];
                    if (!item1.VideoLanguageHasChanged)
                    {
                        item1.VideoLanguageAuto = value;
                    }
                    if (!item2.VideoLanguageHasChanged)
                    {
                        item2.VideoLanguageAuto = value;
                    }
                }

                _dataGridView.Refresh();
            }
        }

        private static IEnumerable<TSPlaylistFile> SortPlaylists(IEnumerable<TSPlaylistFile> playlists)
        {
            var array = playlists.ToArray();
            Array.Sort(array, ComparePlaylists);
            return new List<TSPlaylistFile>(array);
        }

        private static int ComparePlaylists(TSPlaylistFile playlist1, TSPlaylistFile playlist2)
        {
            // XOR - One is a main movie but not the other
            if (playlist1.IsMainMovie && !playlist2.IsMainMovie) return -1;
            if (playlist2.IsMainMovie && !playlist1.IsMainMovie) return +1;

            // AND - Both are main movies
            if (playlist1.IsMainMovie && playlist2.IsMainMovie) return String.Compare(playlist1.Name, playlist2.Name, StringComparison.OrdinalIgnoreCase);

            return 0;
        }

        public PlaylistDataGridPopulator(DataGridView dataGridView, IEnumerable<TSPlaylistFile> playlists, IList<string> languageCodes)
        {
            _dataGridView = dataGridView;
            _languageCodes = languageCodes;

            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            playlists = SortPlaylists(playlists);

            foreach (var code in languageCodes)
            {
                _languages.Add(Language.GetLanguage(code));
            }

            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.AutoSize = true;

            CreateColumns();

            _dataGridView.CellClick += playlistDataGridView_CellClick;
            _dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            _dataGridView.CellBeginEdit += dataGridView_CellBeginEdit;

            foreach (var playlist in playlists)
            {
                var item = new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null);
                var clone = new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null);

                item.PropertyChanged += OnItemChange;

                _playlistGridItems.Add(item);
                _playlistGridItemsOriginal.Add(clone);
            }

            ShowAllPlaylists = false;
        }

        ~PlaylistDataGridPopulator()
        {
            Destroy();
        }

        public void Destroy()
        {
            _dataGridView.CellClick -= playlistDataGridView_CellClick;
            _dataGridView.SelectionChanged -= dataGridView_SelectionChanged;
            _dataGridView.CellBeginEdit -= dataGridView_CellBeginEdit;

            foreach (var pgi in _playlistGridItems)
            {
                pgi.PropertyChanged -= OnItemChange;
            }

            _dataGridView.CellClick -= playButton_CellClick;
        }

        private void OnItemChange(object sender, PropertyChangedEventArgs e)
        {
            ItemChanged.Invoke(this, EventArgs.Empty);
        }

        public bool HasChanged
        {
            get
            {
                var hasChanged = false;
                for (var i = 0; i < _playlistGridItems.Count && !hasChanged; i++)
                {
                    if (! _playlistGridItems[i].Equals(_playlistGridItemsOriginal[i]))
                    {
                        hasChanged = true;
                    }
                }
                return hasChanged;
            }
        }

        public bool ShowAllPlaylists
        {
            get { return _showAllPlaylists; }
            set
            {
                _showAllPlaylists = value;

                _dataGridView.DataSource = null;
                _bindingList.Clear();

                var i = 0;
                IList<int> enabledRowIndexes = new List<int>();
                foreach (var item in _playlistGridItems.Where(item => _showAllPlaylists || item.Playlist.IsFeatureLength))
                {
                    _bindingList.Add(item);

                    if (item.Playlist.IsFeatureLength)
                        enabledRowIndexes.Add(i);

                    i++;
                }

                _dataGridView.DataSource = _bindingList;

                for (var rowIndex = 0; rowIndex < _dataGridView.Rows.Count; rowIndex++)
                {
                    EnableBand(_dataGridView.Rows[rowIndex], enabledRowIndexes.Contains(rowIndex));
                }
            }
        }

        public TSPlaylistFile PlaylistAt(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _dataGridView.Rows.Count)
            {
                throw new IndexOutOfRangeException("Playlist row index " + rowIndex + " is out of range [0, " + _dataGridView.Rows.Count + "]");
            }
            
            var item = _dataGridView.Rows[rowIndex].DataBoundItem;
            
            if (!(item is PlaylistGridItem))
                return null;

            return (item as PlaylistGridItem).Playlist;
        }

        private void EnableBand(DataGridViewBand row, bool enabled)
        {
            row.ReadOnly = !enabled;
            for (var colIndex = 0; colIndex < _dataGridView.Columns.Count; colIndex++)
            {
                EnableCell(_dataGridView[colIndex, row.Index], enabled);
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
        private static void EnableCell(DataGridViewCell dc, bool enabled)
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

        public IEnumerable<JsonPlaylist> JsonPlaylists
        {
            get
            {
                return (from item in _playlistGridItems
                        where item.Playlist.IsFeatureLength
                        select item.JsonPlaylist).ToList();
            }
        }

        public ISet<Language> SelectedVideoLanguages
        {
            get
            {
                return
                    new HashSet<Language>(
                        (from item in _playlistGridItems
                         where item.Playlist.IsFeatureLength
                         select Language.GetLanguage(item.VideoLanguage)).ToList());
            }
        }

        public ISet<Cut> SelectedCuts
        {
            get
            {
                return new HashSet<Cut>(
                    (from item in _playlistGridItems
                     where item.Playlist.IsFeatureLength
                     select item.Cut).ToList());
            }
        }

        public IList<CommentaryOption> SelectedCommentaryOptions
        {
            get
            {
                ISet<CommentaryOption> selectedCommentaryOptionsSet = new HashSet<CommentaryOption>();

                foreach (var item in _playlistGridItems.Where(item => item.Playlist.IsFeatureLength))
                {
                    selectedCommentaryOptionsSet.Add(item.HasCommentary ? CommentaryOption.Yes : CommentaryOption.No);
                }

                IList<CommentaryOption> selectedCommentaryOptionsList = selectedCommentaryOptionsSet.ToList();

                if (selectedCommentaryOptionsList.Count > 1)
                {
                    selectedCommentaryOptionsList.Insert(0, CommentaryOption.Any);
                }

                return selectedCommentaryOptionsList;
            }
        }

        public void AutoConfigure(IEnumerable<JsonPlaylist> jsonPlaylists)
        {
            var mainPlaylistGridItems = new Dictionary<string, PlaylistGridItem>();
            var mainPlaylistGridItemsOriginal = new Dictionary<string, PlaylistGridItem>();

            for (var i = 0; i < _playlistGridItems.Count; i++)
            {
                var item = _playlistGridItems[i];
                var itemOriginal = _playlistGridItemsOriginal[i];

                if (!item.Playlist.IsFeatureLength) continue;

                mainPlaylistGridItems.Add(item.Playlist.Name.ToUpper(), item);
                mainPlaylistGridItemsOriginal.Add(item.Playlist.Name.ToUpper(), itemOriginal);
            }

            foreach (var jsonPlaylist in jsonPlaylists)
            {
                AutoConfigure(jsonPlaylist, mainPlaylistGridItems);
                AutoConfigure(jsonPlaylist, mainPlaylistGridItemsOriginal);
            }

            _dataGridView.Refresh();
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithMainMovie(bool mainMovie)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (var item in _playlistGridItems.Where(item => item.Playlist.IsFeatureLength && item.IsMainMovie == mainMovie))
            {
                files.Add(item.Playlist);
            }

            return files;
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithVideoLanguage(Language lang)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (var item in _playlistGridItems.Where(item => item.Playlist.IsFeatureLength && Language.GetLanguage(item.VideoLanguage) == lang))
            {
                files.Add(item.Playlist);
            }

            return files;
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithCut(Cut cut)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (var item in _playlistGridItems.Where(item => item.Playlist.IsFeatureLength && item.Cut == cut))
            {
                files.Add(item.Playlist);
            }

            return files;
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithCommentaryOption(CommentaryOption commentaryOption)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (var item in _playlistGridItems)
            {
                var isAny = commentaryOption == CommentaryOption.Any;
                var isYes = item.HasCommentary && commentaryOption == CommentaryOption.Yes;
                var isNo = !item.HasCommentary && commentaryOption == CommentaryOption.No;

                if (item.Playlist.IsFeatureLength && (isAny || isYes || isNo))
                    files.Add(item.Playlist);
            }

            return files;
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithAudioLanguages(ICollection<Language> audioLanguages)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (var item in _playlistGridItems)
            {
                if (!item.Playlist.IsFeatureLength) continue;

                foreach (var audioStream in item.Playlist.AudioStreams)
                {
                    if (audioLanguages.Contains(Language.GetLanguage(audioStream.LanguageCode)))
                        files.Add(item.Playlist);
                }
            }

            return files;
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithSubtitleLanguages(ICollection<Language> subtitleLanguages)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            foreach (var item in _playlistGridItems)
            {
                if (item.Playlist.IsFeatureLength)
                {
                    foreach (var graphicsStream in item.Playlist.GraphicsStreams)
                    {
                        if (subtitleLanguages.Contains(Language.GetLanguage(graphicsStream.LanguageCode)))
                            files.Add(item.Playlist);
                    }
                    foreach (var textStream in item.Playlist.TextStreams)
                    {
                        if (subtitleLanguages.Contains(Language.GetLanguage(textStream.LanguageCode)))
                            files.Add(item.Playlist);
                    }
                }
            }

            return files;
        }

        private static void AutoConfigure(JsonPlaylist jsonPlaylist, Dictionary<string, PlaylistGridItem> gridItems)
        {
            var key = jsonPlaylist.filename.ToUpper();

            // TODO: Figure out why this fails on "The Hunger Games"
            if (!gridItems.ContainsKey(key))
                return;

            var item = gridItems[key];

            if (item == null)
                return;

            item.VideoLanguageAuto = jsonPlaylist.ISO_639_2;
            item.IsMainMovie = jsonPlaylist.is_main;

            if (jsonPlaylist.is_theatrical) item.Cut = Cut.Theatrical;
            else if (jsonPlaylist.is_special) item.Cut = Cut.Special;
            else if (jsonPlaylist.is_extended) item.Cut = Cut.Extended;
            else if (jsonPlaylist.is_unrated) item.Cut = Cut.Unrated;

            item.HasCommentary = jsonPlaylist.has_commentary;
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == _filenameColumn.Index || e.ColumnIndex == _lengthColumn.Index || e.ColumnIndex == _sizeColumn.Index)
                e.Cancel = true;
        }

        private void playButton_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells.
            if (e.RowIndex < 0 || e.ColumnIndex != _playButtonColumn.Index)
                return;

            var item = _bindingList[e.RowIndex];

            System.Diagnostics.Process.Start(item.Playlist.FullName);
        }

        /// <see cref="http://stackoverflow.com/a/242760/467582"/>
        private void playlistDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks on the header cells
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Ignore readonly (i.e., disabled) cells
            if (_dataGridView[e.ColumnIndex, e.RowIndex].ReadOnly)
                return;

            var column = _dataGridView.Columns[e.ColumnIndex];
            
            if (!(column is DataGridViewComboBoxColumn)) return;

            _dataGridView.BeginEdit(true);

            if (!_dataGridView[e.ColumnIndex, e.RowIndex].Selected) return;

            ((DataGridViewComboBoxEditingControl)_dataGridView.EditingControl).DroppedDown = true;
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Skip if nothing is selected
            if (_dataGridView.SelectedRows.Count == 0 &&
                _dataGridView.SelectedCells.Count == 0) return;

            // User selected entire row
            var rowIndex = _dataGridView.SelectedRows.Count > 0 ? _dataGridView.SelectedRows[0].Index : _dataGridView.SelectedCells[0].RowIndex;

            // Ignore header cells
            if (rowIndex == -1) return;

            var playlistItem = _bindingList[rowIndex];

            if (playlistItem == null) return;

            SelectedPlaylist = playlistItem.Playlist;

            if (SelectedPlaylist == null || rowIndex == _prevRowIndex) return;

            _prevRowIndex = rowIndex;

            if (SelectionChanged != null)
                SelectionChanged.Invoke(this, EventArgs.Empty);
        }

        public bool SelectAll
        {
            set
            {
                foreach (var gridItem in _playlistGridItems.Where(pgi => pgi.Playlist.IsFeatureLength))
                {
                    gridItem.IsMainMovie = value;
                }
            }
        }

        private DataGridViewButtonColumn   _playButtonColumn;
        private DataGridViewCheckBoxColumn _isMainMovieColumn;
        private DataGridViewTextBoxColumn  _filenameColumn;
        private DataGridViewTextBoxColumn  _lengthColumn;
        private DataGridViewTextBoxColumn  _sizeColumn;
        private DataGridViewComboBoxColumn _videoLanguageColumn;
        private DataGridViewComboBoxColumn _cutColumn;
        private DataGridViewCheckBoxColumn _hasCommentaryColumn;

        private void CreateColumns()
        {
            _dataGridView.Columns.Add(_playButtonColumn    = CreatePlayButtonColumn());
            _dataGridView.Columns.Add(_isMainMovieColumn   = CreateIsMainMovieColumn());
            _dataGridView.Columns.Add(_filenameColumn      = CreateFilenameColumn());
            _dataGridView.Columns.Add(_lengthColumn        = CreateLengthColumn());
            _dataGridView.Columns.Add(_sizeColumn          = CreateSizeColumn());
            _dataGridView.Columns.Add(_videoLanguageColumn = CreateVideoLanguageColumn());
            _dataGridView.Columns.Add(_cutColumn           = CreateCutColumn());
            _dataGridView.Columns.Add(_hasCommentaryColumn = CreateHasCommentaryColumn());
        }

        private DataGridViewButtonColumn CreatePlayButtonColumn()
        {
            // Add a CellClick handler to handle clicks in the button column.
            _dataGridView.CellClick += playButton_CellClick;

            return new DataGridViewButtonColumn
                       {
                           Text = "Play",
                           UseColumnTextForButtonValue = true
                       };
        }

        private DataGridViewCheckBoxColumn CreateIsMainMovieColumn()
        {
            return new DataGridViewCheckBoxColumn
                       {
                           Name = "Main Movie",
                           DataPropertyName = "IsMainMovie"
                       };
        }

        private DataGridViewTextBoxColumn CreateFilenameColumn()
        {
            return new DataGridViewTextBoxColumn
                       {
                           Name = "Filename",
                           DataPropertyName = "Filename",
                           ReadOnly = true
                       };
        }

        private DataGridViewTextBoxColumn CreateLengthColumn()
        {
            return new DataGridViewTextBoxColumn
                       {
                           Name = "Length",
                           DataPropertyName = "Length",
                           DefaultCellStyle = {Alignment = DataGridViewContentAlignment.MiddleRight},
                           ReadOnly = true
                       };
        }

        private DataGridViewTextBoxColumn CreateSizeColumn()
        {
            return new DataGridViewTextBoxColumn
                       {
                           Name = "Size (Bytes)",
                           DataPropertyName = "Size",
                           DefaultCellStyle = {Alignment = DataGridViewContentAlignment.MiddleRight},
                           ReadOnly = true
                       };
        }

        private DataGridViewComboBoxColumn CreateVideoLanguageColumn()
        {
            return new DataGridViewComboBoxColumn
                       {
                           Name = "Video Language",
                           DataSource = _languageCodes.ToArray(),
                           DataPropertyName = "VideoLanguage"
                       };
        }

        private DataGridViewComboBoxColumn CreateCutColumn()
        {
            return new DataGridViewComboBoxColumn
                       {
                           Name = "Cut",
                           DataSource = Enum.GetValues(typeof (Cut)),
                           DataPropertyName = "Cut"
                       };
        }

        private DataGridViewCheckBoxColumn CreateHasCommentaryColumn()
        {
            return new DataGridViewCheckBoxColumn
                       {
                           Name = "Commentary Available",
                           DataPropertyName = "HasCommentary"
                       };
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

        private PlaylistGridItem _savedState;

        private TSPlaylistFile _playlist;
        private bool _isMainMovie;
        private double _length;
        private string _iso6392;
        private Cut _cut;
        private bool _hasCommentary;

        public PlaylistGridItem(TSPlaylistFile playlist, string iso6392)
            : this(playlist, iso6392, true)
        {

        }

        private PlaylistGridItem(TSPlaylistFile playlist, string iso6392, bool clone)
        {
            VideoLanguageHasChanged = false;
            _playlist = playlist;
            _isMainMovie = playlist.IsFeatureLength && !playlist.HasDuplicateClips && !playlist.IsDuplicate;
            Filename = playlist.Name;
            _length = playlist.TotalLength;
            Size = playlist.FileSize.ToString("N0");
            _iso6392 = iso6392;
            _cut = Cut.Theatrical;
            _hasCommentary = false;

            if ( clone )
                _savedState = Clone();
        }

        public PlaylistGridItem Clone()
        {
            var clone = new PlaylistGridItem(_playlist, _iso6392, false);
            CopyTo(clone);
            return clone;
        }

        public void CopyTo(PlaylistGridItem that)
        {
            that._playlist = _playlist;
            that._isMainMovie = _isMainMovie;
            that.Filename = Filename;
            that._length = _length;
            that.Size = Size;
            that._iso6392 = _iso6392;
            that._cut = _cut;
            that._hasCommentary = _hasCommentary;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public TSPlaylistFile Playlist
        {
            get { return _playlist; }
        }

        public bool IsMainMovie
        {
            get { return _isMainMovie; }
            set
            {
                if (_isMainMovie == value) return;

                _isMainMovie = value;
                OnPropertyChanged("IsMainMovie");
            }
        }

// ReSharper disable MemberCanBePrivate.Global
        public string Filename { get; private set; }
// ReSharper restore MemberCanBePrivate.Global

// ReSharper disable MemberCanBePrivate.Global
        public string Length
// ReSharper restore MemberCanBePrivate.Global
        {
            get { return HumanFriendlyLength(_length); }
        }

// ReSharper disable MemberCanBePrivate.Global
        public string Size { get; private set; }
// ReSharper restore MemberCanBePrivate.Global

        public string VideoLanguage
        {
            get { return _iso6392; }
            // setter is required for data binding to work
// ReSharper disable UnusedMember.Global
            set
// ReSharper restore UnusedMember.Global
            {
                if (_iso6392 == value) return;

                _iso6392 = value;
                VideoLanguageHasChanged = true;
                OnPropertyChanged("VideoLanguage");
            }
        }

        public string VideoLanguageAuto
        {
            set { _iso6392 = value; }
        }

        public string VideoLanguageName
        {
            get { return Language.GetName(_iso6392); }
// ReSharper disable ValueParameterNotUsed
            set { } // empty setter is required for data binding to work
// ReSharper restore ValueParameterNotUsed
        }

        public bool VideoLanguageHasChanged { get; private set; }

        public Cut Cut
        {
            get { return _cut; }
            set
            {
                if (_cut == value) return;

                _cut = value;
                OnPropertyChanged("Cut");
            }
        }

        public bool HasCommentary
        {
            get { return _hasCommentary; }
            set
            {
                if (_hasCommentary == value) return;

                _hasCommentary = value;
                OnPropertyChanged("HasCommentary");
            }
        }

        public JsonPlaylist JsonPlaylist
        {
            get
            {
                return new JsonPlaylist
                           {
                               filename = Filename,
                               filesize = Playlist.FileSize,
                               length_sec = (int) Playlist.TotalLength,
                               ISO_639_2 = VideoLanguage,
                               is_main = IsMainMovie,
                               is_theatrical = Cut.Equals(Cut.Theatrical),
                               is_special = Cut.Equals(Cut.Special),
                               is_extended = Cut.Equals(Cut.Extended),
                               is_unrated = Cut.Equals(Cut.Unrated),
                               has_commentary = HasCommentary
                           };
            }
        }

        private static string HumanFriendlyLength(double length)
        {
            var playlistLengthSpan = new TimeSpan((long)(length * 10000000));
            return string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                playlistLengthSpan.Hours,
                playlistLengthSpan.Minutes,
                playlistLengthSpan.Seconds);
        }

        public override int GetHashCode()
        {
            return ("" + Playlist.GetHashCode() + IsMainMovie + Filename + Length + Size + _iso6392 + Cut + HasCommentary).GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var that = obj as PlaylistGridItem;
            if (that == null)
            {
                return false;
            }

            return
                _playlist == that._playlist &&
                _isMainMovie == that._isMainMovie &&
                Filename == that.Filename &&
                _length == that._length &&
                Size == that.Size &&
                _iso6392 == that._iso6392 &&
                _cut == that._cut &&
                _hasCommentary == that._hasCommentary;
        }
    }
}
