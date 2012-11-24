using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BDAutoMuxer.Properties;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.tools;
using BDAutoMuxer.views;
using Microsoft.WindowsAPICodePack.Taskbar;
using WatTmdb.V3;

// ReSharper disable SuggestUseVarKeywordEvident
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable LocalizableElement
namespace BDAutoMuxer
{
    public partial class FormDetails : Form
    {
        #region Fields

        private const string TmdbApiKey = "b59b366b0f0a457d58995537d847409a";

        private bool _initialized;
        private bool _isScanningBDROM;
        private bool _isSearchingMainMovieDb;
        private bool _isSearchingTmdb;
        private bool _isMuxing;

        private Tmdb _tmdbApi;

        private BDROM _bdrom;
        private IList<TSPlaylistFile> _playlists;
        private IList<Language> _languages;
        private IList<string> _languageCodes;

        private TmdbMovieSearch _tmdbMovieSearch;
        private MovieResult _tmdbMovieResult;
        private string _rootUrl;

        private readonly MainMovieService _mainMovieService = new MainMovieService();
        private JsonSearchResult _mainMovieSearchResult;

        private BackgroundWorker _mainMovieBackgroundWorker;
        private BackgroundWorker _tmdbBackgroundWorker;

        private PlaylistDataGridPopulator _populator;

        private bool _autoConfigured;
        private int _autoTmdbId = -1;

        private string _tmdbMovieTitle;
        private int? _tmdbMovieYear;
        private string _tmdbMovieUrl;

        private bool _ignoreFilterControlChange;
        private bool _ignoreDataGridItemChange;

        private TsMuxer _tsMuxer;
        private string _tsMuxerOutputPath;

        private ISet<TSPlaylistFile> _filteredPlaylists = new HashSet<TSPlaylistFile>();

        private readonly IList<Language> _audioLanguages = new List<Language>();
        private readonly IList<Language> _subtitleLanguages = new List<Language>();

        private readonly List<TSVideoStream> _videoTracks = new List<TSVideoStream>();
        private readonly List<TSAudioStream> _audioTracks = new List<TSAudioStream>();
        private readonly List<TSStream> _subtitleTracks = new List<TSStream>();

        private readonly Dictionary<TabPage, string> _tabStatusMessages = new Dictionary<TabPage, string>();
        private readonly Dictionary<Control, String> _controlHints = new Dictionary<Control, String>();

        private readonly PlaylistFinder _playlistFinder = new PlaylistFinder();

        #endregion

        #region Properties

        private bool CanDragAndDrop
        {
            get { return !_isScanningBDROM && !_isSearchingMainMovieDb && !_isSearchingTmdb && !_isMuxing; }
        }

        private Language[] GetSortedLanguageArray(ICollection<Language> collection)
        {
            int i = 0;
            Language[] array = new Language[collection.Count];

            foreach (var value in _languages.Where(collection.Contains))
            {
                array[i++] = value;
            }

            return array;
        }

        private Language[] VideoLanguages
        {
            get
            {
                return GetSortedLanguageArray(_populator.SelectedVideoLanguages);
            }
        }

        private Language[] AudioLanguages
        {
            get
            {
                ISet<Language> audioLanguagesSet = new HashSet<Language>();
                foreach (TSAudioStream audioStream in _playlists.SelectMany(playlist => playlist.AudioStreams))
                {
                    audioLanguagesSet.Add(Language.GetLanguage(audioStream.LanguageCode));
                }
                return GetSortedLanguageArray(audioLanguagesSet);
            }
        }

        private Language[] SubtitleLanguages
        {
            get
            {
                ISet<Language> subtitleLanguagesSet = new HashSet<Language>();
                foreach (TSPlaylistFile playlist in _playlists)
                {
                    foreach (TSGraphicsStream graphicsStream in playlist.GraphicsStreams)
                    {
                        subtitleLanguagesSet.Add(Language.GetLanguage(graphicsStream.LanguageCode));
                    }
                    foreach (TSTextStream textStream in playlist.TextStreams)
                    {
                        subtitleLanguagesSet.Add(Language.GetLanguage(textStream.LanguageCode));
                    }
                }
                return GetSortedLanguageArray(subtitleLanguagesSet);
            }
        }

        private Cut[] GetSortedCutArray(ICollection<Cut> collection)
        {
            int i = 0;
            Cut[] array = new Cut[collection.Count];

            foreach (Cut value in Enum.GetValues(typeof(Cut)).Cast<Cut>().Where(collection.Contains))
            {
                array[i++] = value;
            }

            return array;
        }

        private Cut[] Cuts
        {
            get
            {
                return GetSortedCutArray(_populator.SelectedCuts);
            }
        }

        private CommentaryOption[] GetSortedCommentaryOptionArray(ICollection<CommentaryOption> collection)
        {
            int i = 0;
            CommentaryOption[] array = new CommentaryOption[collection.Count];

            foreach (CommentaryOption value in Enum.GetValues(typeof(CommentaryOption)).Cast<CommentaryOption>().Where(collection.Contains))
            {
                array[i++] = value;
            }

            return array;
        }

        private CommentaryOption[] CommentaryOptions
        {
            get
            {
                return GetSortedCommentaryOptionArray(_populator.SelectedCommentaryOptions);
            }
        }

        private TSPlaylistFile SelectedPlaylist
        {
            get { return comboBoxPlaylist.SelectedValue as TSPlaylistFile; }
        }

        private List<TSVideoStream> SelectedVideoStreams
        {
            get
            {
                return (from ListViewItem item in listViewVideoTracks.CheckedItems
                        select item.Tag as TSVideoStream).ToList();
            }
        }

        private List<TSAudioStream> SelectedAudioStreams
        {
            get
            {
                return (from ListViewItem item in listViewAudioTracks.CheckedItems
                        select item.Tag as TSAudioStream).ToList();
            }
        }

        private List<TSStream> SelectedSubtitleStreams
        {
            get
            {
                return (from ListViewItem item in listViewSubtitleTracks.CheckedItems
                        select item.Tag as TSStream).ToList();
            }
        }

        private string OutputFilePath
        {
            get { return Path.Combine(textBoxOutputDirPreview.Text, textBoxOutputFileNamePreview.Text); }
        }

        #endregion

        #region Initialization

        public FormDetails(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
            {
                string path = args[0];
                textBoxSource.Text = path;
                Scan(path);
            }
            else
            {
                textBoxSource.Text = BDAutoMuxerSettings.LastPath;
            }

            Load += FormDetails_Load;
        }

        ~FormDetails()
        {
            CancelRip();
        }

        private void FormDetails_Load(object sender, EventArgs e)
        {
            if (BDAutoMuxerSettings.DetailsWindowMaximized)
                WindowState = FormWindowState.Maximized;

            if (BDAutoMuxerSettings.DetailsWindowLocation != Point.Empty)
                Location = BDAutoMuxerSettings.DetailsWindowLocation;

            if (BDAutoMuxerSettings.DetailsWindowSize != Size.Empty)
                Size = BDAutoMuxerSettings.DetailsWindowSize;

            Text = BDAutoMuxerSettings.AssemblyName + " v" + BDAutoMuxerSettings.AssemblyVersionDisplay;

            FormUtils.TextBox_EnableSelectAll(this);

            ShowProgressTabPage = false;

            textBoxOutputDir.Text = BDAutoMuxerSettings.OutputDir;
            textBoxOutputFileName.Text = BDAutoMuxerSettings.OutputFileName;

            textBoxReplaceSpaces.Text = BDAutoMuxerSettings.ReplaceSpacesWith;
            checkBoxReplaceSpaces.Checked = BDAutoMuxerSettings.ReplaceSpaces;

            comboBoxAudienceLanguage.SelectedIndexChanged += OnAudienceLanguageChange;
            playlistDataGridView.CurrentCellDirtyStateChanged += playlistDataGridView_CurrentCellDirtyStateChanged;
            pictureBoxMoviePoster.MouseEnter += (o, args) => SetTabStatus(_tmdbMovieUrl, true);
            pictureBoxMoviePoster.MouseLeave += (o, args) => RestoreTabStatus();

            DragLeave += FormDetails_DragLeave;
            textBoxOutputDir.DragLeave += textBoxOutputDir_DragLeave;
            textBoxOutputFileName.DragLeave += textBoxOutputFileName_DragLeave;

            _playlistFinder.ScanSuccess += ScanSuccess;
            _playlistFinder.ScanError += ScanError;
            _playlistFinder.ScanCompleted += ScanCompleted;

            textBoxOutputFileNameHint.Parent.BackColorChanged += (o, args) => UpdateBackgroundColors();
            textBoxOutputFileNamePreview.Parent.BackColorChanged += (o, args) => UpdateBackgroundColors();
            textBoxOutputDirPreview.Parent.BackColorChanged += (o, args) => UpdateBackgroundColors();
            UpdateBackgroundColors();

            InitHints(this);

            ResetUI();

            CheckForUpdates();
        }

