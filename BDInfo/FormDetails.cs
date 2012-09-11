using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WatTmdb.V3;
using System.Text.RegularExpressions;
using System.Globalization;
using BDInfo.views;
using Newtonsoft.Json;
using BDInfo.models;
using BDInfo.controllers;
using System.IO;
using System.Runtime.InteropServices;
using BDInfo.Properties;

namespace BDInfo
{
    public partial class FormDetails : Form
    {
        #region Fields

        private readonly string tmdb_api_key = "b59b366b0f0a457d58995537d847409a";
        private Tmdb tmdb_api;

        private BDROM BDROM;
        private IList<TSPlaylistFile> playlists;
        private IList<Language> languages;
        private IList<string> languageCodes = new List<string>();

        private TmdbMovieSearch tmdbMovieSearch;
        private MovieResult tmdbMovieResult = null;

        private MainMovieService mainMovieService = new MainMovieService();
        private JsonSearchResult mainMovieSearchResult;

        private BackgroundWorker mainMovieBackgroundWorker;
        private BackgroundWorker tmdbBackgroundWorker;

        private PlaylistDataGridPopulator populator;

        private bool auto_configured = false;
        private int auto_tmdb_id = -1;

        private bool ignoreFilterControlChange = false;
        private bool ignoreDataGridItemChange = false;

        private TsMuxer tsMuxer;
        private bool isMuxing = false;

        Settings settings = Settings.Default;

        private ISet<TSPlaylistFile> filteredPlaylists = new HashSet<TSPlaylistFile>();

        private IList<Language> audioLanguages = new List<Language>();
        private IList<Language> subtitleLanguages = new List<Language>();

        private List<TSVideoStream> videoTracks = new List<TSVideoStream>();
        private List<TSAudioStream> audioTracks = new List<TSAudioStream>();
        private List<TSStream> subtitleTracks = new List<TSStream>();

        #endregion

        #region Properties

        private bool IsMuxing
        {
            get { return isMuxing; }
            set
            {
                isMuxing = value;
                if (isMuxing)
                {

                }
            }
        }

        private Language[] GetSortedLanguageArray(ICollection<Language> collection)
        {
            Language[] array = new Language[collection.Count];

            int i = 0;
            foreach (Language value in languages)
            {
                if (collection.Contains(value))
                {
                    array[i++] = value;
                }
            }

            return array;
        }

        private Language[] VideoLanguages
        {
            get
            {
                return GetSortedLanguageArray(populator.SelectedVideoLanguages);
            }
        }

        private Language[] AudioLanguages
        {
            get
            {
                ISet<Language> audioLanguages = new HashSet<Language>();
                foreach (TSPlaylistFile playlist in playlists)
                {
                    foreach (TSAudioStream audioStream in playlist.AudioStreams)
                    {
                        audioLanguages.Add(Language.GetLanguage(audioStream.LanguageCode));
                    }
                }
                return GetSortedLanguageArray(audioLanguages);
            }
        }

        private Language[] SubtitleLanguages
        {
            get
            {
                ISet<Language> subtitleLanguages = new HashSet<Language>();
                foreach (TSPlaylistFile playlist in playlists)
                {
                    foreach (TSGraphicsStream graphicsStream in playlist.GraphicsStreams)
                    {
                        subtitleLanguages.Add(Language.GetLanguage(graphicsStream.LanguageCode));
                    }
                    foreach (TSTextStream textStream in playlist.TextStreams)
                    {
                        subtitleLanguages.Add(Language.GetLanguage(textStream.LanguageCode));
                    }
                }
                return GetSortedLanguageArray(subtitleLanguages);
            }
        }

        private Cut[] GetSortedCutArray(ICollection<Cut> collection)
        {
            Cut[] array = new Cut[collection.Count];

            int i = 0;
            foreach (Cut value in Enum.GetValues(typeof(Cut)))
            {
                if (collection.Contains(value))
                {
                    array[i++] = value;
                }
            }

            return array;
        }

        private Cut[] Cuts
        {
            get
            {
                return GetSortedCutArray(populator.SelectedCuts);
            }
        }

