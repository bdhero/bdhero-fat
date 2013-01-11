using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using BDAutoMuxer.Properties;
using BDAutoMuxer.models;
using BDAutoMuxerCore.BDInfo;
using DotNetUtils;
using MediaInfoWrapper;

namespace BDAutoMuxer.Views
{
    class PlaylistDataGridPopulator
    {
        private const bool DisableShortPlaylists = false;

        private bool _showLowQuality;
        private bool _showBogus;
        private bool _showShort;

        private readonly DataGridView _dataGridView;
        private readonly IList<Language> _languages = new List<Language>();
        private readonly IList<string> _languageCodes;

        private readonly BindingList<PlaylistGridItem> _bindingList = new BindingList<PlaylistGridItem>();

        private IList<TSPlaylistFile> _playlists;
        private readonly IList<PlaylistGridItem> _playlistGridItems = new List<PlaylistGridItem>();
        private readonly IList<PlaylistGridItem> _playlistGridItemsOriginal = new List<PlaylistGridItem>();

        public event EventHandler OnSelectionChange;
        public event EventHandler OnItemChange;

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

        private static IList<TSPlaylistFile> SortPlaylists(IEnumerable<TSPlaylistFile> playlists)
        {
            return playlists.OrderBy(p => p.Rank).ToList();
        }

        public PlaylistDataGridPopulator(DataGridView dataGridView, IEnumerable<TSPlaylistFile> playlists, IList<string> languageCodes)
        {
            _dataGridView = dataGridView;
            _languageCodes = languageCodes;
            _languages.AddRange(_languageCodes.Select(Language.FromCode));

            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.AutoSize = true;

            Init(playlists);
        }

        private void Init(IEnumerable<TSPlaylistFile> playlists)
        {
            _dataGridView.DataSource = null;
            _dataGridView.Rows.Clear();
            _dataGridView.Columns.Clear();

            _playlistGridItems.Clear();
            _playlistGridItemsOriginal.Clear();

            _playlists = SortPlaylists(playlists);

            CreateColumns();

            _dataGridView.CellClick += CellClick;
            _dataGridView.SelectionChanged += SelectionChanged;
            _dataGridView.CellBeginEdit += CellBeginEdit;

            foreach (var playlist in _playlists)
            {
                var item = new PlaylistGridItem(playlist, _languageCodes.Count > 0 ? _languageCodes[0] : null);
                var clone = new PlaylistGridItem(playlist, _languageCodes.Count > 0 ? _languageCodes[0] : null);

                item.PropertyChanged += ItemChanged;

                _playlistGridItems.Add(item);
                _playlistGridItemsOriginal.Add(clone);
            }

            SetVisible(_showLowQuality, _showBogus, _showShort);
        }

        ~PlaylistDataGridPopulator()
        {
            Destroy();
        }

        public void Destroy()
        {
            _dataGridView.CellClick -= CellClick;
            _dataGridView.SelectionChanged -= SelectionChanged;
            _dataGridView.CellBeginEdit -= CellBeginEdit;

            foreach (var pgi in _playlistGridItems)
            {
                pgi.PropertyChanged -= ItemChanged;
            }
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (OnItemChange != null)
                OnItemChange.Invoke(this, EventArgs.Empty);
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

        private static bool ShowPlaylist(PlaylistGridItem item, bool showLowQuality, bool showBogus, bool showShort)
        {
            return item.Playlist.IsLikelyMainMovie || (item.Playlist.IsLowQualityOnly && showLowQuality) || (item.Playlist.IsBogusOnly && showBogus) || (item.Playlist.IsShort && showShort);
        }

        public void SetVisible(bool showLowQuality, bool showBogus, bool showShort)
        {
            _showLowQuality = showLowQuality;
            _showBogus = showBogus;
            _showShort = showShort;

            _dataGridView.DataSource = null;
            _bindingList.Clear();

            var i = 0;
            var enabledRowIndexes = new List<int>();
            foreach (var item in _playlistGridItems.Where(item => ShowPlaylist(item, showLowQuality, showBogus, showShort)))
            {
                _bindingList.Add(item);

                if (item.Playlist.IsFeatureLength)
                    enabledRowIndexes.Add(i);

                i++;
            }

            _dataGridView.DataSource = _bindingList;

            for (var rowIndex = 0; rowIndex < _dataGridView.Rows.Count; rowIndex++)
            {
                TSPlaylistFile tsPlaylistFile = _bindingList[rowIndex].Playlist;
                EnableRow(_dataGridView.Rows[rowIndex], enabledRowIndexes.Contains(rowIndex));
                StyleRow(_dataGridView.Rows[rowIndex], tsPlaylistFile.IsLowQualityOnly, tsPlaylistFile.IsBogusOnly);
            }
        }

        public TSPlaylistFile PlaylistAt(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _dataGridView.Rows.Count)
            {
                throw new IndexOutOfRangeException("Playlist row index " + rowIndex + " is out of range [0, " + _dataGridView.Rows.Count + "]");
            }
            
            var item = _dataGridView.Rows[rowIndex].DataBoundItem as PlaylistGridItem;
            
            return item == null ? null : (item).Playlist;
        }

