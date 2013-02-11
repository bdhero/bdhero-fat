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
using BDAutoMuxer.Services;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.Views;
using BDAutoMuxerCore;
using BDAutoMuxerCore.BDInfo;
using BDAutoMuxerCore.Services;
using BDAutoMuxerCore.Tools;
using DotNetUtils;
using MediaInfoWrapper;
using Microsoft.WindowsAPICodePack.Taskbar;
using ProcessUtils;
using WatTmdb.V3;

// ReSharper disable SuggestUseVarKeywordEvident
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable LocalizableElement
namespace BDAutoMuxer
{
    public partial class FormMain : Form
    {
        #region Fields

        private const string TmdbApiKey = "b59b366b0f0a457d58995537d847409a";

        private bool _initialized;
        private bool _isScanningBDROM;
        private bool _isSearchingMainMovieDb;
        private bool _isSearchingTmdb;
        private bool _isMuxing;
        private bool _isCancelling;

        private Tmdb _tmdbApi;

        private BDROM _bdrom;
        private IList<TSPlaylistFile> _playlists;
        private IList<Language> _languages;
        private IList<string> _languageCodes;

        private TmdbMovieSearch _tmdbMovieSearch;
        private MovieResult _tmdbMovieResult;
        private string _rootUrl;

        private JsonSearchResult _mainMovieSearchResult;

        private BackgroundWorker _mainMovieBackgroundWorker;
        private BackgroundWorker _tmdbBackgroundWorker;

        private PlaylistDataGridPopulator _populator;

        private bool _autoConfigured;
        private int _autoTmdbId = -1;

        private string _tmdbMovieTitle;
        private int? _tmdbMovieYear;
        private string _tmdbMovieUrl;
        private HttpImageCache _imageCache = HttpImageCache.Instance;
        private List<string> posterList;
        private List<string> languageList;
        private string _selectedLanguage;
        private int posterIndex;

        private bool _ignoreFilterControlChange;
        private bool _ignoreDataGridItemChange;

        private TsMuxer _demuxer;
        private TsMuxer _muxer;
        private string _tsMuxerOutputPath;

        private bool _shouldMux;

        private Point _demuxingProgressLocation;
        private Point _muxingProgressLocation;

        private ProgressUIState _eac3ToProgressUIState;
        private ProgressUIState _tsMuxerProgressUIState;

        private ISet<TSPlaylistFile> _filteredPlaylists = new HashSet<TSPlaylistFile>();

        private readonly IList<Language> _audioLanguages = new List<Language>();
        private readonly IList<Language> _subtitleLanguages = new List<Language>();

        private readonly List<TSVideoStream> _videoTracks = new List<TSVideoStream>();
        private readonly List<TSAudioStream> _audioTracks = new List<TSAudioStream>();
        private readonly List<TSStream> _subtitleTracks = new List<TSStream>();

        private readonly Dictionary<TabPage, string> _tabStatusMessages = new Dictionary<TabPage, string>();
        private readonly Dictionary<Control, String> _controlHints = new Dictionary<Control, String>();

        private readonly PlaylistFinder _playlistFinder = new PlaylistFinder();

        private readonly UpdateNotifierCompleteDelegate _updateNotifierComplete;

        #endregion

        #region Properties

        private bool IsDemuxLPCMChecked
        {
            get { return checkBoxDemuxLPCM.Checked && checkBoxDemuxLPCM.Enabled; }
        }