        private void ResetState()
        {
            _initialized = false;
            _isScanningBDROM = false;
            _isSearchingMainMovieDb = false;
            _isSearchingTmdb = false;
            _isMuxing = false;

            _autoConfigured = false;
            _autoTmdbId = -1;

            _filteredPlaylists.Clear();

            _audioLanguages.Clear();
            _subtitleLanguages.Clear();

            _videoTracks.Clear();
            _audioTracks.Clear();
            _subtitleTracks.Clear();
        }

        private void Init(BDROM bdrom, ICollection<Language> languages, List<TSPlaylistFile> playlists)
        {
            _bdrom = bdrom;
            _languages = new List<Language>(languages).ToArray();
            _languageCodes = new List<string>();
            _playlists = TSPlaylistFile.Sort(playlists);
            _tmdbMovieUrl = null;

            string ISO_639_1 = bdrom.DiscLanguage != null ? bdrom.DiscLanguage.ISO_639_1 : null;
            string ISO_639_2 = bdrom.DiscLanguage != null ? bdrom.DiscLanguage.ISO_639_2 : null;

            // TODO: This will fail if we're unable to auto-detect the disc language (e.g., ID4)
            //       or if the user changes the main disc language manually.
            _tmdbApi = new Tmdb(TmdbApiKey, ISO_639_1);

            _languageCodes.AddRange(languages.Select(lang => lang.ISO_639_2));

            if (_populator != null)
            {
                _populator.Destroy();
                _populator = null;
            }

            _populator = new PlaylistDataGridPopulator(playlistDataGridView, _playlists, _languageCodes);
            _populator.ItemChanged += OnPlaylistItemChange;
            _populator.SelectionChanged += playlistDataGridView_SelectionChanged;
            _populator.MainLanguageCode = ISO_639_2;

            _initialized = true;

            ResetUI();

            movieNameTextBox.Text = String.IsNullOrEmpty(_bdrom.DiscNameSearchable) ? _bdrom.VolumeLabel : _bdrom.DiscNameSearchable;
            movieNameTextBox.Text = Regex.Replace(movieNameTextBox.Text.Trim(), @"^(.*), (A|The)$", "$2 $1", RegexOptions.IgnoreCase);
            movieNameTextBox.Text = movieNameTextBox.Text.Replace("-", " ");

            discLanguageComboBox.DataSource = new List<Language>(_languages).ToArray();

            ResetPlaylistDataGrid();
            PopulateOutputTab();
            QueryMainMovie();

            textBoxOutputDir_TextChanged(this, EventArgs.Empty);
            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);

            if (playlistDataGridView.Rows.Count > 0)
            {
                playlistDataGridView.Rows[0].Selected = true;
                playlistDataGridView_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void UpdateBackgroundColors()
        {
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                textBoxOutputFileNameHint.BackColor = textBoxOutputFileNameHint.Parent.BackColor;
                textBoxOutputFileNamePreview.BackColor = textBoxOutputFileNamePreview.Parent.BackColor;
                textBoxOutputDirPreview.BackColor = textBoxOutputDirPreview.Parent.BackColor;
            }
            catch
            {
            }
            // ReSharper restore EmptyGeneralCatchClause
        }

        #endregion

        #region BD-ROM Scanning

        private void Scan(string path)
        {
            ResetState();

            _initialized = false;
            _isScanningBDROM = true;

            ResetUI();

            SetTabStatus("Scanning BD-ROM...");

            _playlistFinder.InitBDROM(path);
        }

        private void ScanSuccess(object sender, EventArgs e)
        {
            var scanResult = _playlistFinder.ScanResult;

            Init(scanResult.BDROM, scanResult.Languages, scanResult.SortedPlaylists);

            tabControl.SelectedIndex = 0;

            SetTabStatus("");

            BDAutoMuxerSettings.LastPath = textBoxSource.Text;
            BDAutoMuxerSettings.SaveSettings();
        }

        private void ScanError(object sender, ErrorEventArgs e)
        {
            SetTabStatus(e.GetException().Message);
        }

        private void ScanCompleted(object sender, EventArgs e)
        {
            _isScanningBDROM = false;
            ResetUI();
        }

        #endregion

        #region "Disc" Tab

