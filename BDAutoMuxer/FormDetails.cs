using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.tools;
using BDAutoMuxer.views;
using Microsoft.WindowsAPICodePack.Taskbar;
using WatTmdb.V3;

namespace BDAutoMuxer
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

        private int currentMouseOverRow = -1;

        private bool auto_configured = false;
        private int auto_tmdb_id = -1;

        private bool ignoreFilterControlChange = false;
        private bool ignoreDataGridItemChange = false;

        private TsMuxer tsMuxer;
        private bool isMuxing = false;
        private string tsMuxerOutputPath = null;

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

                EnableTabPage(tabPageDisc, !isMuxing);
                EnableTabPage(tabPageOutput, !isMuxing);

                if (isMuxing)
                {
                    cancelButton.Text = "Stop";
                }
                else
                {
                    cancelButton.Text = "Close";
                }

                ResetButtons();
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
            this.populator.SelectionChanged += playlistDataGridView_SelectionChanged;
            this.populator.MainLanguageCode = ISO_639_2;

            FormUtils.TextBox_EnableSelectAll(this);

            this.Load += FormDetails_Load;
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

            this.statusLabel.Text = "";

            this.movieNameTextBox.Text = String.IsNullOrEmpty(BDROM.DiscNameSearchable) ? BDROM.VolumeLabel : BDROM.DiscNameSearchable;
            this.discLanguageComboBox.DataSource = new List<Language>(languages).ToArray();

            this.textBoxOutputDir.Text = BDAutoMuxerSettings.OutputDir;
            this.textBoxOutputFileName.Text = BDAutoMuxerSettings.OutputFileName;

            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);

            PopulateOutputTab();

            populator.ItemChanged += OnPlaylistItemChange;
            comboBoxAudienceLanguage.SelectedIndexChanged += OnAudienceLanguageChange;
            playlistDataGridView.CurrentCellDirtyStateChanged += playlistDataGridView_CurrentCellDirtyStateChanged;

            listViewStreamFiles.Enabled = true;
            listViewStreams.Enabled = true;

            InitHints(this);

            ResetPlaylistDataGrid();
            QueryMainMovie();

            textBoxOutputFileNameHint.Parent.BackColorChanged += (o, args) => UpdateBackgroundColors();
            textBoxOutputFileNamePreview.Parent.BackColorChanged += (o, args) => UpdateBackgroundColors();
            textBoxOutputDirPreview.Parent.BackColorChanged += (o, args) => UpdateBackgroundColors();
            UpdateBackgroundColors();
        }

        private void UpdateBackgroundColors()
        {
            try
            {
                textBoxOutputFileNameHint.BackColor = textBoxOutputFileNameHint.Parent.BackColor;
                textBoxOutputFileNamePreview.BackColor = textBoxOutputFileNamePreview.Parent.BackColor;
                textBoxOutputDirPreview.BackColor = textBoxOutputDirPreview.Parent.BackColor;
            }
            catch
            {
            }
        }

        #endregion

        #region Tab Helpers

        public static void EnableTabPage(TabPage page, bool enable)
        {
            EnableControls(page.Controls, enable);
        }

        private static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Enabled = enable;
                EnableControls(ctl.Controls, enable);
            }
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
            ResizeDiscTab();

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

            SetTabStatus(tabPageDisc, "Querying main movie database...");

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

            SetTabStatus(tabPageDisc, "Searching The Movie Database (TMDb)...");

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
            string errorCaption = "Error querying main movie DB";
            
            if (e.Result is Exception)
            {
                DiscTabControlsEnabled = true;
                searchResultListView.Enabled = false;
                ShowErrorMessage(tabPageDisc, errorCaption, ((Exception)e.Result).Message);
            }

            if (mainMovieSearchResult == null) return;

            if (mainMovieSearchResult.error)
            {
                string errorMessages = "";
                foreach (JsonSearchResultError error in mainMovieSearchResult.errors)
                {
                    errorMessages += error.textStatus + " - " + error.errorMessage + "\n";
                }
                string errorMessage = "Main movie service returned the following error(s): \n\n" + errorMessages;
                ShowErrorMessage(tabPageDisc, errorCaption, errorMessage);
            }
            else if (mainMovieSearchResult.discs.Count == 0)
            {
                ShowExclamationMessage(tabPageDisc, "No results found", "No matching discs were found in the database.\n\n" + "Please submit one!");
            }
            else
            {
                JsonDisc disc = mainMovieSearchResult.discs[0];

                this.auto_configured = true;
                this.auto_tmdb_id = disc.tmdb_id;
                this.movieNameTextBox.Text = disc.movie_title;
                this.maskedTextBoxYear.Text = disc.year != null ? disc.year.ToString() : null;

                populator.AutoConfigure(disc.playlists);

                int count = mainMovieSearchResult.discs.Count;
                string plural = count != 1 ? "s" : "";
                string caption = string.Format("{0:d} result{1:s} found", count, plural);
                string message = string.Format("Hooray!  Found {0:d} matching disc{1:s} in the database.", count, plural);
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
            searchResultListView.Enabled = false;

            string errorCaption = "Error searching The Movie Database (TMDb)";
            
            if (e.Result is Exception)
            {
                ShowErrorMessage(tabPageDisc, errorCaption, ((Exception)e.Result).Message);
            }

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

            if (tmdbMovieSearch.results.Count > 0)
            {
                int count = tmdbMovieSearch.results.Count;
                string plural = count != 1 ? "s" : "";
                SetTabStatus(tabPageDisc, string.Format("{0:d} result{1:s} found", count, plural));

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
            if (IsMuxing) return;

            if (SelectedPlaylist != null)
            {
                try
                {
                    Directory.CreateDirectory(textBoxOutputDirPreview.Text);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(tabPageProgress, "Unable to create directory", ex.Message);
                    return;
                }

                tsMuxerOutputPath = Path.Combine(textBoxOutputDirPreview.Text, textBoxOutputFileNamePreview.Text);

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
                BDAutoMuxerSettings.SaveSettings();

                // TODO: Make this status "sticky"
                SetTabStatus(tabPageProgress, "tsMuxeR: 0.0%");
                textBoxTsMuxerCommandLine.Text = "";

                tsMuxer = new TsMuxer(BDROM, SelectedPlaylist, selectedStreams);
                tsMuxer.WorkerReportsProgress = true;
                tsMuxer.WorkerSupportsCancellation = true;
                //tsMuxer.DoWork += tsMuxerBackgroundWorker_DoWork;
                tsMuxer.ProgressChanged += tsMuxerBackgroundWorker_ProgressChanged;
                tsMuxer.RunWorkerCompleted += tsMuxerBackgroundWorker_RunWorkerCompleted;

                tsMuxer.RunWorkerAsync(tsMuxerOutputPath);
            }
        }

        private void CancelRip()
        {
            if (tsMuxer != null && tsMuxer.IsBusy)
            {
                tsMuxer.Resume();
                tsMuxer.CancelAsync();
            }
        }

        private void tsMuxerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void tsMuxerBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            IsMuxing = true;
            UpdateTsMuxerProgress(sender, e);
            ResetButtons();
        }

        private void UpdateTsMuxerProgress(object sender = null, EventArgs e = null)
        {
            // To show fractional progress, progress bar range is 0 to 1000 (instead of 0 to 100)
            progressBarTsMuxer.Value = (int)(tsMuxer.Progress * 10);

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.Normal);
            TaskbarProgress.SetProgressValue(progressBarTsMuxer.Value, 1000);

            string strProgress = tsMuxer.Progress.ToString("##0.0") + "%";
            labelTsMuxerProgress.Text = strProgress;

            // TODO: Make this status "sticky"
            SetTabStatus(tabPageProgress, "tsMuxeR: " + strProgress);

            if (String.IsNullOrEmpty(textBoxTsMuxerCommandLine.Text))
                textBoxTsMuxerCommandLine.Text = tsMuxer.CommandLine;

            labelTsMuxerTimeRemaining.Text = GetElapsedTimeString(tsMuxer.TimeRemaining);
            labelTsMuxerTimeElapsed.Text = GetElapsedTimeString(tsMuxer.TimeElapsed);

            // TODO: Refactor this logic into a separate method
            if (!String.IsNullOrEmpty(tsMuxer.State))
            {
                labelTsMuxerProgress.Text += String.Format(" ({0})", tsMuxer.State);
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
            IsMuxing = false;

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.NoProgress);

            if (e.Cancelled == true && tsMuxer.IsCanceled)
            {
                SetTabStatus(tabPageProgress, "tsMuxeR canceled");
            }
            else if (e.Error != null)
            {
                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error);
                ShowErrorMessage(tabPageProgress, "tsMuxeR Error", e.Error.Message);
            }
            else
            {
                ShowMessage(tabPageProgress, "tsMuxeR completed!", "Finished muxing M2TS with tsMuxeR!");
            }

            ResetButtons();
        }

        #endregion

        #region Submit JSON to DB

        private bool CanSubmitToDB
        {
            get
            {
                if (!String.IsNullOrEmpty(BDAutoMuxerSettings.ApiKey) && searchResultListView.SelectedIndices.Count > 0)
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
                JsonSearchResult postResult = mainMovieService.PostDisc(jsonDisc);
                if (postResult.error)
                {
                    if (postResult.errors.Count > 0)
                        ShowErrorMessage("DB POST Error", postResult.errors[0].textStatus + ": " + postResult.errors[0].errorMessage);
                    else
                        ShowErrorMessage("DB POST Error", "Unknown error occurred while POSTing to the DB");
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

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylistDataGrid();
        }

        private void listViewStreamFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewStreamFiles.SelectedItems.Count == 0) return;

            string filename = listViewStreamFiles.SelectedItems[0].Text;
            string filepath = System.IO.Path.Combine(BDROM.DirectorySTREAM.FullName, filename);

            playFile(filepath);
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
                    MessageBox.Show(this, ex.Message, "BDAutoMuxer Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void playlistDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            TSPlaylistFile playlistFile = populator.SelectedPlaylist;

            if (playlistFile == null) return;

            string playlistFileName = playlistFile.Name;

            StreamTrackListViewPopulator.Populate(playlistFile, listViewStreamFiles, listViewStreams);
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
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = playlistDataGridView.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    TSPlaylistFile playlist = populator.PlaylistAt(currentMouseOverRow);
                    showPlayableFileContextMenu(playlistDataGridView, playlist.FullName, e.X, e.Y);
                }
            }
        }

        private void listViewStreamFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = listViewStreamFiles.HitTest(e.X, e.Y).Item;
                int currentMouseOverRow = item.Index;

                if (currentMouseOverRow >= 0)
                {
                    if (item.Tag is TSStreamClip)
                    {
                        TSStreamClip clip = item.Tag as TSStreamClip;

                        string filename = clip.DisplayName;
                        string filepath = Path.Combine(BDROM.DirectorySTREAM.FullName, filename);

                        showPlayableFileContextMenu(listViewStreamFiles, filepath, e.X, e.Y);
                    }
                }
            }
        }

        private void showPlayableFileContextMenu(Control control, string filePath, int x, int y)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItemOpen = new MenuItem(string.Format("Open \"{0}\"", Path.GetFileName(filePath)));
            menuItemOpen.DefaultItem = true;
            menuItemOpen.Click += (object s1, EventArgs e1) => { playFile(filePath); };
            menuItemOpen.Enabled = FileUtils.HasProgramAssociation(filePath);

            MenuItem menuItemShow = new MenuItem("Show in folder");
            menuItemShow.Click += (object s1, EventArgs e1) => { showInFolder(filePath); };

            contextMenu.MenuItems.Add(menuItemOpen);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(menuItemShow);

            contextMenu.Show(control, new Point(x, y));
        }

        private void playFile(string filePath)
        {
            FileUtils.OpenFile(filePath);
        }

        private void showInFolder(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            // combine the arguments together
            // it doesn't matter if there is a space after ','
            string argument = "/select, \"" + filePath + "\"";

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void playlistDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            currentMouseOverRow = e.RowIndex;
        }

        private void playlistDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            currentMouseOverRow = -1;
        }

        private void ResetButtons()
        {
            continueButton.Text = "Continue";
            continueButton.Enabled = true;

            if (tabControl.SelectedTab == tabPageOutput)
            {
                if (!IsMuxing)
                    continueButton.Text = "Rip It!";
            }
            else if (tabControl.SelectedTab == tabPageProgress)
            {
                if (IsMuxing)
                {
                    if (tsMuxer.IsPaused)
                        continueButton.Text = "Resume";
                    else
                        continueButton.Text = "Pause";
                }
                else
                    continueButton.Enabled = false;
            }
        }

        private string OutputFilePath
        {
            get { return Path.Combine(textBoxOutputDirPreview.Text, textBoxOutputFileNamePreview.Text); }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageDisc)
            {
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (tabControl.SelectedTab == tabPageDisc)
            {
                SubmitJsonDiscIfNecessary();
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (tabControl.SelectedTab == tabPageOutput)
            {
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
                Rip();
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (tabControl.SelectedTab == tabPageProgress)
            {
                if (tsMuxer != null)
                {
                    // TODO: Refactor label text logic into a separate method
                    if (tsMuxer.IsPaused)
                    {
                        tsMuxer.Resume();
                        labelTsMuxerProgress.Text = labelTsMuxerProgress.Text.Replace(" (paused}", "");
                    }
                    else
                    {
                        tsMuxer.Pause();
                        labelTsMuxerProgress.Text += " (paused}";
                    }
                }
            }

            ResetButtons();
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
            string preview = text;

            string volume = BDROM.VolumeLabel;
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

            TextBox_KeyPress(sender, e);
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
            ResetButtons();
            RestoreTabStatus();
        }

        private void buttonSubmitToDB_Click(object sender, EventArgs e)
        {
            SubmitJsonDisc();
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
                string dir = textBoxOutputDirPreview.Text;
                if (!string.IsNullOrEmpty(dir))
                {
                    dialog.SelectedPath = dir;
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

                MessageBox.Show(msg, "BDAutoMuxer Error",
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

        /// <see cref="http://www.dzone.com/snippets/ctrl-shortcut-select-all-text"/>
        private void TextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancelButtonHandled = false;
            if (IsMuxing && MessageBox.Show(this, "Abort muxing?", "Abort muxing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                CancelRip();
                cancelButtonHandled = true;
            }
        }

        private void FormDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancelButtonHandled)
            {
                e.Cancel = true;
            }
            else if (IsMuxing)
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
            }
            cancelButtonHandled = false;
        }

        private bool cancelButtonHandled = false;

        #endregion

        #region User Messages

        private Dictionary<TabPage, string> tabStatusMessages = new Dictionary<TabPage, string>();
        private Dictionary<Control, String> controlHints = new Dictionary<Control, String>();

        private void InitHints(Control parentControl)
        {
            foreach (Control control in ControlFinder.Descendants<Control>(parentControl))
            {
                if (control.Tag != null && control.Tag is string)
                {
                    controlHints[control] = control.Tag as string;

                    control.MouseEnter += delegate(object sender, EventArgs e)
                    {
                        SetTabStatus(controlHints[sender as Control], true);
                    };

                    control.MouseLeave += delegate(object sender, EventArgs e)
                    {
                        RestoreTabStatus();
                    };
                }
            }
        }

        private void SetTabStatus(TabPage tabPage, string message, bool temporary = false)
        {
            if (!temporary)
                tabStatusMessages[tabPage] = message;
            if (tabPage == tabControl.SelectedTab)
                statusLabel.Text = message;
        }

        private void SetTabStatus(string message, bool temporary = false)
        {
            SetTabStatus(tabControl.SelectedTab, message, temporary);
        }

        private void RestoreTabStatus()
        {
            if (tabStatusMessages.ContainsKey(tabControl.SelectedTab))
                statusLabel.Text = tabStatusMessages[tabControl.SelectedTab];
            else
                statusLabel.Text = "";
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

        #endregion
    }
}