        private CommentaryOption[] GetSortedCommentaryOptionArray(ICollection<CommentaryOption> collection)
        {
            CommentaryOption[] array = new CommentaryOption[collection.Count];

            int i = 0;
            foreach (CommentaryOption value in Enum.GetValues(typeof(CommentaryOption)))
            {
                if (collection.Contains(value))
                {
                    array[i++] = value;
                }
            }

            return array;
        }

        private CommentaryOption[] CommentaryOptions
        {
            get
            {
                return GetSortedCommentaryOptionArray(populator.SelectedCommentaryOptions);
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
                List<TSVideoStream> selectedStreams = new List<TSVideoStream>();
                foreach (ListViewItem item in listViewVideoTracks.CheckedItems)
                {
                    selectedStreams.Add(item.Tag as TSVideoStream);
                }
                return selectedStreams;
            }
        }

        private List<TSAudioStream> SelectedAudioStreams
        {
            get
            {
                List<TSAudioStream> selectedStreams = new List<TSAudioStream>();
                foreach (ListViewItem item in listViewAudioTracks.CheckedItems)
                {
                    selectedStreams.Add(item.Tag as TSAudioStream);
                }
                return selectedStreams;
            }
        }

        private List<TSStream> SelectedSubtitleStreams
        {
            get
            {
                List<TSStream> selectedStreams = new List<TSStream>();
                foreach (ListViewItem item in listViewSubtitleTracks.CheckedItems)
                {
                    selectedStreams.Add(item.Tag as TSStream);
                }
                return selectedStreams;
            }
        }

        #endregion

        #region Initialization

        public FormDetails(BDROM BDROM, List<TSPlaylistFile> playlists, ISet<Language> languages)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.languages = new List<Language>(languages).ToArray();
            this.playlists = TSPlaylistFile.Sort(playlists);

            string ISO_639_1 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_1 : null;
            string ISO_639_2 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_2 : null;

            // TODO: This will fail if we're unable to auto-detect the disc language (e.g., ID4)
            //       or if the user changes the main disc language manually.
            if (String.IsNullOrEmpty(ISO_639_1))
                tmdb_api = new Tmdb(tmdb_api_key);
            else
                tmdb_api = new Tmdb(tmdb_api_key, ISO_639_1);

            foreach (Language lang in languages)
                languageCodes.Add(lang.ISO_639_2);

            this.populator = new PlaylistDataGridPopulator(playlistDataGridView, this.playlists, languageCodes);
            this.populator.SelectionChanged += dataGridView_SelectionChanged;
            this.populator.MainLanguageCode = ISO_639_2;

            this.Load += FormDetails_Load;
        }

        ~FormDetails()
        {
            CancelRip();
        }

        private void FormDetails_Load(object sender, EventArgs e)
        {
            this.movieNameTextBox.Text = String.IsNullOrEmpty(BDROM.DiscNameSearchable) ? BDROM.VolumeLabel : BDROM.DiscNameSearchable;
            this.discLanguageComboBox.DataSource = new List<Language>(languages).ToArray();

            this.textBoxOutputDir.Text = settings.OutputDir;
            this.textBoxOutputFileName.Text = settings.OutputFileName;

            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);

            PopulateOutputTab();

            populator.ItemChanged += OnPlaylistItemChange;
            comboBoxAudienceLanguage.SelectedIndexChanged += OnAudienceLanguageChange;
            playlistDataGridView.CurrentCellDirtyStateChanged += playlistDataGridView_CurrentCellDirtyStateChanged;

            listViewStreamFiles.Enabled = true;
            listViewStreams.Enabled = true;