        private bool IsDemuxSubtitlesChecked
        {
            get { return checkBoxDemuxSubtitles.Checked && checkBoxDemuxSubtitles.Enabled; }
        }

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
                    audioLanguagesSet.Add(Language.FromCode(audioStream.LanguageCode));
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
                        subtitleLanguagesSet.Add(Language.FromCode(graphicsStream.LanguageCode));
                    }
                    foreach (TSTextStream textStream in playlist.TextStreams)
                    {
                        subtitleLanguagesSet.Add(Language.FromCode(textStream.LanguageCode));
                    }
                }
                return GetSortedLanguageArray(subtitleLanguagesSet);
            }
        }

        private ShowHiddenTracksOption[] ShowHiddenTracksOptions
        {
            get
            {
                var options = new List<ShowHiddenTracksOption> { ShowHiddenTracksOption.No };
                if (_populator.SelectedPlaylistsHaveAnyHiddenTracks)
                    options.Add(ShowHiddenTracksOption.Yes);
                return options.ToArray();
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

        private readonly bool _scanOnLoad;

        public FormMain(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
            {
                string path = args[0];
                textBoxSource.Text = path;
                _scanOnLoad = true;
            }
            else
            {
                textBoxSource.Text = BDAutoMuxerSettings.LastPath;
            }

            _updateNotifierComplete = () => checkForUpdatesToolStripMenuItem.Enabled = true;

            Load += FormDetails_Load;
        }

        ~FormMain()
        {
            CancelRip();
        }

        private void ShowCodecReference()
        {
            FormCodecReference.ShowReference();
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

            _demuxingProgressLocation = groupBoxDemuxingProgress.Location;
            _muxingProgressLocation = groupBoxMuxingProgress.Location;

            ShowProgressTabPage = false;

            textBoxOutputDir.Text = BDAutoMuxerSettings.OutputDir;
            textBoxOutputFileName.Text = BDAutoMuxerSettings.OutputFileName;

            textBoxReplaceSpaces.Text = BDAutoMuxerSettings.ReplaceSpacesWith;
            checkBoxReplaceSpaces.Checked = BDAutoMuxerSettings.ReplaceSpaces;

            checkBoxDemuxLPCM.Checked = BDAutoMuxerSettings.DemuxLPCM;
            checkBoxDemuxSubtitles.Checked = BDAutoMuxerSettings.DemuxSubtitles;

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

            _eac3ToProgressUIState = new ProgressUIState(labelDemuxingProgress, labelDemuxingTimeRemaining,
                                                         labelDemuxingTimeElapsed, progressBarDemuxing,
                                                         textBoxDemuxingCommandLine);
            _tsMuxerProgressUIState = new ProgressUIState(labelTsMuxerProgress, labelTsMuxerTimeRemaining,
                                                          labelTsMuxerTimeElapsed, progressBarTsMuxer,
                                                          textBoxTsMuxerCommandLine);

            StreamTrackListViewPopulator.AddCodecReferenceContextMenu(listViewVideoTracks, () => _videoTracks.OfType<TSStream>().ToList());
            StreamTrackListViewPopulator.AddCodecReferenceContextMenu(listViewAudioTracks, () => _audioTracks.OfType<TSStream>().ToList());
            StreamTrackListViewPopulator.AddCodecReferenceContextMenu(listViewSubtitleTracks, () => _subtitleTracks);

            CheckForUpdateOnStartup();

            if (_scanOnLoad)
                Scan(textBoxSource.Text);

            new ExternalDragProvider(listViewStreamFiles, this)
            {
                PathGetter = control => listViewStreamFiles.SelectedItems.Count > 0
                                            ? Path.Combine(_bdrom.DirectorySTREAM.FullName, listViewStreamFiles.SelectedItems[0].Text)
                                            : null
            };

            new ExternalDragProvider(playlistDataGridView, this)
            {
                PathGetter = control => _populator.SelectedPlaylist != null ? _populator.SelectedPlaylist.FullName : null,
                Threshold = 5
            };

            comboBoxLangauge.MouseWheel += new MouseEventHandler(comboBoxLangauge_MouseWheel);
        }

        private void ResetState()
        {
            _initialized = false;
            _isScanningBDROM = false;
            _isSearchingMainMovieDb = false;
            _isSearchingTmdb = false;
            _isMuxing = false;
            _isCancelling = false;

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
            _populator.OnItemChange += OnPlaylistItemChange;
            _populator.OnSelectionChange += playlistDataGridView_SelectionChanged;
            _populator.MainLanguageCode = ISO_639_2;

            _initialized = true;

            ResetUI();

            var movieNameSearchable = _bdrom.DiscNameSearchable;
            if (string.IsNullOrWhiteSpace(movieNameSearchable))
            {
                movieNameSearchable = _bdrom.VolumeLabel ?? "";
                movieNameSearchable = Regex.Replace(movieNameSearchable, @"^\d{6,}_", "");
                movieNameSearchable = Regex.Replace(movieNameSearchable, @"_+", " ");
                //                movieNameSearchable = movieNameSearchable.ToTitle();
            }
            if (Regex.Replace(movieNameSearchable, @"\W", "").ToLowerInvariant() == "bluray")
            {
                movieNameSearchable = "";
            }
            movieNameSearchable = Regex.Replace(movieNameSearchable, @"^(.*), (A|An|The)$", "$2 $1", RegexOptions.IgnoreCase);
            movieNameSearchable = movieNameSearchable.Replace("-", " ");
            movieNameSearchable = Regex.Replace(movieNameSearchable, @" \(?Disc \w+(?: of \w+)?\)?", "", RegexOptions.IgnoreCase);

            movieNameTextBox.Text = movieNameSearchable;
            textBoxYear.Text = "";

            discLanguageComboBox.DataSource = new List<Language>(_languages).ToArray();

            ResetPlaylistDataGrid();
            PopulateOutputTab();

            if (BDAutoMuxerSettings.UseMainMovieDb)
                QueryMainMovie();
            else
                SearchTmdb();

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
        }

        private void ScanError(object sender, ErrorEventArgs e)
        {
            SetTabStatus(e.GetException().Message);
        }

        private void ScanCompleted(object sender, EventArgs e)
        {
            _isScanningBDROM = false;
            BDAutoMuxerSettings.LastPath = textBoxSource.Text;
            BDAutoMuxerSettings.SaveSettings();
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

        private void ResetPlaylistDataGrid(object sender = null, EventArgs e = null)
        {
            if (_populator != null)
                _populator.SetVisible(
                    checkBoxShowLowQualityPlaylists.Checked,
                    checkboxShowBogusPlaylists.Checked,
                    checkBoxShowShortPlaylists.Checked);
        }

        #endregion

        #region "Output" Tab

        private void PopulateOutputTab()
        {
            _ignoreFilterControlChange = true;

            Language[] audienceLanguages = new List<Language>(_languages).ToArray();
            comboBoxAudienceLanguage.DataSource = null;
            comboBoxAudienceLanguage.DataSource = audienceLanguages;
            comboBoxAudienceLanguage.Enabled = audienceLanguages.Length > 1;

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

            ShowHiddenTracksOption[] hiddenTracksOptions = ShowHiddenTracksOptions;
            comboBoxShowHiddenTracks.DataSource = hiddenTracksOptions;
            comboBoxShowHiddenTracks.Enabled = hiddenTracksOptions.Length > 1;
            if (_populator.SelectedPlaylistsHaveAllHiddenTracks && hiddenTracksOptions.Length > 1)
                comboBoxShowHiddenTracks.SelectedIndex = 1;

            Language userPrefAudienceLanguage = BDAutoMuxerSettings.AudienceLanguage;
            if (audienceLanguages.Contains(userPrefAudienceLanguage))
                comboBoxAudienceLanguage.SelectedItem = userPrefAudienceLanguage;

            _ignoreFilterControlChange = false;

            FilterPlaylists();
        }

        private void FilterPlaylists()
        {
            if (_ignoreFilterControlChange || !_initialized)
                return;

            _filteredPlaylists = new HashSet<TSPlaylistFile>(_playlists);

            Language videoLanguage = comboBoxVideoLanguage.SelectedValue as Language;
            Cut cut = comboBoxCut.SelectedValue is Cut ? (Cut)comboBoxCut.SelectedValue : Cut.Theatrical;
            CommentaryOption commentaryOption = comboBoxCommentary.SelectedValue is CommentaryOption ? (CommentaryOption)comboBoxCommentary.SelectedValue : CommentaryOption.Any;

            _audioLanguages.Clear();
            _subtitleLanguages.Clear();

            _audioLanguages.AddRange(listBoxAudioLanguages.SelectedItems.OfType<Language>());
            _subtitleLanguages.AddRange(listBoxSubtitleLanguages.SelectedItems.OfType<Language>());

            if (videoLanguage == null) return;

            var playlistsWithMainMovie = _populator.GetPlaylistsWithMainMovie(true);
            var playlistsWithVideoLanguage = _populator.GetPlaylistsWithVideoLanguage(videoLanguage);
            var playlistsWithCut = _populator.GetPlaylistsWithCut(cut);
            var playlistsWithCommentaryOption = _populator.GetPlaylistsWithCommentaryOption(commentaryOption);
            var playlistsWithAudioLanguages = _populator.GetPlaylistsWithAudioLanguages(_audioLanguages);
            var playlistsWithSubtitleLanguages = _populator.GetPlaylistsWithSubtitleLanguages(_subtitleLanguages);

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
                var icon = FileUtils.ExtractIcon(SelectedPlaylist.FullName);
                if (icon != null)
                {
                    var iconSized = new Icon(icon, 16, 16);
                    buttonPlaylistOpen.Image = new Bitmap(iconSized.ToBitmap(), 16, 16);
                }
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

            foreach (TSStream stream in playlist.SortedStreams.Where(IncludeStream))
            {
                Language lang = !String.IsNullOrEmpty(stream.LanguageCode) ? Language.FromCode(stream.LanguageCode) : null;

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

        private bool IncludeStream(TSStream stream)
        {
            return !stream.IsHidden ||
                   (comboBoxShowHiddenTracks.SelectedIndex >= 0 && (ShowHiddenTracksOption)comboBoxShowHiddenTracks.SelectedItem == ShowHiddenTracksOption.Yes);
        }

        private void PopulateVideoTracks()
        {
            var i = 0;
            var icons = new ImageList { ColorDepth = ColorDepth.Depth32Bit };

            var first1080 = _videoTracks.FirstOrDefault(track => track.VideoFormat == TSVideoFormat.VIDEOFORMAT_1080p || track.VideoFormat == TSVideoFormat.VIDEOFORMAT_1080i);
            var first720 = _videoTracks.FirstOrDefault(track => track.VideoFormat == TSVideoFormat.VIDEOFORMAT_720p);
            var first576 = _videoTracks.FirstOrDefault(track => track.VideoFormat == TSVideoFormat.VIDEOFORMAT_576p || track.VideoFormat == TSVideoFormat.VIDEOFORMAT_576i);
            var firstAny = _videoTracks.FirstOrDefault();

            var bestTrack = first1080 ?? first720 ?? first576 ?? firstAny;

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
                streamItem.Checked = stream == bestTrack;
                streamItem.ImageIndex = i++;
                if (stream.IsHidden)
                    streamItem.ForeColor = SystemColors.GrayText;
                listViewVideoTracks.Items.Add(streamItem);

                icons.Images.Add(TSStream.GetCodecIcon(stream.StreamType));
            }

            listViewVideoTracks.SmallImageList = icons;
        }

        private void PopulateAudioTracks()
        {
            var i = 0;
            var icons = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            var preferredAudioCodecs = BDAutoMuxerSettings.PreferredAudioCodecs;
            var minChannelCount = 0;

            if (BDAutoMuxerSettings.SelectHighestChannelCount)
            {
                var highestTrack = _audioTracks.OrderByDescending(a => a.ChannelCount).FirstOrDefault();
                if (highestTrack != null)
                    minChannelCount = highestTrack.ChannelCount;
            }

            foreach (TSAudioStream stream in _audioTracks.Where(track => track.StreamType != TSStreamType.DTS_HD_SECONDARY_AUDIO))
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
                streamItem.Checked = preferredAudioCodecs.Any(audioCodec => audioCodec == MediaInfoHelper.CodecFromStream(stream)) && stream.ChannelCount >= minChannelCount;
                streamItem.ImageIndex = i++;
                if (stream.IsHidden)
                    streamItem.ForeColor = SystemColors.GrayText;
                listViewAudioTracks.Items.Add(streamItem);

                icons.Images.Add(TSStream.GetCodecIcon(stream.StreamType));
            }

            // If none of the filtered audio tracks contain a preferred codec, select the first track as a fallback
            if (listViewAudioTracks.CheckedIndices.Count == 0 && listViewAudioTracks.Items.Count > 0)
                listViewAudioTracks.Items[0].Checked = true;

            listViewAudioTracks.SmallImageList = icons;
        }

        private void PopulateSubtitleTracks()
        {
            var i = 0;
            var icons = new ImageList { ColorDepth = ColorDepth.Depth32Bit };

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
                if (stream.IsHidden)
                    streamItem.ForeColor = SystemColors.GrayText;
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

            SetTabStatus(tabPagePlaylists, "Querying main movie database...");

            _mainMovieBackgroundWorker = new BackgroundWorker();
            _mainMovieBackgroundWorker.WorkerReportsProgress = true;
            _mainMovieBackgroundWorker.WorkerSupportsCancellation = true;
            _mainMovieBackgroundWorker.DoWork += mainMovieBackgroundWorker_DoWork;
            _mainMovieBackgroundWorker.ProgressChanged += mainMovieBackgroundWorker_ProgressChanged;
            _mainMovieBackgroundWorker.RunWorkerCompleted += mainMovieBackgroundWorker_RunWorkerCompleted;
            _mainMovieBackgroundWorker.RunWorkerAsync();
        }

        private void SearchTmdb(object sender = null, EventArgs e = null)
        {
            Language language = discLanguageComboBox.SelectedValue as Language;

            if (String.IsNullOrEmpty(movieNameTextBox.Text) || language == null)
            {
                return;
            }

            _isSearchingTmdb = true;

            ResetUI();
            ClearTmdb();

            SetTabStatus(tabPagePlaylists, "Searching The Movie Database (TMDb)...");

            string ISO_639_1 = language.ISO_639_1;
            string query = movieNameTextBox.Text;
            int? year = Regex.IsMatch(textBoxYear.Text, @"^\d{4}$") ? (int?)Int32.Parse(textBoxYear.Text) : null;

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
                IList<TSPlaylistFile> mainPlaylists = _playlists.Where(playlist => playlist.IsFeatureLength).ToList();
                _mainMovieSearchResult = null;
                _mainMovieSearchResult = OldMainMovieService.FindMainMovie(_bdrom.VolumeLabel, mainPlaylists);
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
                ShowErrorMessage(tabPagePlaylists, errorCaption, exception.Message);
            }

            if (_mainMovieSearchResult == null) return;

            if (_mainMovieSearchResult.error)
            {
                string errorMessages = _mainMovieSearchResult.errors.Aggregate("", (current, error) => current + (error.textStatus + " - " + error.errorMessage + "\n"));
                string errorMessage = "Main movie service returned the following error(s): \n\n" + errorMessages;
                ShowErrorMessage(tabPagePlaylists, errorCaption, errorMessage);
            }
            else if (_mainMovieSearchResult.discs.Count == 0)
            {
                ShowExclamationMessage(tabPagePlaylists, "No results found", "No matching discs were found in the database.\n\n" + "Please submit one!");
            }
            else
            {
                JsonDisc disc = _mainMovieSearchResult.discs[0];

                _autoConfigured = true;
                _autoTmdbId = disc.tmdb_id;

                movieNameTextBox.Text = disc.movie_title;
                textBoxYear.Text = disc.year != null ? disc.year.ToString() : null;

                _populator.AutoConfigure(disc.playlists);

                int count = _mainMovieSearchResult.discs.Count;
                string plural = count != 1 ? "s" : "";
                string caption = string.Format("{0:d} result{1} found", count, plural);
                string message = string.Format("Hooray!  Found {0:d} matching disc{1} in the database.", count, plural);
                ShowMessage(tabPagePlaylists, caption, message);
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
                ShowErrorMessage(tabPagePlaylists, errorCaption, exception.Message);
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
                SetTabStatus(tabPagePlaylists, string.Format("{0:d} result{1} found", count, plural));

                searchResultListView.Enabled = true;
                searchResultListView.Select();
                searchResultListView.Items[0].Selected = true;
            }
            else
            {
                ShowExclamationMessage(tabPagePlaylists, "No results found", "No matching movies found in The Movie Database (TMDb)");
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

            BDAutoMuxerSettings.OutputDir = textBoxOutputDir.Text;
            BDAutoMuxerSettings.OutputFileName = textBoxOutputFileName.Text;
            BDAutoMuxerSettings.ReplaceSpaces = checkBoxReplaceSpaces.Checked;
            BDAutoMuxerSettings.ReplaceSpacesWith = textBoxReplaceSpaces.Text;
            BDAutoMuxerSettings.DemuxLPCM = checkBoxDemuxLPCM.Checked;
            BDAutoMuxerSettings.DemuxSubtitles = checkBoxDemuxSubtitles.Checked;
            BDAutoMuxerSettings.SaveSettings();

            _muxer = null;
            _demuxer = null;

            _eac3ToProgressUIState.Reset();
            _tsMuxerProgressUIState.Reset();

            _shouldMux = SelectedStreams.Any(ShouldMuxTrack);

            groupBoxDemuxingProgress.Visible = IsDemuxLPCMChecked || IsDemuxSubtitlesChecked;
            groupBoxMuxingProgress.Visible = _shouldMux;

            if (!groupBoxDemuxingProgress.Visible && groupBoxMuxingProgress.Visible)
                groupBoxMuxingProgress.Location = _demuxingProgressLocation;
            else
                groupBoxMuxingProgress.Location = _muxingProgressLocation;

            if (IsDemuxLPCMChecked || IsDemuxSubtitlesChecked)
                StartDemuxer(SelectedStreams);
            else if (_shouldMux)
                StartMuxer(SelectedStreams);
        }

        private ISet<TSStream> SelectedStreams
        {
            get
            {
                ISet<TSStream> selectedStreams = new HashSet<TSStream>();
                selectedStreams.UnionWith(SelectedVideoStreams);
                selectedStreams.UnionWith(SelectedAudioStreams);
                selectedStreams.UnionWith(SelectedSubtitleStreams);
                return selectedStreams;
            }
        }

        // TODO: Finish me!
        private void StartDemuxer(ISet<TSStream> selectedStreams)
        {
            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, GetDemuxerProgressMessage("0.0%"));
            textBoxDemuxingCommandLine.Text = "";
            textBoxTsMuxerCommandLine.Text = "";
            progressBarTsMuxer.Value = 0;
            toolStripProgressBar.Visible = true;

            _demuxer = new TsMuxer(_bdrom, SelectedPlaylist, selectedStreams, IsDemuxLPCMChecked, IsDemuxSubtitlesChecked);
            _demuxer.WorkerReportsProgress = true;
            _demuxer.WorkerSupportsCancellation = true;
            _demuxer.ProgressChanged += DemuxerBackgroundWorkerProgressChanged;
            _demuxer.RunWorkerCompleted += DemuxerBackgroundWorkerCompleted;

            _demuxer.RunWorkerAsync(_tsMuxerOutputPath);
        }

        private void StartMuxer(ISet<TSStream> selectedStreams)
        {
            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, "Muxing M2TS: 0.0%");
            textBoxTsMuxerCommandLine.Text = "";
            toolStripProgressBar.Visible = true;

            var selectedMuxingStreams = new HashSet<TSStream>(selectedStreams);

            if (IsDemuxLPCMChecked)
                selectedMuxingStreams.RemoveWhere(stream => stream.StreamType == TSStreamType.LPCM_AUDIO);

            if (IsDemuxSubtitlesChecked)
                selectedMuxingStreams.RemoveWhere(stream => stream.IsGraphicsStream || stream.IsTextStream);

            _muxer = new TsMuxer(_bdrom, SelectedPlaylist, selectedMuxingStreams);
            _muxer.WorkerReportsProgress = true;
            _muxer.WorkerSupportsCancellation = true;
            _muxer.ProgressChanged += MuxerBackgroundWorkerProgressChanged;
            _muxer.RunWorkerCompleted += MuxerBackgroundWorkerCompleted;

            _muxer.RunWorkerAsync(_tsMuxerOutputPath);
        }

        private void CancelRip()
        {
            _isCancelling = true;
            ResetUI();
            CancelRip(_muxer);
            CancelRip(_demuxer);
        }

        private static void CancelRip(AbstractExternalTool tool)
        {
            if (tool == null || !tool.IsBusy) return;
            tool.Resume();
            tool.CancelAsync();
        }

        private void MuxerBackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _isMuxing = true;
            ResetUI();
            UpdateTsMuxerProgress(sender, e);
        }

        private void UpdateTsMuxerProgress(object sender = null, EventArgs e = null)
        {
            // 0.0 to 100.0
            double muxerProgressPct = _muxer.Progress;
            double overallProgressPct = muxerProgressPct;

            if (_demuxer != null)
            {
                overallProgressPct /= 2;
                overallProgressPct += 50.0;
            }

            string muxerProgressStr = muxerProgressPct.ToString("##0.0") + "%";
            string overallProgressStr = overallProgressPct.ToString("##0.0") + "%";

            int muxerProgressValue = (int)(muxerProgressPct * 10);
            int overallProgressValue = (int)(overallProgressPct * 10);

            // To show fractional progress, progress bar range is 0 to 1000 (instead of 0 to 100)
            progressBarTsMuxer.Value = muxerProgressValue;

            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                // TODO: This throws a NPE if the window is closed while muxing is in progress
                toolStripProgressBar.Value = overallProgressValue;
            }
            catch
            {
            }
            // ReSharper restore EmptyGeneralCatchClause

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.Normal, Handle);
            TaskbarProgress.SetProgressValue(overallProgressValue, 1000, Handle);

            labelTsMuxerProgress.Text = muxerProgressStr;
            progressLabel.Text = overallProgressStr;

            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, "Muxing M2TS: " + muxerProgressStr);

            if (String.IsNullOrEmpty(textBoxTsMuxerCommandLine.Text))
                textBoxTsMuxerCommandLine.Text = _muxer.CommandLine;

            labelTsMuxerTimeRemaining.Text = GetElapsedTimeString(_muxer.TimeRemaining);
            labelTsMuxerTimeElapsed.Text = GetElapsedTimeString(_muxer.TimeElapsed);

            // TODO: Refactor this logic into a separate method
            if (!String.IsNullOrEmpty(_muxer.State))
            {
                labelTsMuxerProgress.Text += String.Format(" ({0})", _muxer.State);

                if (_muxer.IsPaused)
                    TaskbarProgress.SetProgressState(TaskbarProgressBarState.Paused, Handle);
                else if (_muxer.IsCanceled || _muxer.IsError)
                    TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
            }
        }

        private void MuxerBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isMuxing = false;

            ResetUI();

            if (e.Cancelled && _muxer.IsCanceled)
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                SetTabStatus(tabPageProgress, "Muxing canceled");
            }
            else if (e.Error != null)
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                ShowErrorMessage(tabPageProgress, "Muxing error", e.Error.Message);
            }
            else
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.NoProgress, Handle);
                ShowMessage(tabPageProgress, "Muxing completed!", "Finished muxing M2TS with tsMuxeR!");
            }

            if (_muxer != null && (_muxer.IsCanceled || _muxer.IsError))
                CleanupFiles();

            _isCancelling = false;

            ResetUI();
        }

        private void CleanupFiles()
        {
            var junkFiles = new HashSet<string>();

            if (_demuxer != null && _demuxer.HasOutputFiles())
                junkFiles.AddRange(_demuxer.GetOutputFiles());

            if (_muxer != null && _muxer.HasOutputFiles())
                junkFiles.AddRange(_muxer.GetOutputFiles());

            var existingJunkFiles = junkFiles.Where(File.Exists).ToList();

            if (!existingJunkFiles.Any())
                return;

            if (PromptToDeleteOutputFiles(existingJunkFiles.ToList()))
                AbstractExternalTool.DeleteOutputFiles(existingJunkFiles);
        }

        public bool PromptToDeleteOutputFiles(ICollection<string> outputFiles)
        {
            if (!outputFiles.Any()) return false;
            var files = String.Join("\n", outputFiles.Select(path => string.Format("\t{0}", path)));
            var message = String.Format("The following files are incomplete and unusable:\n\n{0}\n\nDo you want to delete them?", files);
            return MessageBox.Show(message, "Delete output files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void DemuxerBackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _isMuxing = true;
            ResetUI();
            UpdateDemuxerProgress(sender, e);
        }

        private void UpdateDemuxerProgress(object sender = null, EventArgs e = null)
        {
            // 0.0 to 100.0
            double demuxerProgressPct = _demuxer.Progress;
            double overallProgressPct = demuxerProgressPct;

            if (_shouldMux)
                overallProgressPct /= 2;

            string demuxerProgressStr = demuxerProgressPct.ToString("##0.0") + "%";
            string overallProgressStr = overallProgressPct.ToString("##0.0") + "%";

            int demuxerProgressValue = (int)(demuxerProgressPct * 10);
            int overallProgressValue = (int)(overallProgressPct * 10);

            // To show fractional progress, progress bar range is 0 to 1000 (instead of 0 to 100)
            progressBarDemuxing.Value = demuxerProgressValue;

            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                // TODO: This throws a NPE if the window is closed while muxing is in progress
                toolStripProgressBar.Value = overallProgressValue;
            }
            catch
            {
            }
            // ReSharper restore EmptyGeneralCatchClause

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.Normal, Handle);
            TaskbarProgress.SetProgressValue(overallProgressValue, 1000, Handle);

            labelDemuxingProgress.Text = demuxerProgressStr;
            progressLabel.Text = overallProgressStr;

            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, GetDemuxerProgressMessage(demuxerProgressStr));

            if (String.IsNullOrEmpty(textBoxDemuxingCommandLine.Text))
                textBoxDemuxingCommandLine.Text = _demuxer.CommandLine;

            labelDemuxingTimeRemaining.Text = GetElapsedTimeString(_demuxer.TimeRemaining);
            labelDemuxingTimeElapsed.Text = GetElapsedTimeString(_demuxer.TimeElapsed);

            // TODO: Refactor this logic into a separate method
            if (!String.IsNullOrEmpty(_demuxer.State))
            {
                labelDemuxingProgress.Text += String.Format(" ({0})", _demuxer.State);

                if (_demuxer.IsPaused)
                    TaskbarProgress.SetProgressState(TaskbarProgressBarState.Paused, Handle);
                else if (_demuxer.IsCanceled || _demuxer.IsError)
                    TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
            }
        }

        private string GetDemuxerProgressMessage(string demuxerProgressStr)
        {
            string types;
            if (IsDemuxLPCMChecked && IsDemuxSubtitlesChecked)
                types = "LPCM and subtitles";
            else if (IsDemuxLPCMChecked)
                types = "LPCM";
            else if (IsDemuxSubtitlesChecked)
                types = "subtitles";
            else
                types = "tracks";
            return string.Format("Demuxing {0}: {1}", types, demuxerProgressStr);
        }

        private void DemuxerBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isMuxing = false;

            ResetUI();

            if (e.Cancelled && _demuxer.IsCanceled)
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                SetTabStatus(tabPageProgress, "Demuxing canceled");
            }
            else if (e.Error != null)
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                ShowErrorMessage(tabPageProgress, "Demuxing Error", e.Error.Message);
            }
            else
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.NoProgress, Handle);

                if (_shouldMux)
                    StartMuxer(SelectedStreams);
            }

            if (_demuxer != null && (_demuxer.IsCanceled || _demuxer.IsError))
                CleanupFiles();

            _isCancelling = false;

            ResetUI();
        }

        private bool ShouldMuxTrack(TSStream stream)
        {
            if (IsDemuxLPCMChecked && stream.StreamType == TSStreamType.LPCM_AUDIO)
                return false;
            if (IsDemuxSubtitlesChecked && stream.IsTextStream || stream.IsGraphicsStream)
                return false;
            return true;
        }

        private static string GetElapsedTimeString(TimeSpan elapsedTime)
        {
            if (elapsedTime == TimeSpan.MaxValue)
                elapsedTime = TimeSpan.Zero;

            return string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                elapsedTime.Hours,
                elapsedTime.Minutes,
                elapsedTime.Seconds);
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
                var postResult = OldMainMovieService.PostDisc(jsonDisc);
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
            if (_populator != null && language != null && !_autoConfigured)
                _populator.MainLanguageCode = language.ISO_639_2;
        }

        private void TmdbSearchTextChanged(object sender, EventArgs e)
        {
            textBoxOutputDir_TextChanged(sender, e);
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
                    {
                        pictureBoxMoviePoster.Image = Resources.no_poster_w185;
                        pictureBoxCoverArt.Image = Resources.no_poster_w185;
                    }

                    else
                    {
                        var image = _imageCache.GetImage(_rootUrl + _tmdbMovieResult.poster_path);
                        pictureBoxMoviePoster.Image = image;
                        pictureBoxCoverArt.Image = image;
                        getLanguageList();
                    }

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
            if (!_ignoreFilterControlChange)
            {
                SetAudienceLanguage(VideoLanguages, comboBoxVideoLanguage);
                SetAudienceLanguage(AudioLanguages, listBoxAudioLanguages);
                SetAudienceLanguage(SubtitleLanguages, listBoxSubtitleLanguages);
            }
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
            if (!_initialized || e.Button != MouseButtons.Right) return;

            int row = playlistDataGridView.HitTest(e.X, e.Y).RowIndex;
            int col = playlistDataGridView.HitTest(e.X, e.Y).ColumnIndex;

            if (row < 0) return;

            col = col < 0 ? 0 : col;

            playlistDataGridView.CurrentCell = playlistDataGridView[col, row];

            TSPlaylistFile playlist = _populator.PlaylistAt(row);
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

        private bool SelectedPlaylistHasUnsupportedCodecs
        {
            get
            {
                return
                    _populator != null && _populator.SelectedPlaylist != null &&
                    _populator.SelectedPlaylist.SortedStreams.Any(stream => stream.StreamType == TSStreamType.DTS_HD_SECONDARY_AUDIO || stream.StreamType == TSStreamType.INTERACTIVE_GRAPHICS);
            }
        }

        private bool SelectedPlaylistHasHiddenTracks
        {
            get
            {
                return
                    _populator != null && _populator.SelectedPlaylist != null &&
                    _populator.SelectedPlaylist.SortedStreams.Any(stream => stream.IsHidden);
            }
        }

        private bool DiscTabEnabled
        {
            set
            {
                panelMovieDetails.Enabled = value;
                checkBoxShowLowQualityPlaylists.Enabled = value && _playlists != null &&
                                                     _playlists.Any(playlist => playlist.IsLowQualityOnly);
                checkboxShowBogusPlaylists.Enabled = value && _playlists != null &&
                                                     _playlists.Any(playlist => playlist.IsBogusOnly);
                checkBoxShowShortPlaylists.Enabled = value && _playlists != null &&
                                                     _playlists.Any(playlist => playlist.IsShort);
            }
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
                checkBoxDemuxLPCM.Enabled = value && SelectedAudioStreams.Any(audio => audio.StreamType == TSStreamType.LPCM_AUDIO);
                checkBoxDemuxSubtitles.Enabled = value && SelectedSubtitleStreams.Any();
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

        private void AutoEnableDiscTab()
        {
            DiscTabEnabled = _initialized && !_isSearchingMainMovieDb && !_isSearchingTmdb && !_isMuxing;
        }

        private void AutoEnableOutputTab()
        {
            OutputTabEnabled = !_isMuxing;
        }

        private void ResetUI()
        {
            if (Disposing || IsDisposed) return;

            // Scan controls
            EnableScanControls = !_isScanningBDROM && !_isSearchingMainMovieDb && !_isSearchingTmdb && !_isMuxing;

            // All tabs
            tabControl.Enabled = _initialized && !_isSearchingMainMovieDb;

            // Disc tab
            AutoEnableDiscTab();

            // Output tab
            AutoEnableOutputTab();

            // Progress tab
            ShowProgressTabPage = ShowProgressTabPage || _isMuxing;

            var notices = new List<string>();
            var hasHidden = SelectedPlaylistHasHiddenTracks;
            var hasUnsupported = SelectedPlaylistHasUnsupportedCodecs;

            if (hasHidden)
                notices.Add("* Hidden track");
            if (hasUnsupported)
                notices.Add("** Unsupported codec - will not be muxed");

            hiddenTrackLabel.Text = string.Join("  ", notices);
            hiddenTrackLabel.Visible = hasHidden || hasUnsupported;

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

            if (posterList != null && posterList.Count > 0)
            {
                posterList.Clear();
                posterIndex = 0;
            }

            buttonSubmitToDB.Enabled = CanSubmitToDb;
            submitToDbToolStripMenuItem.Enabled = CanSubmitToDb;

            continueButton.Text = _isMuxing ? (IsMuxingOrDemuxingPaused ? "Resume" : "Pause") : "Mux it!";
            continueButton.Visible = _initialized && (_isMuxing || tabControl.SelectedTab == tabPageOutput) && SelectedPlaylist != null;
            cancelButton.Text = _isMuxing ? "Stop" : "Close";

            continueButton.Enabled = cancelButton.Enabled = !_isCancelling;

            ResizeDiscTab();
            ResizeOutputTab();

            RefreshOutputPaths();
        }

        private void RefreshOutputPaths()
        {
            textBoxOutputDir_TextChanged();
            textBoxOutputFileName_TextChanged();
        }

        private bool IsMuxingOrDemuxingPaused
        {
            get { return (_muxer != null && _muxer.IsPaused) || (_demuxer != null && _demuxer.IsPaused); }
        }

        private static void PauseResume(AbstractExternalTool tool, Control progressLabel)
        {
            if (tool == null) return;

            // TODO: Refactor label text logic into a separate method
            if (tool.IsPaused)
            {
                tool.Resume();
                progressLabel.Text = progressLabel.Text.Replace(" (paused}", "");
            }
            else
            {
                tool.Pause();
                progressLabel.Text += " (paused}";
            }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (_isMuxing)
            {
                if (_demuxer != null && _demuxer.IsBusy)
                    PauseResume(_demuxer, labelDemuxingProgress);
                else
                    PauseResume(_muxer, labelTsMuxerProgress);
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

        private static string Sanitize(string text)
        {
            return FileUtils.SanitizeFileName(text);
        }

        private string ReplacePlaceholders(string text)
        {
            if (!_initialized)
                return text;

            string preview = text;

            var videoLanguage = comboBoxVideoLanguage.SelectedValue as Language;

            string volume = _bdrom.VolumeLabel;
            string title = String.IsNullOrEmpty(_tmdbMovieTitle) ? movieNameTextBox.Text : _tmdbMovieTitle;
            string year = _tmdbMovieYear == null ? GetYearString(textBoxYear.Text) : _tmdbMovieYear + "";
            string res = "";
            string vcodec = "";
            string acodec = "";
            string channels = "";
            string cut = (comboBoxCut.SelectedValue is Cut ? (Cut)comboBoxCut.SelectedValue : Cut.Theatrical).ToString();
            string vlang = videoLanguage != null ? videoLanguage.ISO_639_2.ToUpperInvariant() : "";

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

            volume = Sanitize(volume);
            title = Sanitize(title);
            year = Sanitize(year);
            res = Sanitize(res);
            vcodec = Sanitize(vcodec);
            acodec = Sanitize(acodec);
            channels = Sanitize(channels);
            cut = Sanitize(cut);
            vlang = Sanitize(vlang);

            preview = Regex.Replace(preview, @"%volume%", volume, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%title%", title, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%year%", year, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%res%", res, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%vcodec%", vcodec, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%acodec%", acodec, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%channels%", channels, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%cut%", cut, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%vlang%", vlang, RegexOptions.IgnoreCase);
            preview = Environment.ExpandEnvironmentVariables(preview);

            if (checkBoxReplaceSpaces.Checked)
            {
                preview = preview.Replace(" ", Sanitize(textBoxReplaceSpaces.Text));
            }

            return preview;
        }

        private void textBoxOutputDir_TextChanged(object sender = null, EventArgs e = null)
        {
            textBoxOutputDirPreview.Text = ReplacePlaceholders(textBoxOutputDir.Text);
        }

        private void textBoxOutputFileName_TextChanged(object sender = null, EventArgs e = null)
        {
            textBoxOutputFileNamePreview.Text = Sanitize(ReplacePlaceholders(textBoxOutputFileName.Text)) + labelOutputFileExtension.Text;
        }

        private void textBoxReplaceSpaces_TextChanged(object sender = null, EventArgs e = null)
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
            _populator.SelectLikely = true;
            _ignoreDataGridItemChange = false;
            OnPlaylistItemChange(sender, e);
        }

        private void buttonUnselectAll_Click(object sender, EventArgs e)
        {
            _ignoreDataGridItemChange = true;
            _populator.SelectLikely = false;
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
            checkForUpdatesToolStripMenuItem.Enabled = false;
            UpdateNotifier.CheckForUpdate(this, true, _updateNotifierComplete);
        }

        private void remuxerToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

        private void contextMenuStripTmdb_Opened(object sender, EventArgs e)
        {
            if (toolStripMenuItemTmdb.Image == null)
                toolStripMenuItemTmdb.Image = Win32.Browser.DefaultBrowserIconAsBitmap;
            toolStripMenuItemTmdb.Enabled = _tmdbMovieUrl != null;
        }

        private void toolStripMenuItemTmdb_Click(object sender, EventArgs e)
        {
            if (_tmdbMovieUrl != null)
                Process.Start(_tmdbMovieUrl);
        }

        private void OutputTrackChecked(object sender, ItemCheckedEventArgs e)
        {
            RefreshOutputPaths();
            AutoEnableOutputTab();
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

        private void comboBoxLangauge_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var index = comboBoxLangauge.SelectedIndex;
            var x = (ComboboxItem)comboBoxLangauge.Items[index];
            _selectedLanguage = x.Value.ToString();

            getImageList(_tmdbMovieResult.id);
        }

        private void comboBoxLangauge_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void pictureBoxCoverArt_Click(object sender, EventArgs e)
        {

            if (posterList != null && posterList.Count > 0)
            {
                getCoverArt();
            }
            else
                getImageList(_tmdbMovieResult.id);
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

        private void CheckForUpdateOnStartup()
        {
            if (BDAutoMuxerSettings.CheckForUpdates && !ClickOnceUpdateService.IsClickOnce)
            {
                checkForUpdatesToolStripMenuItem.Enabled = false;
                UpdateNotifier.CheckForUpdate(this, false, _updateNotifierComplete);
            }
        }

        private bool ShouldAbortMuxing()
        {
            return MessageBox.Show(this, "Abort muxing?", "Abort muxing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        #endregion

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            new FormCodecScanner().Show();
        }

        private void codecsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCodecReference();
        }

        private void linkLabelPreviewPlaylistsUI_Click(object sender, EventArgs e)
        {
        }

        private void findIncompleteMKVsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormFindIncompleteMedia().Show();
        }
        
        #region CoverArt

        private void linkLabelEdit_Click(object sender, EventArgs e)
        {
            if (posterList != null && posterList.Count > 0)
            {
                getCoverArt();
            }
            else
                getImageList(_tmdbMovieResult.id);
        }

        // Language Worker
        private void getLanguageList()
        {
            comboBoxLangauge.Items.Clear();
            var tmdbLanguageBackgroundWorker = new BackgroundWorker();
            tmdbLanguageBackgroundWorker.DoWork += tmdbLanguageBackgroundWorkerOnDoWork;
            tmdbLanguageBackgroundWorker.RunWorkerCompleted += tmdbLanguageBackgroundWorkerOnRunWorkerCompleted;
            tmdbLanguageBackgroundWorker.RunWorkerAsync();
        }

        private void tmdbLanguageBackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            languageList = runWorkerCompletedEventArgs.Result as List<string>;
            if (languageList != null && languageList.Count > 0)
            {
                var languegeList = languageList.Distinct().ToList();
                languegeList.Sort();
                var languages = languegeList;
                
                for (var i = 0; i < languages.Count(); i++ )
                {
                    var language = languages[i];
                    if (!string.IsNullOrEmpty(language))
                    {
                        try
                        {
                            var lang = new CultureInfo(language).DisplayName;

                            ComboboxItem item = new ComboboxItem();
                            item.Text = lang;
                            item.Value = language;
                            comboBoxLangauge.Items.Add(item);
                        }
                        catch {}
                    }
                    else
                    {
                        /*To Do:  Handle Null Countries*/
                    }
                }

                comboBoxLangauge.SelectedIndex = comboBoxLangauge.FindStringExact("English") ;
            }
        }

        private void tmdbLanguageBackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var tmdbMovieImages = _tmdbApi.GetMovieImages(_tmdbMovieResult.id, null);
            doWorkEventArgs.Result = (tmdbMovieImages.posters.Select(poster => poster.iso_639_1).ToList());
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        // Poster Worker
        private void getImageList(int mID)
        {
            posterIndex = 0;
            var tmdbImagebackgroundWorker = new BackgroundWorker();
            tmdbImagebackgroundWorker.DoWork += tmdbImagebackgroundWorkerOnDoWork;
            tmdbImagebackgroundWorker.RunWorkerCompleted += tmdbImagebackgroundWorkerOnRunWorkerCompleted;
            tmdbImagebackgroundWorker.RunWorkerAsync();
        }

        private void tmdbImagebackgroundWorkerOnRunWorkerCompleted(object sender,RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            posterList = runWorkerCompletedEventArgs.Result as List<string>;
            if (posterList != null && posterList.Count > 0)
            {
                getCoverArt();
            }
        }

        private void tmdbImagebackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            if (!string.IsNullOrEmpty(_selectedLanguage))
            {
                var tmdbMovieImages = _tmdbApi.GetMovieImages(_tmdbMovieResult.id, _selectedLanguage);
                doWorkEventArgs.Result = (tmdbMovieImages.posters.Select(poster => _rootUrl + poster.file_path).ToList());
            }
            else
            {
                var tmdbMovieImages = _tmdbApi.GetMovieImages(_tmdbMovieResult.id, "en");
                doWorkEventArgs.Result = (tmdbMovieImages.posters.Select(poster => _rootUrl + poster.file_path).ToList());
            }
        }
        
        private void getCoverArt()
        {
            var formCoverArt = new FormCoverArt(posterList, posterIndex, _selectedLanguage);
            var dialogResult = formCoverArt.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                posterIndex = formCoverArt.SelectedIndex;
                if (posterIndex >= 0 && posterList != null)
                {
                    var posterUrl = posterList[posterIndex];
                    pictureBoxCoverArt.Image = _imageCache.GetImage(posterUrl);
                }
            }
        }

        

        

        #endregion
        
    }

    class ProgressUIState
    {
        private readonly Label _percent;
        private readonly Label _timeRemaining;
        private readonly Label _timeElapsed;
        private readonly ProgressBar _progressBar;
        private readonly TextBox _commandLine;

        private readonly string _percentTextInitial;
        private readonly string _timeRemainingTextInitial;
        private readonly string _timeElapsedTextInitial;

        public ProgressUIState(Label percent, Label timeRemaining, Label timeElapsed, ProgressBar progressBar, TextBox commandLine)
        {
            _percent = percent;
            _timeRemaining = timeRemaining;
            _timeElapsed = timeElapsed;
            _progressBar = progressBar;
            _commandLine = commandLine;

            _percentTextInitial = _percent.Text;
            _timeRemainingTextInitial = _timeRemaining.Text;
            _timeElapsedTextInitial = _timeElapsed.Text;
        }

        public void Reset()
        {
            _percent.Text = _percentTextInitial;
            _timeRemaining.Text = _timeRemainingTextInitial;
            _timeElapsed.Text = _timeElapsedTextInitial;
            _progressBar.Value = 0;
            _commandLine.Text = null;
        }
    }

    enum ShowHiddenTracksOption
    {
        No, Yes
    }
}