        private void EnableRow(DataGridViewBand row, bool enabled)
        {
            if (DisableShortPlaylists)
                row.ReadOnly = !enabled;

            for (var colIndex = 0; colIndex < _dataGridView.Columns.Count; colIndex++)
            {
                EnableCell(_dataGridView[colIndex, row.Index], enabled);
            }
        }

        private void StyleRow(DataGridViewBand row, bool isLowQuality, bool isBogus)
        {
            for (var colIndex = 0; colIndex < _dataGridView.Columns.Count; colIndex++)
            {
                StyleCell(_dataGridView[colIndex, row.Index], isLowQuality, isBogus);
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
            if (DisableShortPlaylists)
                dc.ReadOnly = !enabled;

            if (enabled)
            {
                // restore cell style to the default value
                dc.Style.BackColor = dc.OwningColumn.DefaultCellStyle.BackColor;
                dc.Style.ForeColor = dc.OwningColumn.DefaultCellStyle.ForeColor;
                dc.Style.SelectionBackColor = dc.OwningColumn.DefaultCellStyle.SelectionBackColor;
                dc.Style.SelectionForeColor = dc.OwningColumn.DefaultCellStyle.SelectionForeColor;
            }
            else
            {
                // gray out the cell
                dc.Style.BackColor = Color.FromArgb(230, 230, 230);
                dc.Style.ForeColor = Color.FromArgb(105, 105, 105);
                dc.Style.SelectionBackColor = Color.FromArgb(105, 105, 105);
                dc.Style.SelectionForeColor = Color.FromArgb(230, 230, 230);
            }
        }

        private static void StyleCell(DataGridViewCell dc, bool isLowQuality, bool isBogus)
        {
            if (isLowQuality)
            {
                dc.Style.BackColor = Color.FromArgb(254, 251, 214);
                dc.Style.ForeColor = Color.FromArgb(213, 162, 62);
                dc.Style.SelectionBackColor = Color.FromArgb(213, 162, 62);
                dc.Style.SelectionForeColor = Color.FromArgb(254, 251, 214);
            }
            else if (isBogus)
            {
                dc.Style.BackColor = Color.FromArgb(255, 239, 239);
                dc.Style.ForeColor = Color.FromArgb(219, 0, 0);
                dc.Style.SelectionBackColor = Color.FromArgb(219, 0, 0);
                dc.Style.SelectionForeColor = Color.FromArgb(255, 239, 239);
            }
        }

        public IEnumerable<JsonPlaylist> JsonPlaylists
        {
            get
            {
                return (from item in _playlistGridItems
                        where item.Playlist.IsFeatureLength
                        select item.JsonPlaylist);
            }
        }

        public ISet<Language> SelectedVideoLanguages
        {
            get
            {
                return new HashSet<Language>(
                    (from item in _playlistGridItems
                     where item.IsMainMovie
                     select item.VideoLanguageObject));
            }
        }

        public ISet<Cut> SelectedCuts
        {
            get
            {
                return new HashSet<Cut>(
                    (from item in _playlistGridItems
                     where item.IsMainMovie
                     select item.Cut));
            }
        }

        public IList<CommentaryOption> SelectedCommentaryOptions
        {
            get
            {
                ISet<CommentaryOption> selectedCommentaryOptionsSet = new HashSet<CommentaryOption>();

                foreach (var item in _playlistGridItems.Where(item => item.IsMainMovie))
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

        public bool SelectedPlaylistsHaveAnyHiddenTracks
        {
            get
            {
                return _playlistGridItems.Any(item => item.IsMainMovie && item.Playlist.HasHiddenTracks);
            }
        }

        public bool SelectedPlaylistsHaveAllHiddenTracks
        {
            get
            {
                return _playlistGridItems.Any(HasAllHiddenTracks);
            }
        }

        private static bool HasAllHiddenTracks(PlaylistGridItem item)
        {
            if (!item.IsMainMovie) return false;
            var playlist = item.Playlist;
            return playlist.AudioStreams
                           .All(stream => stream.IsHidden) ||
                   playlist.GraphicsStreams.Where(stream => stream.StreamType == TSStreamType.PRESENTATION_GRAPHICS)
                           .All(stream => stream.IsHidden);
        }

        public void AutoConfigure(IList<JsonPlaylist> jsonPlaylists)
        {
            _dataGridView.EndEdit();

            // TODO: Clean this garbage up
            foreach (var playlist in _playlists)
            {
                playlist.IsMainMovieAuto = false;

                var playlist1 = playlist;
                var playlistGridItem = _playlistGridItems.FirstOrDefault(p => p.Playlist.Name == playlist1.Name);

                if (playlistGridItem != null)
                {
                    playlist.IsMainMovieAuto = playlistGridItem.IsMainMovie;
                }
            }

            foreach (var jsonPlaylist in jsonPlaylists)
            {
                var key = jsonPlaylist.filename.ToUpperInvariant();
                var playlist = _playlists.FirstOrDefault(tsPlaylistFile => tsPlaylistFile.Name.ToUpperInvariant() == key);

                if (playlist == null) continue;

                playlist.IsMainMovieAuto = jsonPlaylist.is_main;
            }

            // Re-sort the playlists in the GridView
            Destroy();
            Init(_playlists);

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
            return new HashSet<TSPlaylistFile>(_playlistGridItems.Where(item => item.IsMainMovie == mainMovie).Select(item => item.Playlist));
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithVideoLanguage(Language lang)
        {
            return new HashSet<TSPlaylistFile>(_playlistGridItems.Where(item => item.VideoLanguageObject == lang).Select(item => item.Playlist));
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithCut(Cut cut)
        {
            return new HashSet<TSPlaylistFile>(_playlistGridItems.Where(item => item.Cut == cut).Select(item => item.Playlist));
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithCommentaryOption(CommentaryOption commentaryOption)
        {
            return new HashSet<TSPlaylistFile>(_playlistGridItems.Where(item => ShouldIncludePlaylist(item, commentaryOption)).Select(item => item.Playlist));
        }

        private static bool ShouldIncludePlaylist(PlaylistGridItem item, CommentaryOption commentaryOption)
        {
            var isAny = commentaryOption == CommentaryOption.Any;
            var isYes = item.HasCommentary && commentaryOption == CommentaryOption.Yes;
            var isNo = !item.HasCommentary && commentaryOption == CommentaryOption.No;
            return isAny || isYes || isNo;
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithAudioLanguages(ICollection<Language> audioLanguages)
        {
            return new HashSet<TSPlaylistFile>(_playlistGridItems.Select(item => item.Playlist).Where(playlist => playlist.AudioStreams.Any(audioStream => audioLanguages.Contains(audioStream.Language))));
        }

        public IEnumerable<TSPlaylistFile> GetPlaylistsWithSubtitleLanguages(ICollection<Language> subtitleLanguages)
        {
            ISet<TSPlaylistFile> files = new HashSet<TSPlaylistFile>();

            files.AddRange(
                _playlistGridItems.Select(item => item.Playlist)
                    .Where(playlist => playlist.GraphicsStreams.Any(graphicsStream => graphicsStream.StreamType == TSStreamType.PRESENTATION_GRAPHICS && subtitleLanguages.Contains(graphicsStream.Language)))
            );
            files.AddRange(
                _playlistGridItems.Select(item => item.Playlist)
                    .Where(playlist => playlist.TextStreams.Any(textStream => subtitleLanguages.Contains(textStream.Language)))
            );

            return files;
        }

        private static void AutoConfigure(JsonPlaylist jsonPlaylist, IDictionary<string, PlaylistGridItem> gridItems)
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

        private void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == _filenameColumn.Index || e.ColumnIndex == _lengthColumn.Index || e.ColumnIndex == _sizeColumn.Index)
                e.Cancel = true;
        }

        private static bool IsHeaderCell(DataGridViewCellEventArgs e)
        {
            return e.RowIndex < 0 || e.ColumnIndex < 0;
        }

        private bool IsReadOnly(DataGridViewCellEventArgs e)
        {
            return _dataGridView[e.ColumnIndex, e.RowIndex].ReadOnly;
        }

        private bool IsComboBoxColumn(DataGridViewCellEventArgs e)
        {
            return _dataGridView.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn;
        }

        private bool IsPlayButton(DataGridViewCellEventArgs e)
        {
            return e.RowIndex >= 0 && e.ColumnIndex == _playButtonColumn.Index;
        }

        private bool IsSelected(DataGridViewCellEventArgs e)
        {
            return _dataGridView[e.ColumnIndex, e.RowIndex].Selected;
        }

        private DataGridViewComboBoxEditingControl GetCurComboBox()
        {
            return _dataGridView.EditingControl as DataGridViewComboBoxEditingControl;
        }

        /// <see cref="http://stackoverflow.com/a/242760/467582"/>
        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks on the header cells
            if (IsHeaderCell(e))
                return;

            if (IsPlayButton(e))
            {
                Process.Start(_bindingList[e.RowIndex].Playlist.FullName);
                return;
            }

            // Ignore readonly (i.e., disabled) cells
            if (IsReadOnly(e) && DisableShortPlaylists)
                return;

            if (IsComboBoxColumn(e))
            {
                _dataGridView.BeginEdit(true);

                if (IsSelected(e) && GetCurComboBox() != null)
                    GetCurComboBox().DroppedDown = true;
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
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

            if (OnSelectionChange != null)
                OnSelectionChange.Invoke(this, EventArgs.Empty);
        }

        public bool SelectFeatureLength
        {
            set
            {
                foreach (var gridItem in _playlistGridItems)
                {
                    gridItem.IsMainMovie = gridItem.Playlist.IsFeatureLength && value;
                }
            }
        }

        public bool SelectLikely
        {
            set
            {
                foreach (var gridItem in _playlistGridItems)
                {
                    gridItem.IsMainMovie = gridItem.Playlist.IsLikelyMainMovie && value;
                }
            }
        }

        private DataGridViewImageColumn    _rankImageColumn;
        private DataGridViewButtonColumn   _playButtonColumn;
        private DataGridViewCheckBoxColumn _isMainMovieColumn;
        private DataGridViewTextBoxColumn  _filenameColumn;
        private DataGridViewTextBoxColumn  _lengthColumn;
        private DataGridViewTextBoxColumn  _sizeColumn;
        private DataGridViewTextBoxColumn  _chaptersColumn;
        private DataGridViewComboBoxColumn _videoLanguageColumn;
        private DataGridViewComboBoxColumn _cutColumn;
        private DataGridViewCheckBoxColumn _hasCommentaryColumn;

        private void CreateColumns()
        {
            _dataGridView.Columns.Add(_rankImageColumn     = CreateRankImageColumn());
            _dataGridView.Columns.Add(_playButtonColumn    = CreatePlayButtonColumn());
            _dataGridView.Columns.Add(_isMainMovieColumn   = CreateIsMainMovieColumn());
            _dataGridView.Columns.Add(_filenameColumn      = CreateFilenameColumn());
            _dataGridView.Columns.Add(_lengthColumn        = CreateLengthColumn());
            _dataGridView.Columns.Add(_chaptersColumn      = CreateChaptersColumn());
            _dataGridView.Columns.Add(_sizeColumn          = CreateSizeColumn());
            _dataGridView.Columns.Add(_videoLanguageColumn = CreateVideoLanguageColumn());
            _dataGridView.Columns.Add(_cutColumn           = CreateCutColumn());
            _dataGridView.Columns.Add(_hasCommentaryColumn = CreateHasCommentaryColumn());
        }

        private DataGridViewImageColumn CreateRankImageColumn()
        {
            _dataGridView.CellToolTipTextNeeded += DataGridViewOnCellToolTipTextNeeded;
            return new DataGridViewImageColumn
                       {
                           Name = "Type",
                           DataPropertyName = "RankImage"
                       };
        }

        private void DataGridViewOnCellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            var row = e.RowIndex;
            var col = e.ColumnIndex;
            if (row > -1 && col == _rankImageColumn.Index)
            {
                var playlist = PlaylistAt(row);
                if (playlist != null)
                {
                    e.ToolTipText = playlist.RankToolTipText;
                }
            }
        }

        private DataGridViewButtonColumn CreatePlayButtonColumn()
        {
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

        private DataGridViewTextBoxColumn CreateChaptersColumn()
        {
            return new DataGridViewTextBoxColumn
                       {
                           Name = "Chapters",
                           DataPropertyName = "ChapterCount",
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
        private const double Epsilon = 0.1;

        public event PropertyChangedEventHandler PropertyChanged;

        private PlaylistGridItem _savedState;

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
            Playlist = playlist;
            Filename = playlist.Name;
            Size = playlist.FileSize.ToString("N0");

            _length = playlist.TotalLength;
            ChapterCount = playlist.Chapters.Count;
            _isMainMovie = playlist.IsLikelyMainMovie;
            _iso6392 = iso6392;
            _cut = Cut.Theatrical;
            _hasCommentary = false;

            if ( clone )
                _savedState = Clone();
        }

        public PlaylistGridItem Clone()
        {
            var clone = new PlaylistGridItem(Playlist, _iso6392, false);
            CopyTo(clone);
            return clone;
        }

        public void CopyTo(PlaylistGridItem that)
        {
            that.Playlist = Playlist;
            that.Filename = Filename;
            that.Size = Size;
            that._length = _length;
            that.ChapterCount = ChapterCount;
            that._isMainMovie = _isMainMovie;
            that._iso6392 = _iso6392;
            that._cut = _cut;
            that._hasCommentary = _hasCommentary;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public TSPlaylistFile Playlist { get; private set; }

        public Image RankImage
        {
            get
            {
                var rank = Playlist.Rank;
                if (rank == TSPlaylistRank.MainMovieHq) return Resources.bullet_green;
                if (rank == TSPlaylistRank.MainMovieLq) return Resources.bullet_error;
                if (rank == TSPlaylistRank.BogusFeature) return Resources.bullet_delete;
                return Resources.bullet_black;
            }
            // Empty setter necessary for reflection to work
            set
            {}
        }

// ReSharper disable MemberCanBePrivate.Global
        public string Filename { get; private set; }

        public string Size { get; private set; }

        public int ChapterCount { get; private set; }

        public string Length
        {
            get { return HumanFriendlyLength(_length); }
        }
// ReSharper restore MemberCanBePrivate.Global

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

        public Language VideoLanguageObject
        {
            get { return Language.FromCode(VideoLanguage); }
        }

// ReSharper disable UnusedMember.Global
        public string VideoLanguage
        {
            get { return _iso6392; }
            // setter is required for data binding to work
            set
            {
                if (_iso6392 == value) return;

                _iso6392 = value;
                VideoLanguageHasChanged = true;
                OnPropertyChanged("VideoLanguage");
            }
        }
// ReSharper restore UnusedMember.Global

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
                Playlist == that.Playlist &&
                Filename == that.Filename &&
                Size == that.Size &&
                Math.Abs(_length - that._length) < Epsilon &&
                _isMainMovie == that._isMainMovie &&
                _iso6392 == that._iso6392 &&
                _cut == that._cut &&
                _hasCommentary == that._hasCommentary;
        }
    }
}