            ResetPlaylistDataGrid();
            QueryMainMovie();
        }

        #endregion

        #region "Disc" Tab

        private bool DiscTabControlsEnabled
        {
            set
            {
                discLanguageComboBox.Enabled = value;
                movieNameTextBox.Enabled = value;
                maskedTextBoxYear.Enabled = value;
                searchButton.Enabled = value;
                searchResultListView.Enabled = value;
            }
        }

        private void ResizeDiscTab(object sender = null, EventArgs e = null)
        {
            var width = searchResultListView.ClientSize.Width;
            var columns = searchResultListView.Columns;
            columns[0].Width = (int)(width - columns[1].Width - columns[2].Width);
        }

        #endregion

        #region "Playlists" Tab

        private void ResetPlaylistDataGrid()
        {
            populator.ShowAllPlaylists = showAllPlaylistsCheckbox.Checked;
        }

        private void ResizePlaylistsTab(object sender = null, EventArgs e = null)
        {
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

        #endregion

        #region "Output" Tab

        private void PopulateOutputTab()
        {
            ignoreFilterControlChange = true;

            comboBoxAudienceLanguage.DataSource = null;
            comboBoxAudienceLanguage.DataSource = new List<Language>(languages).ToArray();
            comboBoxAudienceLanguage.Enabled = languages.Count > 1;

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

            ignoreFilterControlChange = false;

            FilterPlaylists();
        }

        private void FilterPlaylists()
        {
            if (ignoreFilterControlChange) return;

            filteredPlaylists = new HashSet<TSPlaylistFile>(playlists);

            Language videoLanguage = comboBoxVideoLanguage.SelectedValue as Language;
            Cut cut = (Cut)comboBoxCut.SelectedValue;
            CommentaryOption commentaryOption = (CommentaryOption)comboBoxCommentary.SelectedValue;
            audioLanguages.Clear();
            subtitleLanguages.Clear();

            foreach (Object o in listBoxAudioLanguages.SelectedItems)
                if (o is Language) audioLanguages.Add(o as Language);

            foreach (Object o in listBoxSubtitleLanguages.SelectedItems)
                if (o is Language) subtitleLanguages.Add(o as Language);

            if (videoLanguage == null) return;

            ISet<TSPlaylistFile> playlistsWithMainMovie = populator.GetPlaylistsWithMainMovie(true);
            ISet<TSPlaylistFile> playlistsWithVideoLanguage = populator.GetPlaylistsWithVideoLanguage(videoLanguage);
            ISet<TSPlaylistFile> playlistsWithCut = populator.GetPlaylistsWithCut(cut);
            ISet<TSPlaylistFile> playlistsWithCommentaryOption = populator.GetPlaylistsWithCommentaryOption(commentaryOption);
            ISet<TSPlaylistFile> playlistsWithAudioLanguages = populator.GetPlaylistsWithAudioLanguages(audioLanguages);
            ISet<TSPlaylistFile> playlistsWithSubtitleLanguages = populator.GetPlaylistsWithSubtitleLanguages(subtitleLanguages);

            filteredPlaylists.IntersectWith(playlistsWithMainMovie);
            filteredPlaylists.IntersectWith(playlistsWithVideoLanguage);
            filteredPlaylists.IntersectWith(playlistsWithCut);
            filteredPlaylists.IntersectWith(playlistsWithCommentaryOption);
            filteredPlaylists.IntersectWith(playlistsWithAudioLanguages);
            filteredPlaylists.IntersectWith(playlistsWithSubtitleLanguages);

            comboBoxPlaylist.DataSource = null;
            comboBoxPlaylist.DataSource = new List<TSPlaylistFile>(filteredPlaylists).ToArray();
            comboBoxPlaylist.Enabled = filteredPlaylists.Count > 1;
        }

        private void FilterTracks()
        {
            listViewVideoTracks.Items.Clear();
            listViewAudioTracks.Items.Clear();
            listViewSubtitleTracks.Items.Clear();

            videoTracks.Clear();
            audioTracks.Clear();
            subtitleTracks.Clear();

            if (filteredPlaylists == null || filteredPlaylists.Count == 0)
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
                    videoTracks.Add(stream as TSVideoStream);
                else if (stream is TSAudioStream && audioLanguages.Contains(lang))
                    audioTracks.Add(stream as TSAudioStream);
                else if (stream is TSGraphicsStream && subtitleLanguages.Contains(lang))
                    subtitleTracks.Add(stream);
                else if (stream is TSTextStream && subtitleLanguages.Contains(lang))
                    subtitleTracks.Add(stream);
            }

            PopulateVideoTracks();
            PopulateAudioTracks();
            PopulateSubtitleTracks();

            ResizeOutputTab();
            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);
        }

        private void PopulateVideoTracks()
        {
            foreach (TSVideoStream stream in videoTracks)
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
                    new ListViewItem.ListViewSubItem[]
                    {
                        codec,
                        resolution,
                        frameRate,
                        aspectRatio
                    };

                ListViewItem streamItem = new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream;
                streamItem.Checked = true;
                listViewVideoTracks.Items.Add(streamItem);
            }
        }

        private void PopulateAudioTracks()
        {
            foreach (TSAudioStream stream in audioTracks)
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
                    new ListViewItem.ListViewSubItem[]
                    {
                        codec,
                        language,
                        channels
                    };

                ListViewItem streamItem = new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream;
                streamItem.Checked = true;
                listViewAudioTracks.Items.Add(streamItem);
            }
        }

        private void PopulateSubtitleTracks()
        {
            foreach (TSStream stream in subtitleTracks)
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
                    new ListViewItem.ListViewSubItem[]
                    {
                        codec,
                        language
                    };

                ListViewItem streamItem = new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream;
                streamItem.Checked = true;
                listViewSubtitleTracks.Items.Add(streamItem);
            }
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
            DiscTabControlsEnabled = false;

            this.auto_configured = false;
            this.auto_tmdb_id = -1;

            mainMovieBackgroundWorker = new BackgroundWorker();
            mainMovieBackgroundWorker.WorkerReportsProgress = true;
            mainMovieBackgroundWorker.WorkerSupportsCancellation = true;
            mainMovieBackgroundWorker.DoWork += mainMovieBackgroundWorker_DoWork;
            mainMovieBackgroundWorker.ProgressChanged += mainMovieBackgroundWorker_ProgressChanged;
            mainMovieBackgroundWorker.RunWorkerCompleted += mainMovieBackgroundWorker_RunWorkerCompleted;
            mainMovieBackgroundWorker.RunWorkerAsync();
        }

        private void SearchTmdb()
        {
            if (String.IsNullOrEmpty(this.movieNameTextBox.Text))
            {
                this.discLanguageComboBox.Enabled = true;
                this.maskedTextBoxYear.Enabled = true;
                this.searchButton.Enabled = true;
                return;
            }

            searchResultListView.Items.Clear();

            DiscTabControlsEnabled = false;

            string query = this.movieNameTextBox.Text;
            string ISO_639_1 = (this.discLanguageComboBox.SelectedValue as Language).ISO_639_1;
            int? year = Regex.IsMatch(maskedTextBoxYear.Text, @"^\d{4}$") ? (int?)Int32.Parse(maskedTextBoxYear.Text) : null;
            
            TmdbSearchRequestParams reqParams = new TmdbSearchRequestParams(query, year, ISO_639_1);

            tmdbBackgroundWorker = new BackgroundWorker();
            tmdbBackgroundWorker.WorkerReportsProgress = true;
            tmdbBackgroundWorker.WorkerSupportsCancellation = true;
            tmdbBackgroundWorker.DoWork += tmdbBackgroundWorker_DoWork;
            tmdbBackgroundWorker.ProgressChanged += tmdbBackgroundWorker_ProgressChanged;
            tmdbBackgroundWorker.RunWorkerCompleted += tmdbBackgroundWorker_RunWorkerCompleted;
            tmdbBackgroundWorker.RunWorkerAsync(reqParams);
        }

        private class TmdbSearchRequestParams
        {
            public string query;
            public int? year;
            public string ISO_639_1;
            public TmdbSearchRequestParams(string query, int? year, string ISO_639_1)
            {
                this.query = query;
                this.year = year;
                this.ISO_639_1 = ISO_639_1;
            }
        }

        #endregion

        #region Main Movie Background Worker

        private void mainMovieBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IList<TSPlaylistFile> mainPlaylists = new List<TSPlaylistFile>();
                foreach (TSPlaylistFile playlist in playlists)
                {
                    if (playlist.IsMainPlaylist)
                        mainPlaylists.Add(playlist);
                }
                mainMovieSearchResult = mainMovieService.FindMainMovie(BDROM.VolumeLabel, mainPlaylists);
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
            if (e.Result is Exception)
            {
                string msg = ((Exception)e.Result).Message;

                MessageBox.Show(msg, "BDInfo Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // TODO: Disable POST submission only - don't disable ripping completely
                DiscTabControlsEnabled = true;
                searchResultListView.Enabled = false;
            }

            if (mainMovieSearchResult == null) return;

            if (mainMovieSearchResult.error)
            {
                string errorMessages = "";
                foreach (JsonSearchResultError error in mainMovieSearchResult.errors)
                {
                    errorMessages += error.textStatus + " - " + error.errorMessage + "\n";
                }
                MessageBox.Show(this, "Main movie service returned the following error(s): \n\n" + errorMessages, "Error - main movie service", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (mainMovieSearchResult.discs.Count == 0)
            {
                MessageBox.Show(this, "No matching discs were found in the database.\n\n" + "Please submit one!", "No results found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(this, "Hooray!  Found " + mainMovieSearchResult.discs.Count + " matching discs in the database.", mainMovieSearchResult.discs.Count + " result(s) found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                JsonDisc disc = mainMovieSearchResult.discs[0];

                this.auto_configured = true;
                this.auto_tmdb_id = disc.tmdb_id;
                //this.movieNameTextBox.Text = disc.movie_title + " (" + disc.year + ")";
                this.movieNameTextBox.Text = disc.movie_title;
                this.maskedTextBoxYear.Text = disc.year != null ? disc.year.ToString() : null;

                populator.AutoConfigure(disc.playlists);
            }

            SearchTmdb();
        }

        #endregion

        #region TMDb Background Worker

        private void tmdbBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                TmdbSearchRequestParams reqParams = e.Argument as TmdbSearchRequestParams;
                tmdbMovieSearch = tmdb_api.SearchMovie(reqParams.query, 1, reqParams.ISO_639_1, false, reqParams.year);
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

        private string movieTitle = null;
        private int? movieYear = null;

        private string GetYearString(string tmdb_date)
        {
            return String.IsNullOrEmpty(tmdb_date) ? null : Regex.Replace(tmdb_date, @"^(\d{4})-(\d{1,2})-(\d{1,2})$", @"$1", RegexOptions.IgnoreCase);
        }

        private int? GetYearInt(string tmdb_date)
        {
            string yearString = GetYearString(tmdb_date);
            return String.IsNullOrEmpty(yearString) ? (int?)null : Convert.ToInt32(yearString);
        }

        private void tmdbBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DiscTabControlsEnabled = true;

            if (tmdbMovieSearch == null || tmdbMovieSearch.results == null)
                return;

            foreach (MovieResult curResult in tmdbMovieSearch.results)
            {
                ListViewItem.ListViewSubItem movieNameSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem movieYearSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem moviePopularitySubItem = new ListViewItem.ListViewSubItem();

                movieNameSubItem.Text = curResult.title;
                movieYearSubItem.Text = GetYearString(curResult.release_date);
                moviePopularitySubItem.Text = curResult.popularity.ToString("N3", CultureInfo.CurrentUICulture);

                ListViewItem.ListViewSubItem[] searchResultSubItems =
                    new ListViewItem.ListViewSubItem[]
                        {
                            movieNameSubItem,
                            movieYearSubItem,
                            moviePopularitySubItem
                        };

                ListViewItem searchResultListItem =
                    new ListViewItem(searchResultSubItems, 0);

                searchResultListView.Items.Add(searchResultListItem);
            }

            searchResultListView.Select();

            if (tmdbMovieSearch.results.Count > 0)
            {
                searchResultListView.Items[0].Selected = true;
            }

            ResizeDiscTab();
        }

        #endregion

        #region Ripping

        private void Rip()
        {
            if (SelectedPlaylist != null)
            {
                try
                {
                    Directory.CreateDirectory(textBoxOutputDir.Text);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(
                        "BDInfo Error",
                        ex.Message
                    );
                    return;
                }

                string outputPath = Path.Combine(textBoxOutputDir.Text, labelOutputFileNamePreview.Text);

                /*
                if (!FileUtils.IsFileWritableRecursive(outputPath))
                {
                    ShowErrorMessage(
                        "File is not writable",
                        "File \"" + outputPath + "\" is not writable!"
                    );
                    return;
                }
                */
                
                ulong minFreeSpace = (ulong)(SelectedPlaylist.FileSize * 2.5);

                // TODO: Remove "false"
                if (false && FileUtils.GetFreeSpace(textBoxOutputDir.Text) < minFreeSpace)
                {
                    ShowErrorMessage(
                        "Not enough free space",
                        "At least " + FileUtils.FormatFileSize(textBoxOutputDir.Text) + " (" + minFreeSpace + " bytes) of free space is required."
                    );
                    return;
                }

                ISet<TSStream> selectedStreams = new HashSet<TSStream>();
                selectedStreams.UnionWith(SelectedVideoStreams);
                selectedStreams.UnionWith(SelectedAudioStreams);
                selectedStreams.UnionWith(SelectedSubtitleStreams);

                settings.OutputDir = textBoxOutputDir.Text;
                settings.OutputFileName = textBoxOutputFileName.Text;

                tsMuxer = new TsMuxer(BDROM, SelectedPlaylist, selectedStreams);
                tsMuxer.WorkerReportsProgress = true;
                tsMuxer.WorkerSupportsCancellation = true;
                //tsMuxer.DoWork += tsMuxerBackgroundWorker_DoWork;
                tsMuxer.ProgressChanged += tsMuxerBackgroundWorker_ProgressChanged;
                tsMuxer.RunWorkerCompleted += tsMuxerBackgroundWorker_RunWorkerCompleted;
                tsMuxer.RunWorkerAsync(outputPath);
            }
        }

        private void CancelRip()
        {
            if (tsMuxer != null && tsMuxer.IsBusy)
            {
                tsMuxer.CancelAsync();
            }
        }

        private void tsMuxerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void tsMuxerBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            isMuxing = true;
            progressBarTsMuxer.Value = e.ProgressPercentage;
            labelTsMuxerProgress.Text = tsMuxer.Progress.ToString("##0.0") + "%";
            textBoxTsMuxerCommandLine.Text = tsMuxer.CommandLine;
        }

        private void tsMuxerBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isMuxing = false;
            if ((e.Cancelled == true))
            {
                continueButton.Text = "Canceled!";
            }
            else if (!(e.Error == null))
            {
                continueButton.Text = "Error!";
                ShowErrorMessage(e.Error.Message, "tsMuxeR Error");
            }
            else
            {
                continueButton.Text = "Done!";
                MessageBox.Show(this, "tsMuxeR Completed!", "Finished ripping!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Submit JSON to DB

        private bool CanSubmitToDB
        {
            get
            {
                if (searchResultListView.SelectedIndices.Count > 0)
                {
                    if (auto_configured)
                    {
                        if (tmdbMovieResult.id != auto_tmdb_id || populator.HasChanged)
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
            if (CanSubmitToDB)
            {
                DialogResult answer = MessageBox.Show(this, "Submit a new disc to the database?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    SubmitJsonDisc();
                }
            }
        }

        private void SubmitJsonDisc()
        {
            if (searchResultListView.SelectedItems.Count == 0) return;

            DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to submit to the database?", "Confirm database submit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No) return;

            JsonDisc jsonDisc = new JsonDisc();

            jsonDisc.disc_name = BDROM.DiscName;
            jsonDisc.volume_label = BDROM.VolumeLabel;
            jsonDisc.ISO_639_2 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_2 : null;

            jsonDisc.tmdb_id = tmdbMovieResult != null ? tmdbMovieResult.id : -1;
            jsonDisc.movie_title = movieTitle;
            jsonDisc.year = movieYear;

            jsonDisc.playlists = new List<JsonPlaylist>();

            foreach (JsonPlaylist jsonPlaylist in populator.JsonPlaylists)
            {
                jsonDisc.playlists.Add(jsonPlaylist);
            }

            try
            {
                mainMovieService.PostDisc(jsonDisc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "ERROR: \n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(this, "Awesome!  Successfully added disc to database.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return;

            //string jsonString = JsonConvert.SerializeObject(jsonDisc);
            //Clipboard.SetText(jsonString);
            //MessageBox.Show("Copied to clipboard: \n\n" + jsonString);
        }

        #endregion

        // TODO: Organize Event Handlers into separate regions for each tab
        #region Event Handlers

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylistDataGrid();
        }

        private void listViewStreamFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewStreamFiles.SelectedItems.Count == 0) return;

            string filename = listViewStreamFiles.SelectedItems[0].Text;
            string filepath = System.IO.Path.Combine(BDROM.DirectorySTREAM.FullName, filename);

            System.Diagnostics.Process.Start(filepath);
        }

        private void discLanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            populator.MainLanguageCode = (discLanguageComboBox.SelectedValue as Language).ISO_639_2;
        }

        private void maskedTextBoxYear_TextChanged(object sender, EventArgs e)
        {
            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void searchResultListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tmdbMovieResult = null;
            this.movieTitle = null;
            this.movieYear = null;

            if (this.tmdbMovieSearch != null && searchResultListView.SelectedIndices.Count > 0)
            {
                int index = searchResultListView.SelectedIndices[0];
                this.tmdbMovieResult = this.tmdbMovieSearch.results[index];

                if (tmdbMovieResult != null)
                {
                    this.movieTitle = this.tmdbMovieResult.title;
                    this.movieYear = GetYearInt(this.tmdbMovieResult.release_date);
                }
            }

            this.buttonSubmitToDB.Enabled = CanSubmitToDB;

            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SearchTmdb();
        }

        private void OnPlaylistItemChange(object sender, EventArgs e)
        {
            if (!ignoreDataGridItemChange)
            {
                PopulateOutputTab();
                buttonSubmitToDB.Enabled = CanSubmitToDB;
            }
        }

        private void OnAudienceLanguageChange(object sender, EventArgs e)
        {
            SetAudienceLanguage(VideoLanguages, comboBoxVideoLanguage);
            SetAudienceLanguage(AudioLanguages, listBoxAudioLanguages);
            SetAudienceLanguage(SubtitleLanguages, listBoxSubtitleLanguages);
        }

        private void SetAudienceLanguage(Language[] array, ListControl control)
        {
            Language audienceLanguage = comboBoxAudienceLanguage.SelectedValue as Language;
            IList<Language> list = new List<Language>(array);
            if (list.Contains(audienceLanguage))
            {
                try
                {
                    control.SelectedIndex = -1;
                    control.SelectedIndex = list.IndexOf(audienceLanguage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "BDInfo Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            TSPlaylistFile playlistFile = populator.SelectedPlaylist;

            if (playlistFile == null) return;

            string playlistFileName = playlistFile.Name;

            StreamTrackListViewPopulator.Populate(playlistFile, listViewStreamFiles, listViewStreams);
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageDisc)
            {
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (tabControl.SelectedTab == tabPagePlaylists)
            {
                SubmitJsonDiscIfNecessary();
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (tabControl.SelectedTab == tabPageOutput)
            {
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
                Rip();
            }
            else if (tabControl.SelectedTab == tabPageProgress)
            {
                CancelRip();
            }
        }

        private void checkBoxReplaceSpaces_CheckedChanged(object sender, EventArgs e)
        {
            textBoxReplaceSpaces.Enabled = checkBoxReplaceSpaces.Checked;
            if (checkBoxReplaceSpaces.Checked)
            {
                textBoxReplaceSpaces.Focus();
                textBoxReplaceSpaces.SelectAll();
            }
            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void textBoxOutputFileName_TextChanged(object sender, EventArgs e)
        {
            string preview = textBoxOutputFileName.Text;

            string title = String.IsNullOrEmpty(this.movieTitle) ? movieNameTextBox.Text : this.movieTitle;
            string year = this.movieYear == null ? GetYearString(maskedTextBoxYear.Text) : this.movieYear + "";
            string res = "";
            string vcodec = "";
            string acodec = "";
            string channels = "";

            if (string.IsNullOrEmpty(year))
                year = "";

            if (listViewVideoTracks.CheckedItems.Count > 0)
            {
                TSVideoStream videoStream = listViewVideoTracks.CheckedItems[0].Tag as TSVideoStream;
                res = videoStream.HeightDescription;
                vcodec = videoStream.CodecShortName;
            }

            if (listViewAudioTracks.CheckedItems.Count > 0)
            {
                TSAudioStream audioStream = listViewAudioTracks.CheckedItems[0].Tag as TSAudioStream;
                acodec = audioStream.CodecShortName;
                channels = audioStream.ChannelCountDescription;
            }

            preview = Regex.Replace(preview, @"%title%", title, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%year%", year, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%res%", res, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%vcodec%", vcodec, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%acodec%", acodec, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%channels%", channels, RegexOptions.IgnoreCase);

            if (checkBoxReplaceSpaces.Checked)
            {
                preview = preview.Replace(" ", textBoxReplaceSpaces.Text);
            }

            labelOutputFileNamePreview.Text = preview + labelOutputFileExtension.Text;
        }

        private void textBoxReplaceSpaces_TextChanged(object sender, EventArgs e)
        {
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
            continueButton.Text = "Continue";
            continueButton.Enabled = true;

            if (tabControl.SelectedTab == tabPageOutput)
            {
                if (!isMuxing)
                    continueButton.Text = "Rip It!";
            }
            else if (tabControl.SelectedTab == tabPageProgress)
            {
                continueButton.Enabled = false;
            }
        }

        private void buttonSubmitToDB_Click(object sender, EventArgs e)
        {
            SubmitJsonDisc();
        }

        private void playlistDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (playlistDataGridView.IsCurrentCellDirty)
            {
                playlistDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            ignoreDataGridItemChange = true;
            populator.SelectAll = true;
            ignoreDataGridItemChange = false;
            OnPlaylistItemChange(sender, e);
        }

        private void buttonUnselectAll_Click(object sender, EventArgs e)
        {
            ignoreDataGridItemChange = true;
            populator.SelectAll = false;
            ignoreDataGridItemChange = false;
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
                if (!string.IsNullOrEmpty(textBoxOutputDir.Text))
                {
                    dialog.SelectedPath = textBoxOutputDir.Text;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.SelectedPath;
                    textBoxOutputDir.Text = path;

                    // TODO: Validate that the selected directory has enough free space for 2x-3x the playlist size
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Error opening path {0}: {1}{2}",
                    path,
                    ex.Message,
                    Environment.NewLine);

                MessageBox.Show(msg, "BDInfo Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxOutputDir_DragEnter(object sender, DragEventArgs e)
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

        private void textBoxOutputDir_DragDrop(object sender, DragEventArgs e)
        {
            string path = DragUtils.GetFirstPath(e);
            if (path != null)
            {
                if (FileUtils.IsDirectory(path))
                    textBoxOutputDir.Text = path;
                else
                    textBoxOutputDir.Text = Path.GetDirectoryName(path);

                // TODO: Validate that the selected directory has enough free space for 2x-3x the playlist size
            }
        }

        private void textBoxOutputFileName_DragEnter(object sender, DragEventArgs e)
        {
            if (DragUtils.HasFile(e) && !String.IsNullOrEmpty(DragUtils.GetFirstFileNameWithoutExtension(e)))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBoxOutputFileName_DragDrop(object sender, DragEventArgs e)
        {
            string filenameWithoutExtension = DragUtils.GetFirstFileNameWithoutExtension(e);
            if (!String.IsNullOrEmpty(filenameWithoutExtension))
                textBoxOutputFileName.Text = filenameWithoutExtension;
        }

        private void FormDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isMuxing)
            {
                if (MessageBox.Show(this, "Abort muxing?", "Abort muxing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    CancelRip();
                }
            }
        }

        #endregion

        private void ShowErrorMessage(string caption, string text)
        {
            MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