        private void ResizeDiscTab(object sender = null, EventArgs e = null)
        {
            var width = searchResultListView.ClientSize.Width;
            var columns = searchResultListView.Columns;
            columns[0].Width = width - columns[1].Width - columns[2].Width;

            listViewStreamFiles.Columns[0].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.30);
            listViewStreamFiles.Columns[1].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.10);
            listViewStreamFiles.Columns[2].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.30);
            listViewStreamFiles.Columns[3].Width =
                (int)(listViewStreamFiles.ClientSize.Width * 0.30);

            listViewStreams.Columns[0].Width =
                (int)(listViewStreams.ClientSize.Width * 0.25);
            listViewStreams.Columns[1].Width =
                (int)(listViewStreams.ClientSize.Width * 0.15);
            listViewStreams.Columns[2].Width =
                (int)(listViewStreams.ClientSize.Width * 0.15);
            listViewStreams.Columns[3].Width =
                (int)(listViewStreams.ClientSize.Width * 0.45);
        }

        private void ResetPlaylistDataGrid()
        {
            if (_populator != null)
                _populator.ShowAllPlaylists = showAllPlaylistsCheckbox.Checked;
        }

        #endregion

        #region "Output" Tab

        private void PopulateOutputTab()
        {
            _ignoreFilterControlChange = true;

            comboBoxAudienceLanguage.DataSource = null;
            comboBoxAudienceLanguage.DataSource = new List<Language>(_languages).ToArray();
            comboBoxAudienceLanguage.Enabled = _languages.Count > 1;

            Language[] selectedVideoLanguages = VideoLanguages;
            comboBoxVideoLanguage.DataSource = null;
            comboBoxVideoLanguage.DataSource = selectedVideoLanguages;
            comboBoxVideoLanguage.Enabled = selectedVideoLanguages.Length > 1;

            Cut[] selectedCuts = Cuts;
            comboBoxCut.DataSource = null;
            comboBoxCut.DataSource = selectedCuts;
            comboBoxCut.Enabled = selectedCuts.Length > 1;

            CommentaryOption[] selectedCommentaryOptions = CommentaryOptions;
            comboBoxCommentary.DataSource = null;
            comboBoxCommentary.DataSource = selectedCommentaryOptions;
            comboBoxCommentary.Enabled = selectedCommentaryOptions.Length > 1;

            Language[] audioLanguages = AudioLanguages;
            listBoxAudioLanguages.DataSource = null;
            listBoxAudioLanguages.DataSource = audioLanguages;
            listBoxAudioLanguages.Enabled = audioLanguages.Length > 1;

            Language[] subtitleLanguages = AudioLanguages;
            listBoxSubtitleLanguages.DataSource = null;
            listBoxSubtitleLanguages.DataSource = SubtitleLanguages;
            listBoxSubtitleLanguages.Enabled = subtitleLanguages.Length > 1;

            _ignoreFilterControlChange = false;

            FilterPlaylists();
        }

        private void FilterPlaylists()
        {
            if (_ignoreFilterControlChange || !_initialized)
                return;

            _filteredPlaylists = new HashSet<TSPlaylistFile>(_playlists);

            Language videoLanguage = comboBoxVideoLanguage.SelectedValue as Language;
            Cut cut = (Cut)comboBoxCut.SelectedValue;
            CommentaryOption commentaryOption = (CommentaryOption)comboBoxCommentary.SelectedValue;
            _audioLanguages.Clear();
            _subtitleLanguages.Clear();

            foreach (Language lang in listBoxAudioLanguages.SelectedItems.OfType<Language>())
                _audioLanguages.Add(lang);

            foreach (Language lang in listBoxSubtitleLanguages.SelectedItems.OfType<Language>())
                _subtitleLanguages.Add(lang);

            if (videoLanguage == null) return;

            IEnumerable<TSPlaylistFile> playlistsWithMainMovie = _populator.GetPlaylistsWithMainMovie(true);
            IEnumerable<TSPlaylistFile> playlistsWithVideoLanguage = _populator.GetPlaylistsWithVideoLanguage(videoLanguage);
            IEnumerable<TSPlaylistFile> playlistsWithCut = _populator.GetPlaylistsWithCut(cut);
            IEnumerable<TSPlaylistFile> playlistsWithCommentaryOption = _populator.GetPlaylistsWithCommentaryOption(commentaryOption);
            IEnumerable<TSPlaylistFile> playlistsWithAudioLanguages = _populator.GetPlaylistsWithAudioLanguages(_audioLanguages);
            IEnumerable<TSPlaylistFile> playlistsWithSubtitleLanguages = _populator.GetPlaylistsWithSubtitleLanguages(_subtitleLanguages);

            _filteredPlaylists.IntersectWith(playlistsWithMainMovie);
            _filteredPlaylists.IntersectWith(playlistsWithVideoLanguage);
            _filteredPlaylists.IntersectWith(playlistsWithCut);
            _filteredPlaylists.IntersectWith(playlistsWithCommentaryOption);
            _filteredPlaylists.IntersectWith(playlistsWithAudioLanguages);
            _filteredPlaylists.IntersectWith(playlistsWithSubtitleLanguages);

            comboBoxPlaylist.DataSource = null;
            comboBoxPlaylist.DataSource = new List<TSPlaylistFile>(_filteredPlaylists).ToArray();
            comboBoxPlaylist.Enabled = _filteredPlaylists.Count > 1;

            buttonPlaylistOpen.Enabled = SelectedPlaylist != null;

            if (SelectedPlaylist != null)
            {
                var icon = FileUtils.ExtractIconAsBitmap(SelectedPlaylist.FullName);
                if (icon != null)
                    buttonPlaylistOpen.Image = new Bitmap(icon, 16, 16);
            }

            ResetUI();
        }

        private void FilterTracks()
        {
            listViewVideoTracks.Items.Clear();
            listViewAudioTracks.Items.Clear();
            listViewSubtitleTracks.Items.Clear();

            _videoTracks.Clear();
            _audioTracks.Clear();
            _subtitleTracks.Clear();

            if (_filteredPlaylists == null || _filteredPlaylists.Count == 0)
                return;

            TSPlaylistFile playlist = comboBoxPlaylist.SelectedValue as TSPlaylistFile;

            if (playlist == null)
                return;

            foreach (TSStream stream in playlist.SortedStreams)
            {
                if (stream.IsHidden)
                    continue;

                Language lang = !String.IsNullOrEmpty(stream.LanguageCode) ? Language.GetLanguage(stream.LanguageCode) : null;

                if (stream is TSVideoStream)
                    _videoTracks.Add(stream as TSVideoStream);
                else if (stream is TSAudioStream && _audioLanguages.Contains(lang))
                    _audioTracks.Add(stream as TSAudioStream);
                else if (stream is TSGraphicsStream && _subtitleLanguages.Contains(lang))
                    _subtitleTracks.Add(stream);
                else if (stream is TSTextStream && _subtitleLanguages.Contains(lang))
                    _subtitleTracks.Add(stream);
            }

            PopulateVideoTracks();
            PopulateAudioTracks();
            PopulateSubtitleTracks();

            ResizeOutputTab();

            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);
        }

        private void PopulateVideoTracks()
        {
            var i = 0;
            ImageList icons = new ImageList();

            foreach (TSVideoStream stream in _videoTracks)
            {
                ListViewItem.ListViewSubItem codec = new ListViewItem.ListViewSubItem();
                codec.Text = stream.CodecName;
                if (stream.AngleIndex > 0)
                    codec.Text += string.Format(" ({0})", stream.AngleIndex);
                codec.Tag = stream.CodecName;

                ListViewItem.ListViewSubItem resolution = new ListViewItem.ListViewSubItem();
                resolution.Text = stream.HeightDescription;
                resolution.Tag = stream.Height;

                ListViewItem.ListViewSubItem frameRate = new ListViewItem.ListViewSubItem();
                frameRate.Text = stream.FrameRateDescription;
                frameRate.Tag = stream.FrameRate;

                ListViewItem.ListViewSubItem aspectRatio = new ListViewItem.ListViewSubItem();
                aspectRatio.Text = stream.AspectRatioDescription;
                aspectRatio.Tag = stream.AspectRatio;

                ListViewItem.ListViewSubItem[] streamSubItems =
                    new[]
                    {
                        codec,
                        resolution,
                        frameRate,
                        aspectRatio
                    };

                ListViewItem streamItem = new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream;
                streamItem.Checked = true;
                streamItem.ImageIndex = i++;
                listViewVideoTracks.Items.Add(streamItem);

                icons.Images.Add(TSStream.GetCodecIcon(stream.StreamType));
            }

            listViewVideoTracks.SmallImageList = icons;
        }

        private void PopulateAudioTracks()
        {
            var i = 0;
            ImageList icons = new ImageList();

            foreach (TSAudioStream stream in _audioTracks)
            {
                ListViewItem.ListViewSubItem codec = new ListViewItem.ListViewSubItem();
                codec.Text = stream.CodecName;
                if (stream.AngleIndex > 0)
                    codec.Text += string.Format(" ({0})", stream.AngleIndex);
                codec.Tag = stream.CodecName;

                ListViewItem.ListViewSubItem language = new ListViewItem.ListViewSubItem();
                language.Text = stream.LanguageName;
                language.Tag = stream.LanguageCode;

                ListViewItem.ListViewSubItem channels = new ListViewItem.ListViewSubItem();
                channels.Text = stream.ChannelCountDescription;
                channels.Tag = stream.ChannelCountDescription;

                ListViewItem.ListViewSubItem[] streamSubItems =
                    new[]
                    {
                        codec,
                        language,
                        channels
                    };

                ListViewItem streamItem = new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream;
                streamItem.Checked = true;
                streamItem.ImageIndex = i++;
                listViewAudioTracks.Items.Add(streamItem);

                icons.Images.Add(TSStream.GetCodecIcon(stream.StreamType));
            }

            listViewAudioTracks.SmallImageList = icons;
        }

        private void PopulateSubtitleTracks()
        {
            var i = 0;
            ImageList icons = new ImageList();

            foreach (TSStream stream in _subtitleTracks.Where(track => track.StreamType != TSStreamType.INTERACTIVE_GRAPHICS))
            {
                ListViewItem.ListViewSubItem codec = new ListViewItem.ListViewSubItem();
                codec.Text = stream.CodecName;
                if (stream.AngleIndex > 0)
                    codec.Text += string.Format(" ({0})", stream.AngleIndex);
                codec.Tag = stream.CodecName;

                ListViewItem.ListViewSubItem language = new ListViewItem.ListViewSubItem();
                language.Text = stream.LanguageName;
                language.Tag = stream.LanguageCode;

                ListViewItem.ListViewSubItem[] streamSubItems =
                    new[]
                    {
                        codec,
                        language
                    };

                ListViewItem streamItem = new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream;
                streamItem.Checked = true;
                streamItem.ImageIndex = i++;
                listViewSubtitleTracks.Items.Add(streamItem);

                icons.Images.Add(TSStream.GetCodecIcon(stream.StreamType));
            }

            listViewSubtitleTracks.SmallImageList = icons;
        }

        private void ResizeOutputTab(object sender = null, EventArgs e = null)
        {
            listViewVideoTracks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewVideoTracks.AutoResizeColumn(columnHeaderVideoCodec.Index, ColumnHeaderAutoResizeStyle.ColumnContent);

            listViewAudioTracks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewAudioTracks.AutoResizeColumn(columnHeaderAudioCodec.Index, ColumnHeaderAutoResizeStyle.ColumnContent);

            listViewSubtitleTracks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewSubtitleTracks.AutoResizeColumn(columnHeaderSubtitleCodec.Index, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        #endregion

        #region Web Service Queries

        private void QueryMainMovie()
        {
            _isSearchingMainMovieDb = true;

            ResetUI();

            _autoConfigured = false;
            _autoTmdbId = -1;

            SetTabStatus(tabPageDisc, "Querying main movie database...");

            _mainMovieBackgroundWorker = new BackgroundWorker();
            _mainMovieBackgroundWorker.WorkerReportsProgress = true;
            _mainMovieBackgroundWorker.WorkerSupportsCancellation = true;
            _mainMovieBackgroundWorker.DoWork += mainMovieBackgroundWorker_DoWork;
            _mainMovieBackgroundWorker.ProgressChanged += mainMovieBackgroundWorker_ProgressChanged;
            _mainMovieBackgroundWorker.RunWorkerCompleted += mainMovieBackgroundWorker_RunWorkerCompleted;
            _mainMovieBackgroundWorker.RunWorkerAsync();
        }

        private void SearchTmdb()
        {
            Language language = discLanguageComboBox.SelectedValue as Language;

            if (String.IsNullOrEmpty(movieNameTextBox.Text) || language == null)
            {
                return;
            }

            _isSearchingTmdb = true;

            ResetUI();
            ClearTmdb();

            SetTabStatus(tabPageDisc, "Searching The Movie Database (TMDb)...");

            string ISO_639_1 = language.ISO_639_1;
            string query = movieNameTextBox.Text;
            int? year = Regex.IsMatch(maskedTextBoxYear.Text, @"^\d{4}$") ? (int?)Int32.Parse(maskedTextBoxYear.Text) : null;
            
            TmdbSearchRequestParams reqParams = new TmdbSearchRequestParams(query, year, ISO_639_1);

            _tmdbBackgroundWorker = new BackgroundWorker();
            _tmdbBackgroundWorker.WorkerReportsProgress = true;
            _tmdbBackgroundWorker.WorkerSupportsCancellation = true;
            _tmdbBackgroundWorker.DoWork += tmdbBackgroundWorker_DoWork;
            _tmdbBackgroundWorker.ProgressChanged += tmdbBackgroundWorker_ProgressChanged;
            _tmdbBackgroundWorker.RunWorkerCompleted += tmdbBackgroundWorker_RunWorkerCompleted;
            _tmdbBackgroundWorker.RunWorkerAsync(reqParams);
        }

        private class TmdbSearchRequestParams
        {
            public readonly string Query;
            public readonly int? Year;
            public readonly string ISO_639_1;
            public TmdbSearchRequestParams(string query, int? year, string ISO_639_1)
            {
                Query = query;
                Year = year;
                this.ISO_639_1 = ISO_639_1;
            }
        }

        #endregion

        #region Main Movie Background Worker

        private void mainMovieBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IList<TSPlaylistFile> mainPlaylists = _playlists.Where(playlist => playlist.IsMainPlaylist).ToList();
                _mainMovieSearchResult = _mainMovieService.FindMainMovie(_bdrom.VolumeLabel, mainPlaylists);
                e.Result = null;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void mainMovieBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void mainMovieBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isSearchingMainMovieDb = false;

            ResetUI();

            string errorCaption = "Error querying main movie DB";

            var exception = e.Result as Exception;
            if (exception != null)
            {
                searchResultListView.Enabled = false;
                ShowErrorMessage(tabPageDisc, errorCaption, exception.Message);
            }

            if (_mainMovieSearchResult == null) return;

            if (_mainMovieSearchResult.error)
            {
                string errorMessages = _mainMovieSearchResult.errors.Aggregate("", (current, error) => current + (error.textStatus + " - " + error.errorMessage + "\n"));
                string errorMessage = "Main movie service returned the following error(s): \n\n" + errorMessages;
                ShowErrorMessage(tabPageDisc, errorCaption, errorMessage);
            }
            else if (_mainMovieSearchResult.discs.Count == 0)
            {
                ShowExclamationMessage(tabPageDisc, "No results found", "No matching discs were found in the database.\n\n" + "Please submit one!");
            }
            else
            {
                JsonDisc disc = _mainMovieSearchResult.discs[0];

                _autoConfigured = true;
                _autoTmdbId = disc.tmdb_id;

                movieNameTextBox.Text = disc.movie_title;
                maskedTextBoxYear.Text = disc.year != null ? disc.year.ToString() : null;

                _populator.AutoConfigure(disc.playlists);

                int count = _mainMovieSearchResult.discs.Count;
                string plural = count != 1 ? "s" : "";
                string caption = string.Format("{0:d} result{1} found", count, plural);
                string message = string.Format("Hooray!  Found {0:d} matching disc{1} in the database.", count, plural);
                ShowMessage(tabPageDisc, caption, message);
            }

            SearchTmdb();
        }

        #endregion

        #region TMDb Background Worker

        private void tmdbBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var reqParams = e.Argument as TmdbSearchRequestParams;

                Debug.Assert(reqParams != null, "reqParams != null");

                _tmdbMovieSearch = _tmdbApi.SearchMovie(reqParams.Query, 1, reqParams.ISO_639_1, false, reqParams.Year);

                if (_rootUrl == null)
                    _rootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";

                e.Result = null;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void tmdbBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private static string GetYearString(string tmdbDate)
        {
            return String.IsNullOrEmpty(tmdbDate) ? null : Regex.Replace(tmdbDate, @"^(\d{4})-(\d{1,2})-(\d{1,2})$", @"$1", RegexOptions.IgnoreCase);
        }

        private static int? GetYearInt(string tmdbDate)
        {
            string yearString = GetYearString(tmdbDate);
            return String.IsNullOrEmpty(yearString) ? (int?)null : Convert.ToInt32(yearString);
        }

        private void tmdbBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isSearchingTmdb = false;

            ResetUI();

            string errorCaption = "Error searching The Movie Database (TMDb)";

            var exception = e.Result as Exception;
            if (exception != null)
            {
                ShowErrorMessage(tabPageDisc, errorCaption, exception.Message);
            }

            if (_tmdbMovieSearch == null || _tmdbMovieSearch.results == null)
                return;

            foreach (MovieResult curResult in _tmdbMovieSearch.results)
            {
                ListViewItem.ListViewSubItem movieNameSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem movieYearSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem moviePopularitySubItem = new ListViewItem.ListViewSubItem();

                movieNameSubItem.Text = curResult.title;
                movieYearSubItem.Text = GetYearString(curResult.release_date);
                moviePopularitySubItem.Text = curResult.popularity.ToString("N3", CultureInfo.CurrentUICulture);

                ListViewItem.ListViewSubItem[] searchResultSubItems =
                    new[]
                        {
                            movieNameSubItem,
                            movieYearSubItem,
                            moviePopularitySubItem
                        };

                ListViewItem searchResultListItem =
                    new ListViewItem(searchResultSubItems, 0);

                searchResultListView.Items.Add(searchResultListItem);
            }

            if (_tmdbMovieSearch.results.Count > 0)
            {
                int count = _tmdbMovieSearch.results.Count;
                string plural = count != 1 ? "s" : "";
                SetTabStatus(tabPageDisc, string.Format("{0:d} result{1} found", count, plural));

                searchResultListView.Enabled = true;
                searchResultListView.Select();
                searchResultListView.Items[0].Selected = true;
            }
            else
            {
                ShowExclamationMessage(tabPageDisc, "No results found", "No matching movies found in The Movie Database (TMDb)");
                movieNameTextBox.Select();
            }

            ResizeDiscTab();
        }

        #endregion

        #region tsMuxeR Muxing

        private void Rip()
        {
            if (_isMuxing) return;

            if (File.Exists(OutputFilePath))
            {
                if (DialogResult.Yes != MessageBox.Show(this,
                    string.Format("Overwrite \"{0}\"?", OutputFilePath),
                    "File already exists",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    return;
                }
            }

            if (SelectedPlaylist == null) return;

            ShowProgressTabPage = true;
            tabControl.SelectedTab = tabPageProgress;

            try
            {
                Directory.CreateDirectory(textBoxOutputDirPreview.Text);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(tabPageProgress, "Unable to create directory", ex.Message);
                return;
            }

            _tsMuxerOutputPath = Path.Combine(textBoxOutputDirPreview.Text, textBoxOutputFileNamePreview.Text);

            /*
            if (!FileUtils.IsFileWritableRecursive(tsMuxerOutputPath))
            {
                ShowErrorMessage(tabPageProgress, "File is not writable", "File \"" + tsMuxerOutputPath + "\" is not writable!");
                return;
            }
            */

            ulong minFreeSpace = (ulong)(SelectedPlaylist.FileSize * 2.5);

            // TODO: Fix this
            /*
            if (FileUtils.GetFreeSpace(textBoxOutputDirPreview.Text) < minFreeSpace)
            {
                ShowErrorMessage(tabPageProgress, "Not enough free space", "At least " + FileUtils.FormatFileSize(textBoxOutputDirPreview.Text) + " (" + minFreeSpace + " bytes) of free space is required.");
                return;
            }
            */

            ISet<TSStream> selectedStreams = new HashSet<TSStream>();
            selectedStreams.UnionWith(SelectedVideoStreams);
            selectedStreams.UnionWith(SelectedAudioStreams);
            selectedStreams.UnionWith(SelectedSubtitleStreams);

            BDAutoMuxerSettings.OutputDir = textBoxOutputDir.Text;
            BDAutoMuxerSettings.OutputFileName = textBoxOutputFileName.Text;
            BDAutoMuxerSettings.ReplaceSpaces = checkBoxReplaceSpaces.Checked;
            BDAutoMuxerSettings.ReplaceSpacesWith = textBoxReplaceSpaces.Text;
            BDAutoMuxerSettings.SaveSettings();

            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, "tsMuxeR: 0.0%");
            textBoxTsMuxerCommandLine.Text = "";
            toolStripProgressBar.Visible = true;

            _tsMuxer = new TsMuxer(_bdrom, SelectedPlaylist, selectedStreams);
            _tsMuxer.WorkerReportsProgress = true;
            _tsMuxer.WorkerSupportsCancellation = true;
            _tsMuxer.ProgressChanged += tsMuxerBackgroundWorker_ProgressChanged;
            _tsMuxer.RunWorkerCompleted += tsMuxerBackgroundWorker_RunWorkerCompleted;

            _tsMuxer.RunWorkerAsync(_tsMuxerOutputPath);
        }

        private void CancelRip()
        {
            if (_tsMuxer == null || !_tsMuxer.IsBusy) return;

            _tsMuxer.Resume();
            _tsMuxer.CancelAsync();
        }

        private void tsMuxerBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _isMuxing = true;
            ResetUI();
            UpdateTsMuxerProgress(sender, e);
        }

        private void UpdateTsMuxerProgress(object sender = null, EventArgs e = null)
        {
            // To show fractional progress, progress bar range is 0 to 1000 (instead of 0 to 100)
            progressBarTsMuxer.Value = (int)(_tsMuxer.Progress * 10);

// ReSharper disable EmptyGeneralCatchClause
            try
            {
                // TODO: This throws a NPE if the window is closed while muxing is in progress
                toolStripProgressBar.Value = progressBarTsMuxer.Value;
            }
            catch
            {
            }
// ReSharper restore EmptyGeneralCatchClause

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.Normal, Handle);
            TaskbarProgress.SetProgressValue(progressBarTsMuxer.Value, 1000, Handle);

            string strProgress = _tsMuxer.Progress.ToString("##0.0") + "%";
            labelTsMuxerProgress.Text = strProgress;
            progressLabel.Text = strProgress;

            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, "tsMuxeR: " + strProgress);

            if (String.IsNullOrEmpty(textBoxTsMuxerCommandLine.Text))
                textBoxTsMuxerCommandLine.Text = _tsMuxer.CommandLine;

            labelTsMuxerTimeRemaining.Text = GetElapsedTimeString(_tsMuxer.TimeRemaining);
            labelTsMuxerTimeElapsed.Text = GetElapsedTimeString(_tsMuxer.TimeElapsed);

            // TODO: Refactor this logic into a separate method
            if (!String.IsNullOrEmpty(_tsMuxer.State))
            {
                labelTsMuxerProgress.Text += String.Format(" ({0})", _tsMuxer.State);
            }
        }

        private string GetElapsedTimeString(TimeSpan elapsedTime)
        {
            if (elapsedTime == TimeSpan.MaxValue)
                elapsedTime = TimeSpan.Zero;

            return string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                elapsedTime.Hours,
                elapsedTime.Minutes,
                elapsedTime.Seconds);
        }

        private void tsMuxerBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isMuxing = false;

            ResetUI();

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.NoProgress, Handle);

            if (e.Cancelled && _tsMuxer.IsCanceled)
            {
                SetTabStatus(tabPageProgress, "tsMuxeR canceled");
            }
            else if (e.Error != null)
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                ShowErrorMessage(tabPageProgress, "tsMuxeR Error", e.Error.Message);
            }
            else
            {
                ShowMessage(tabPageProgress, "tsMuxeR completed!", "Finished muxing M2TS with tsMuxeR!");
            }
        }

        #endregion

        #region Submit JSON to DB

        private bool CanSubmitToDb
        {
            get
            {
                if (!_initialized)
                    return false;
                if (!String.IsNullOrEmpty(BDAutoMuxerSettings.ApiKey) && searchResultListView.SelectedIndices.Count > 0)
                {
                    if (_autoConfigured)
                    {
                        if (_tmdbMovieResult.id != _autoTmdbId || _populator.HasChanged)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void SubmitJsonDiscIfNecessary()
        {
            if (!CanSubmitToDb) return;

            DialogResult answer = MessageBox.Show(this, "Submit a new disc to the database?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (answer == DialogResult.Yes)
            {
                SubmitJsonDisc();
            }
        }

        private void SubmitJsonDisc(object sender = null, EventArgs e = null)
        {
            if (searchResultListView.SelectedItems.Count == 0 || !_initialized)
                return;

            DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to submit to the database?", "Confirm database submit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No) return;

            JsonDisc jsonDisc = new JsonDisc();

            jsonDisc.disc_name = _bdrom.DiscName;
            jsonDisc.volume_label = _bdrom.VolumeLabel;
            jsonDisc.ISO_639_2 = _bdrom.DiscLanguage != null ? _bdrom.DiscLanguage.ISO_639_2 : null;

            jsonDisc.tmdb_id = _tmdbMovieResult != null ? _tmdbMovieResult.id : -1;
            jsonDisc.movie_title = _tmdbMovieTitle;
            jsonDisc.year = _tmdbMovieYear;

            jsonDisc.playlists = new List<JsonPlaylist>();

            foreach (JsonPlaylist jsonPlaylist in _populator.JsonPlaylists)
            {
                jsonDisc.playlists.Add(jsonPlaylist);
            }

            try
            {
                var postResult = _mainMovieService.PostDisc(jsonDisc);
                if (postResult.error)
                {
                    var errorMessage = "Unknown error occurred while POSTing to the DB";
                    if (postResult.errors.Count > 0)
                    {
                        var error = postResult.errors[0];
                        errorMessage = string.IsNullOrEmpty(error.errorMessage) ? error.textStatus : error.errorMessage;
                    }
                    ShowErrorMessage("DB POST Error", errorMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("DB POST error", "ERROR: \n\n" + ex.Message);
                return;
            }

            ShowMessage("DB POST successful", "Awesome!  Successfully added disc to database.");
        }

        #endregion

        // TODO: Organize Event Handlers into separate regions for each tab
        #region Event Handlers

        private void discLanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var language = discLanguageComboBox.SelectedValue as Language;
            if (_populator != null && language != null)
                _populator.MainLanguageCode = language.ISO_639_2;
        }

        private void maskedTextBoxYear_TextChanged(object sender, EventArgs e)
        {
            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void searchResultListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            _tmdbMovieResult = null;
            _tmdbMovieTitle = null;
            _tmdbMovieYear = null;
            _tmdbMovieUrl = null;

            pictureBoxMoviePoster.Image = null;
            pictureBoxMoviePoster.ImageLocation = null;
            pictureBoxMoviePoster.Cursor = Cursors.Default;

            if (_tmdbMovieSearch != null && searchResultListView.SelectedIndices.Count > 0)
            {
                int index = searchResultListView.SelectedIndices[0];
                _tmdbMovieResult = _tmdbMovieSearch.results[index];

                if (_tmdbMovieResult != null)
                {
                    _tmdbMovieTitle = _tmdbMovieResult.title;
                    _tmdbMovieYear = GetYearInt(_tmdbMovieResult.release_date);

                    if (string.IsNullOrEmpty(_tmdbMovieResult.poster_path))
                        pictureBoxMoviePoster.Image = Properties.Resources.no_poster_w185;
                    else
                        pictureBoxMoviePoster.ImageLocation = _rootUrl + _tmdbMovieResult.poster_path;

                    _tmdbMovieUrl = string.Format("http://www.themoviedb.org/movie/{0}", _tmdbMovieResult.id);
                    
                    pictureBoxMoviePoster.Cursor = Cursors.Hand;
                }
            }

            buttonSubmitToDB.Enabled = CanSubmitToDb;

            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void searchButton_Click(object sender = null, EventArgs e = null)
        {
            if (searchButton.Enabled)
                SearchTmdb();
        }

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylistDataGrid();
        }

        private void listViewStreamFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewStreamFiles.SelectedItems.Count == 0) return;

            string filename = listViewStreamFiles.SelectedItems[0].Text;
            string filepath = Path.Combine(_bdrom.DirectorySTREAM.FullName, filename);

            PlayFile(filepath);
        }

        private void OnPlaylistItemChange(object sender, EventArgs e)
        {
            if (_ignoreDataGridItemChange) return;

            PopulateOutputTab();
            buttonSubmitToDB.Enabled = CanSubmitToDb;
        }

        private void OnAudienceLanguageChange(object sender, EventArgs e)
        {
            SetAudienceLanguage(VideoLanguages, comboBoxVideoLanguage);
            SetAudienceLanguage(AudioLanguages, listBoxAudioLanguages);
            SetAudienceLanguage(SubtitleLanguages, listBoxSubtitleLanguages);
        }

        private void SetAudienceLanguage(IEnumerable<Language> array, ListControl control)
        {
            Language audienceLanguage = comboBoxAudienceLanguage.SelectedValue as Language;
            IList<Language> list = new List<Language>(array);
            
            if (!list.Contains(audienceLanguage) || control.DataSource == null) return;

            try
            {
                control.SelectedIndex = -1;
                control.SelectedIndex = list.IndexOf(audienceLanguage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "BDAutoMuxer Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void playlistDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (!_initialized)
                return;

            TSPlaylistFile playlistFile = _populator.SelectedPlaylist;

            if (playlistFile == null) return;

            StreamTrackListViewPopulator.Populate(playlistFile, listViewStreamFiles, listViewStreams);

            ResetUI();
        }

        private void playlistDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (playlistDataGridView.IsCurrentCellDirty)
            {
                playlistDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void playlistDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_initialized)
                return;
            if (e.Button != MouseButtons.Right) return;

            int currentMouseOverRow = playlistDataGridView.HitTest(e.X, e.Y).RowIndex;

            if (currentMouseOverRow < 0) return;

            playlistDataGridView.ClearSelection();
            playlistDataGridView.Rows[currentMouseOverRow].Selected = true;

            TSPlaylistFile playlist = _populator.PlaylistAt(currentMouseOverRow);
            ShowPlayableFileContextMenu(playlistDataGridView, playlist.FullName, e.X, e.Y);
        }

        private void listViewStreamFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            ListViewItem item = listViewStreamFiles.HitTest(e.X, e.Y).Item;

            if (item.Index < 0) return;

            if (!(item.Tag is TSStreamClip)) return;

            TSStreamClip clip = item.Tag as TSStreamClip;

            string filename = clip.DisplayName;
            string filepath = Path.Combine(_bdrom.DirectorySTREAM.FullName, filename);

            ShowPlayableFileContextMenu(listViewStreamFiles, filepath, e.X, e.Y);
        }

        private void ShowPlayableFileContextMenu(Control control, string filePath, int x, int y)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem menuItemOpen = new ToolStripMenuItem(string.Format("&Open \"{0}\"", Path.GetFileName(filePath)));
            //menuItemOpen.DefaultItem = true;
            menuItemOpen.Font = new Font(menuItemOpen.Font, FontStyle.Bold);
            menuItemOpen.Image = FileUtils.ExtractIconAsBitmap(filePath);
            menuItemOpen.Click += (s1, e1) => PlayFile(filePath);
            menuItemOpen.Enabled = FileUtils.HasProgramAssociation(filePath);

            ToolStripMenuItem menuItemCopyPath = new ToolStripMenuItem("&Copy path to clipboard");
            menuItemCopyPath.Image = Resources.copy;
            menuItemCopyPath.Click += (s1, e1) => Clipboard.SetText(filePath);

            ToolStripMenuItem menuItemShow = new ToolStripMenuItem("Show in &folder");
            menuItemShow.Image = Resources.folder_open;
            menuItemShow.Click += (s1, e1) => ShowInFolder(filePath);

            contextMenu.Items.Add(menuItemOpen);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add(menuItemCopyPath);
            contextMenu.Items.Add(menuItemShow);

//            contextMenu.Opening += (s, e) => menuItemOpen.Select();

            contextMenu.Show(control, new Point(x, y));
        }

        private static void PlayFile(string filePath)
        {
            FileUtils.OpenFile(filePath);
        }

        private static void ShowInFolder(string filePath)
        {
            FileUtils.ShowInFolder(filePath);
        }

        private bool EnableScanControls
        {
            set
            {
                textBoxSource.Enabled = value;

                buttonBrowse.Enabled = value;
                buttonRescan.Enabled = value;

                openFolderToolStripMenuItem.Enabled = value;
                rescanToolStripMenuItem.Enabled = value;
            }
        }

        private bool SelectedPlaylistHasHiddenTracks
        {
            get
            {
                return _populator != null && _populator.SelectedPlaylist != null && _populator.SelectedPlaylist.SortedStreams.Any(stream => stream.IsHidden);
            }
        }

        private bool DiscTabEnabled
        {
            set { panelMovieDetails.Enabled = value; }
        }

        private bool OutputTabEnabled
        {
            set
            {
                groupBoxMasterOverride.Enabled = value;
                groupBoxFilter.Enabled = value;
                groupBoxTracks.Enabled = value;
                checkBoxReplaceSpaces.Enabled = value;
                textBoxReplaceSpaces.Enabled = value && checkBoxReplaceSpaces.Checked;
                textBoxOutputDir.Enabled = value;
                buttonOutputDir.Enabled = value;
                textBoxOutputFileName.Enabled = value;
            }
        }

        private bool ShowProgressTabPage
        {
            get { return tabControl.TabPages.Contains(tabPageProgress); }
            set
            {
                if (value)
                {
                    if (!ShowProgressTabPage)
                        tabControl.TabPages.Add(tabPageProgress);
                }
                else
                {
                    tabControl.TabPages.Remove(tabPageProgress);
                }
            }
        }

        private void ClearTmdb()
        {
            searchResultListView.Items.Clear();
            pictureBoxMoviePoster.Image = null;
            pictureBoxMoviePoster.ImageLocation = null;
        }

        private void ResetUI()
        {
            EnableScanControls = !_isScanningBDROM && !_isSearchingMainMovieDb && !_isSearchingTmdb && !_isMuxing;

            tabControl.Enabled = _initialized && !_isSearchingMainMovieDb;

            DiscTabEnabled = _initialized && !_isSearchingMainMovieDb && !_isSearchingTmdb && !_isMuxing;
            OutputTabEnabled = !_isMuxing;
            ShowProgressTabPage = ShowProgressTabPage || _isMuxing;

            hiddenTrackLabel.Visible = SelectedPlaylistHasHiddenTracks;
            buttonPlaylistOpen.Enabled = SelectedPlaylist != null;

            toolStripProgressBar.Visible = _isMuxing;

            if (!_initialized)
            {
                ClearTmdb();
                statusLabel.Text = "";
            }

            if (!_isMuxing)
            {
                progressLabel.Text = "";
            }

            buttonSubmitToDB.Enabled = CanSubmitToDb;
            submitToDbToolStripMenuItem.Enabled = CanSubmitToDb;

            continueButton.Text = _isMuxing ? (_tsMuxer.IsPaused ? "Resume" : "Pause") : "Mux it!";
            continueButton.Enabled = _initialized && (_isMuxing || tabControl.SelectedTab == tabPageOutput) && SelectedPlaylist != null;
            cancelButton.Text = _isMuxing ? "Stop" : "Close";

            ResizeDiscTab();
            ResizeOutputTab();
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (_isMuxing && _tsMuxer != null)
            {
                // TODO: Refactor label text logic into a separate method
                if (_tsMuxer.IsPaused)
                {
                    _tsMuxer.Resume();
                    labelTsMuxerProgress.Text = labelTsMuxerProgress.Text.Replace(" (paused}", "");
                }
                else
                {
                    _tsMuxer.Pause();
                    labelTsMuxerProgress.Text += " (paused}";
                }
            }
            else if (tabControl.SelectedTab == tabPageOutput)
            {
                Rip();
            }

            ResetUI();
        }

        private void checkBoxReplaceSpaces_CheckedChanged(object sender, EventArgs e)
        {
            textBoxReplaceSpaces.Enabled = checkBoxReplaceSpaces.Checked;
            if (checkBoxReplaceSpaces.Checked)
            {
                textBoxReplaceSpaces.Focus();
                textBoxReplaceSpaces.SelectAll();
            }
            textBoxOutputDir_TextChanged(sender, e);
            textBoxOutputFileName_TextChanged(sender, e);
        }

        private string ReplacePlaceholders(string text)
        {
            if (!_initialized)
                return text;

            string preview = text;

            string volume = _bdrom.VolumeLabel;
            string title = String.IsNullOrEmpty(_tmdbMovieTitle) ? movieNameTextBox.Text : _tmdbMovieTitle;
            string year = _tmdbMovieYear == null ? GetYearString(maskedTextBoxYear.Text) : _tmdbMovieYear + "";
            string res = "";
            string vcodec = "";
            string acodec = "";
            string channels = "";

            if (string.IsNullOrEmpty(year))
                year = "";

            if (listViewVideoTracks.CheckedItems.Count > 0)
            {
                TSVideoStream videoStream = listViewVideoTracks.CheckedItems[0].Tag as TSVideoStream;
                if (videoStream != null)
                {
                    res = videoStream.HeightDescription;
                    vcodec = videoStream.CodecShortName;
                }
            }

            if (listViewAudioTracks.CheckedItems.Count > 0)
            {
                TSAudioStream audioStream = listViewAudioTracks.CheckedItems[0].Tag as TSAudioStream;
                if (audioStream != null)
                {
                    acodec = audioStream.CodecShortName;
                    channels = audioStream.ChannelCountDescription;
                }
            }

            preview = Regex.Replace(preview, @"%volume%", volume, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%title%", title, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%year%", year, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%res%", res, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%vcodec%", vcodec, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%acodec%", acodec, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%channels%", channels, RegexOptions.IgnoreCase);
            preview = Environment.ExpandEnvironmentVariables(preview);

            if (checkBoxReplaceSpaces.Checked)
            {
                preview = preview.Replace(" ", textBoxReplaceSpaces.Text);
            }

            return preview;
        }

        private void textBoxOutputDir_TextChanged(object sender, EventArgs e)
        {
            textBoxOutputDirPreview.Text = ReplacePlaceholders(textBoxOutputDir.Text);
        }

        private void textBoxOutputFileName_TextChanged(object sender, EventArgs e)
        {
            textBoxOutputFileNamePreview.Text = FileUtils.SanitizeFileName(ReplacePlaceholders(textBoxOutputFileName.Text)) + labelOutputFileExtension.Text;
        }

        private void textBoxReplaceSpaces_TextChanged(object sender, EventArgs e)
        {
            textBoxOutputDir_TextChanged(sender, e);
            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void textBoxReplaceSpaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            // TODO: Also handle pasted text
            if (!FileUtils.IsValidFilename(e.KeyChar + "") && e.KeyChar >= 32 && e.KeyChar != 127)
                e.Handled = true;
        }

        private void FilterControlChanged(object sender, EventArgs e)
        {
            // TODO: This gets called WAY too many times when we set the combobox
            //       values programmatically, since each one fires this event.
            FilterPlaylists();
        }

        private void comboBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTracks();
        }

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
            ResetUI();
            RestoreTabStatus();
        }

        private void buttonSubmitToDB_Click(object sender, EventArgs e)
        {
            SubmitJsonDisc();
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            _ignoreDataGridItemChange = true;
            _populator.SelectAll = true;
            _ignoreDataGridItemChange = false;
            OnPlaylistItemChange(sender, e);
        }

        private void buttonUnselectAll_Click(object sender, EventArgs e)
        {
            _ignoreDataGridItemChange = true;
            _populator.SelectAll = false;
            _ignoreDataGridItemChange = false;
            OnPlaylistItemChange(sender, e);
        }

        private void buttonOutputDir_Click(
            object sender,
            EventArgs e)
        {
            string path = null;
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "Select an Output Folder:";
                dialog.ShowNewFolderButton = false;
                string dir = textBoxOutputDirPreview.Text;
                if (!string.IsNullOrEmpty(dir))
                {
                    dialog.SelectedPath = dir;
                }
                
                if (dialog.ShowDialog() != DialogResult.OK) return;

                path = dialog.SelectedPath;
                textBoxOutputDir.Text = path;

                // TODO: Validate that the selected directory has enough free space for 2x-3x the playlist size
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

        private void textBoxOutputDir_DragEnter(object sender, DragEventArgs e)
        {
            var accept = CanDragAndDrop && e.Data.GetDataPresent(DataFormats.FileDrop);
            e.Effect = accept ? DragDropEffects.All : DragDropEffects.None;
            textBoxOutputDir.BorderStyle = accept ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
        }

        private void textBoxOutputDir_DragDrop(object sender, DragEventArgs e)
        {
            string path = DragUtils.GetFirstPath(e);

            if (CanDragAndDrop && path != null)
                textBoxOutputDir.Text = FileUtils.IsDirectory(path) ? path : Path.GetDirectoryName(path);

            textBoxOutputDir.BorderStyle = BorderStyle.Fixed3D;

            // TODO: Validate that the selected directory has enough free space for 2x-3x the playlist size
        }

        private void textBoxOutputDir_DragLeave(object sender, EventArgs e)
        {
            textBoxOutputDir.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBoxOutputFileName_DragEnter(object sender, DragEventArgs e)
        {
            var accept = CanDragAndDrop && DragUtils.HasFile(e) && !String.IsNullOrEmpty(DragUtils.GetFirstFileNameWithoutExtension(e));
            e.Effect = accept ? DragDropEffects.All : DragDropEffects.None;
            textBoxOutputFileName.BorderStyle = accept ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
        }

        private void textBoxOutputFileName_DragDrop(object sender, DragEventArgs e)
        {
            string filenameWithoutExtension = DragUtils.GetFirstFileNameWithoutExtension(e);

            if (CanDragAndDrop && !String.IsNullOrEmpty(filenameWithoutExtension))
                textBoxOutputFileName.Text = filenameWithoutExtension;

            textBoxOutputFileName.BorderStyle = BorderStyle.Fixed3D;
        }

        private void textBoxOutputFileName_DragLeave(object sender, EventArgs e)
        {
            textBoxOutputFileName.BorderStyle = BorderStyle.Fixed3D;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (_isMuxing)
            {
                if (ShouldAbortMuxing())
                    CancelRip();
            }
            else
                Close();
        }

        private void pictureBoxMoviePoster_MouseClick(object sender, MouseEventArgs e)
        {
            if (_tmdbMovieUrl != null)
                Process.Start(_tmdbMovieUrl);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSettings().ShowDialog();
            ResetUI();
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
                    Scan(path);
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

        private void buttonRescan_Click(object sender = null, EventArgs e = null)
        {
            if (!buttonRescan.Enabled) return;

            string path = textBoxSource.Text;
            try
            {
                Scan(path);
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

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateNotifier.CheckForUpdate(this, true);
        }

        private void remuxerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormRemux().Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxSource_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            buttonRescan_Click();
            e.Handled = true;
        }

        private void movieNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            searchButton_Click();
            e.Handled = true;
        }

        private void buttonPlaylistOpen_Click(object sender, EventArgs e)
        {
            if (SelectedPlaylist != null)
                PlayFile(SelectedPlaylist.FullName);
        }

        private static bool IsBDROMDir(DirectoryInfo dir)
        {
            return dir != null && dir.Name.ToLowerInvariant() == "bdmv";
        }

        private static string GetBDROMDirectory(DragEventArgs e)
        {
            return DragUtils.GetPaths(e).Select(GetBDROMDirectory).FirstOrDefault(path => path != null);
        }

        private static string GetBDROMDirectory(string path)
        {
            var dir = FileUtils.IsDirectory(path) ? new DirectoryInfo(path) : new FileInfo(path).Directory;

            if (dir == null)
                return null;

            if (IsBDROMDir(dir))
                return path;

            if (dir.GetDirectories().Any(IsBDROMDir))
                return path;

            while (dir != null)
            {
                if (IsBDROMDir(dir))
                    return dir.FullName;
                dir = dir.Parent;
            }

            return null;
        }

        private void FormDetails_DragEnter(object sender, DragEventArgs e)
        {
            var accept = CanDragAndDrop && !String.IsNullOrEmpty(GetBDROMDirectory(e));
            e.Effect = accept ? DragDropEffects.All : DragDropEffects.None;
            textBoxSource.BorderStyle = accept ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
        }

        private void FormDetails_DragDrop(object sender, DragEventArgs e)
        {
            var firstDirectoryPath = GetBDROMDirectory(e);

            textBoxSource.BorderStyle = BorderStyle.Fixed3D;

            if (!CanDragAndDrop || String.IsNullOrEmpty(firstDirectoryPath)) return;

            textBoxSource.Text = firstDirectoryPath;
            buttonRescan_Click();
        }

        private void FormDetails_DragLeave(object sender, EventArgs e)
        {
            textBoxSource.BorderStyle = BorderStyle.Fixed3D;
        }

        private void FormDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isMuxing)
            {
                if (ShouldAbortMuxing())
                {
                    CancelRip();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    BDAutoMuxerSettings.DetailsWindowLocation = RestoreBounds.Location;
                    BDAutoMuxerSettings.DetailsWindowSize = RestoreBounds.Size;
                    BDAutoMuxerSettings.DetailsWindowMaximized = true;
                }
                else
                {
                    BDAutoMuxerSettings.DetailsWindowLocation = Location;
                    BDAutoMuxerSettings.DetailsWindowSize = Size;
                    BDAutoMuxerSettings.DetailsWindowMaximized = false;
                }

                BDAutoMuxerSettings.SaveSettings();

                _playlistFinder.CancelScan();
            }
        }

        #endregion

        #region User Messages

        private void InitHints(Control parentControl)
        {
            foreach (Control control in parentControl.Descendants<Control>().Where(control => control.Tag is string))
            {
                _controlHints[control] = control.Tag as string;
                control.MouseEnter += (sender, e) => SetTabStatus(_controlHints[sender as Control], true);
                control.MouseLeave += (sender, e) => RestoreTabStatus();
            }
        }

        private void SetTabStatus(TabPage tabPage, string message, bool temporary = false)
        {
            if (!temporary)
                _tabStatusMessages[tabPage] = message;
            if (tabPage == tabControl.SelectedTab)
                statusLabel.Text = message;
        }

        private void SetTabStatus(string message, bool temporary = false)
        {
            SetTabStatus(tabControl.SelectedTab, message, temporary);
        }

        private void RestoreTabStatus()
        {
            statusLabel.Text = _tabStatusMessages.ContainsKey(tabControl.SelectedTab) ? _tabStatusMessages[tabControl.SelectedTab] : "";
        }

        private void ShowMessage(TabPage tabPage, string caption, string text, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            SetTabStatus(tabPage, caption);
            MessageBox.Show(this, text, caption, MessageBoxButtons.OK, icon);
        }

        private void ShowMessage(string caption, string text, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            ShowMessage(tabControl.SelectedTab, caption, text, icon);
        }

        private void ShowExclamationMessage(TabPage tabPage, string caption, string text)
        {
            ShowMessage(caption, text, MessageBoxIcon.Exclamation);
        }

        private void ShowExclamationMessage(string caption, string text)
        {
            ShowExclamationMessage(tabControl.SelectedTab, caption, text);
        }

        private void ShowErrorMessage(TabPage tabPage, string caption, string text)
        {
            ShowMessage(caption, text, MessageBoxIcon.Error);
        }

        private void ShowErrorMessage(string caption, string text)
        {
            ShowErrorMessage(tabControl.SelectedTab, caption, text);
        }

        private void CheckForUpdates()
        {
            if (BDAutoMuxerSettings.CheckForUpdates)
            {
                UpdateNotifier.CheckForUpdate(this);
            }
        }

        private bool ShouldAbortMuxing()
        {
            return MessageBox.Show(this, "Abort muxing?", "Abort muxing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        #endregion

        private void contextMenuStripTmdb_Opened(object sender, EventArgs e)
        {
            if (toolStripMenuItemTmdb.Image == null)
                toolStripMenuItemTmdb.Image = Win32.Win32.DefaultBrowserIconAsBitmap;
            toolStripMenuItemTmdb.Enabled = _tmdbMovieUrl != null;
        }

        private void toolStripMenuItemTmdb_Click(object sender, EventArgs e)
        {
            if (_tmdbMovieUrl != null)
                Process.Start(_tmdbMovieUrl);
        }

    }
}
