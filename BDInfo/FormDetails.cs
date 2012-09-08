﻿using System;
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

namespace BDInfo
{
    public partial class FormDetails : Form
    {
        private readonly string api_key = "b59b366b0f0a457d58995537d847409a";
        private readonly Tmdb api;

        private BDROM BDROM;
        private IList<TSPlaylistFile> playlists;
        private IList<Language> languages;
        private IList<string> languageCodes = new List<string>();

        private TmdbMovieSearch movieSearch;
        private MainMovieService mainMovieService = new MainMovieService();
        private JsonSearchResult mainMovieSearchResult;

        private MovieResult movieResult = null;
        private BackgroundWorker mainMovieBackgroundWorker;
        private BackgroundWorker tmdbBackgroundWorker;

        private PlaylistDataGridPopulator populator;

        private bool auto_configured = false;
        private int auto_tmdb_id = -1;

        public FormDetails(BDROM BDROM, List<TSPlaylistFile> playlists, ISet<Language> languages)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.playlists = playlists;
            this.languages = new List<Language>(languages).ToArray();

            string ISO_639_1 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_1 : null;
            string ISO_639_2 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_2 : null;

            // TODO: This will fail if we're unable to auto-detect the disc language (e.g., ID4)
            //       or if the user changes the main disc language manually.
            if (String.IsNullOrEmpty(ISO_639_1))
                api = new Tmdb(api_key);
            else
                api = new Tmdb(api_key, ISO_639_1);

            foreach (Language lang in languages)
            {
                languageCodes.Add(lang.ISO_639_2);
            }

            this.populator = new PlaylistDataGridPopulator(playlistDataGridView, playlists, languageCodes);
            this.populator.SelectionChanged += dataGridView_SelectionChanged;
            this.populator.MainLanguageCode = ISO_639_2;

            this.Load += FormDetails_Load;
        }

        private void FormDetails_Load(object sender, EventArgs e)
        {
            this.movieNameTextBox.Text = BDROM.DiscNameSearchable;
            this.discLanguageComboBox.DataSource = new List<Language>(languages).ToArray();

            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);

            InitOutputTab();

            populator.ItemChanged += OnPlaylistItemChange;
            comboBoxAudienceLanguage.SelectedIndexChanged += OnAudienceLanguageChange;
            playlistDataGridView.CurrentCellDirtyStateChanged += playlistDataGridView_CurrentCellDirtyStateChanged;

            listViewStreamFiles.Enabled = true;
            listViewStreams.Enabled = true;

            ResetPlaylists();
            QueryMainMovie();
        }

        private bool ignoreFilterControlChange = false;
        private bool ignoreDataGridItemChange = false;

        private void OnPlaylistItemChange(object sender, EventArgs e)
        {
            InitOutputTab();
            buttonSubmitToDB.Enabled = CanSubmitToDB;
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
                    // TODO: This throws "InvalidIndex" exceptions sometimes - find out why and fix it!
                    control.SelectedIndex = -1;
                    control.SelectedIndex = list.IndexOf(audienceLanguage);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "BDInfo Error",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitOutputTab()
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

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            TSPlaylistFile playlistFile = populator.SelectedPlaylist;

            if (playlistFile == null) return;

            string playlistFileName = playlistFile.Name;

            StreamTrackListViewPopulator.Populate(playlistFile, listViewStreamFiles, listViewStreams);
        }

        #region Web Service Queries

        private void QueryMainMovie()
        {
            EnableForm(false);
            continueButton.Enabled = false;

            this.auto_configured = false;
            this.auto_tmdb_id = -1;

            mainMovieBackgroundWorker = new BackgroundWorker();
            mainMovieBackgroundWorker.WorkerReportsProgress = true;
            mainMovieBackgroundWorker.WorkerSupportsCancellation = true;
            mainMovieBackgroundWorker.DoWork += mainMovieBackgroundWorker_DoWork;
            mainMovieBackgroundWorker.ProgressChanged += mainMovieBackgroundWorker_ProgressChanged;
            mainMovieBackgroundWorker.RunWorkerCompleted += mainMovieBackgroundWorker_RunWorkerCompleted;
            mainMovieBackgroundWorker.RunWorkerAsync(this.movieNameTextBox.Text);
        }

        private void SearchTmdb()
        {
            if (String.IsNullOrEmpty(this.movieNameTextBox.Text))
                return;

            searchResultListView.Items.Clear();

            EnableForm(false);
            continueButton.Enabled = false;

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

        #region View Manipulation

        private void EnableForm(bool enabled)
        {
            discLanguageComboBox.Enabled = enabled;
            movieNameTextBox.Enabled = enabled;
            searchButton.Enabled = enabled;
            searchResultListView.Enabled = enabled;
        }

        private void ResizeDiscTab(object sender = null, EventArgs e = null)
        {
            var width = searchResultListView.ClientSize.Width;
            var columns = searchResultListView.Columns;
            columns[0].Width = (int)(width - columns[1].Width - columns[2].Width);
        }

        private void ResetPlaylists()
        {
            populator.ShowAllPlaylists = showAllPlaylistsCheckbox.Checked;
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

        private bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        private bool IsDirectory(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }

        private bool HasFile(DragEventArgs e)
        {
            return GetFirstFilePath(e) != null;
        }

        private bool HasDirectory(DragEventArgs e)
        {
            return GetFirstDirectoryPath(e) != null;
        }

        private string[] GetPaths(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return (string[])e.Data.GetData(DataFormats.FileDrop, false);
            }
            return new string[0];
        }

        private string GetFirstFilePath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            foreach (string path in paths)
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == 0)
                    return path;
            }
            return null;
        }

        private string GetFirstDirectoryPath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            foreach (string path in paths)
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return path;
            }
            return null;
        }

        private string GetFirstPath(DragEventArgs e)
        {
            string[] paths = GetPaths(e);
            return paths.Length > 0 ? paths[0] : null;
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
            string path = GetFirstPath(e);
            if (path != null)
            {
                if (IsDirectory(path))
                    textBoxOutputDir.Text = path;
                else
                    textBoxOutputDir.Text = Path.GetDirectoryName(path);

                // TODO: Validate that the selected directory has enough free space for 2x-3x the playlist size
            }
        }

        private void textBoxOutputFileName_DragEnter(object sender, DragEventArgs e)
        {
            if (HasFile(e) && !String.IsNullOrEmpty(GetFirstFileNameWithoutExtension(e)))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBoxOutputFileName_DragDrop(object sender, DragEventArgs e)
        {
            string filenameWithoutExtension = GetFirstFileNameWithoutExtension(e);
            if (!String.IsNullOrEmpty(filenameWithoutExtension))
                textBoxOutputFileName.Text = filenameWithoutExtension;
        }

        private string GetFirstFileNameWithoutExtension(DragEventArgs e)
        {
            string path = GetFirstFilePath(e);
            return path == null ? null : Path.GetFileNameWithoutExtension(path);
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
            // TODO: Uncomment once tab switching works
            //continueButton.Enabled = true;

            if (e.Result is Exception)
            {
                string msg = ((Exception)e.Result).Message;

                MessageBox.Show(msg, "BDInfo Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // TODO: Disable POST submission only - don't disable ripping completely
                EnableForm(true);
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
            else if(mainMovieSearchResult.discs.Count == 0)
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
                movieSearch = api.SearchMovie(reqParams.query, 1, reqParams.ISO_639_1, false, reqParams.year);
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
            return String.IsNullOrEmpty(yearString) ? (int?) null : Convert.ToInt32(yearString);
        }

        private void tmdbBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableForm(true);
            // TODO: Uncomment once tab switching works
            //continueButton.Enabled = true;

            if (movieSearch == null || movieSearch.results == null)
                return;

            foreach (MovieResult curResult in movieSearch.results)
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

            if (movieSearch.results.Count > 0)
            {
                searchResultListView.Items[0].Selected = true;
            }

            ResizeDiscTab();
        }

        #endregion

        #region Event Handlers

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylists();
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

        private void searchResultListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.movieResult = null;
            this.movieTitle = null;
            this.movieYear = null;

            if (this.movieSearch != null && searchResultListView.SelectedIndices.Count > 0)
            {
                int index = searchResultListView.SelectedIndices[0];
                this.movieResult = this.movieSearch.results[index];

                if (movieResult != null)
                {
                    this.movieTitle = this.movieResult.title;
                    this.movieYear = GetYearInt(this.movieResult.release_date);
                }
            }

            this.buttonSubmitToDB.Enabled = CanSubmitToDB;

            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SearchTmdb();
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            // TODO: Fix TabIndexChanged and SelectedIndexChanged
            return;

            TabPage curTab = tabControl.TabPages[tabControl.TabIndex];

            if (curTab == tabPageDisc)
            {
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (curTab == tabPagePlaylists)
            {
                SubmitJsonDiscIfNecessary();
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
            }
            else if (curTab == tabPageOutput)
            {
                tabControl.SelectedIndex++;
                tabControl.TabIndex++;
                Rip();
            }
        }

        private void Rip()
        {

        }

        private bool CanSubmitToDB
        {
            get
            {
                if (searchResultListView.SelectedIndices.Count > 0)
                {
                    if (auto_configured)
                    {
                        if (movieResult.id != auto_tmdb_id || populator.HasChanged)
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

        #endregion

        private void SubmitJsonDisc()
        {
            if (searchResultListView.SelectedItems.Count == 0) return;

            DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to submit to the database?", "Confirm database submit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No) return;

            JsonDisc jsonDisc = new JsonDisc();

            jsonDisc.disc_name = BDROM.DiscName;
            jsonDisc.volume_label = BDROM.VolumeLabel;
            jsonDisc.ISO_639_2 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_2 : null;

            jsonDisc.tmdb_id = movieResult != null ? movieResult.id : -1;
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

        private void checkBoxReplaceSpaces_CheckedChanged(object sender, EventArgs e)
        {
            textBoxReplaceSpaces.Enabled = checkBoxReplaceSpaces.Checked;
            if (checkBoxReplaceSpaces.Checked)
            {
                textBoxReplaceSpaces.Focus();
                textBoxReplaceSpaces.SelectAll();
            }
            textBoxOutputFileName_TextChanged(this, EventArgs.Empty);
        }

        private void textBoxOutputFileName_TextChanged(object sender, EventArgs e)
        {
            string preview = textBoxOutputFileName.Text;

            string title = String.IsNullOrEmpty(this.movieTitle) ? movieNameTextBox.Text : this.movieTitle;
            string year = this.movieYear + "";
            string res = null;

            if (listViewVideoTracks.CheckedItems.Count > 0)
            {
                res = (listViewVideoTracks.CheckedItems[0].Tag as TSVideoStream).HeightDescription;
            }

            year = String.IsNullOrEmpty(year) ? "xxxx" : year;
            res = String.IsNullOrEmpty(res) ? "xxxxx" : res;

            preview = Regex.Replace(preview, @"%title%", title, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%year%", year, RegexOptions.IgnoreCase);
            preview = Regex.Replace(preview, @"%res%", res, RegexOptions.IgnoreCase);

            if (checkBoxReplaceSpaces.Checked)
            {
                preview = preview.Replace(" ", textBoxReplaceSpaces.Text);
            }

            labelOutputFileNamePreview.Text = preview +labelOutputFileExtension.Text;
        }

        private void textBoxReplaceSpaces_TextChanged(object sender, EventArgs e)
        {
            textBoxOutputFileName_TextChanged(sender, e);
        }

        private void textBoxReplaceSpaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            // TODO: Also handle pasted text
            if (!IsValidFilename(e.KeyChar + "") && e.KeyChar >= 32 && e.KeyChar != 127)
                e.Handled = true;
        }

        /// <see cref="http://stackoverflow.com/questions/62771/how-check-if-given-string-is-legal-allowed-file-name-under-windows"/>
        bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
            return !containsABadCharacter.IsMatch(testName);
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

        private ISet<TSPlaylistFile> filteredPlaylists = new HashSet<TSPlaylistFile>();

        private IList<Language> audioLanguages = new List<Language>();
        private IList<Language> subtitleLanguages = new List<Language>();

        private List<TSVideoStream> videoTracks = new List<TSVideoStream>();
        private List<TSAudioStream> audioTracks = new List<TSAudioStream>();
        private List<TSStream> subtitleTracks = new List<TSStream>();

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

            ISet<TSPlaylistFile> playlistsWithVideoLanguage = populator.GetPlaylistsWithVideoLanguage(videoLanguage);
            ISet<TSPlaylistFile> playlistsWithCut = populator.GetPlaylistsWithCut(cut);
            ISet<TSPlaylistFile> playlistsWithCommentaryOption = populator.GetPlaylistsWithCommentaryOption(commentaryOption);
            ISet<TSPlaylistFile> playlistsWithAudioLanguages = populator.GetPlaylistsWithAudioLanguages(audioLanguages);
            ISet<TSPlaylistFile> playlistsWithSubtitleLanguages = populator.GetPlaylistsWithSubtitleLanguages(subtitleLanguages);

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

        private void ResizeOutputTab(object sender = null, EventArgs e = null)
        {
            listViewVideoTracks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewVideoTracks.AutoResizeColumn(columnHeaderVideoCodec.Index, ColumnHeaderAutoResizeStyle.ColumnContent);

            listViewAudioTracks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewAudioTracks.AutoResizeColumn(columnHeaderAudioCodec.Index, ColumnHeaderAutoResizeStyle.ColumnContent);

            listViewSubtitleTracks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewSubtitleTracks.AutoResizeColumn(columnHeaderSubtitleCodec.Index, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
            TabPage curTab = tabControl.TabPages[tabControl.TabIndex];

            continueButton.Text = "Continue";
            // TODO: Uncomment once tab switching works
            //continueButton.Enabled = true;

            if (curTab == tabPageOutput)
            {
                continueButton.Text = "Rip It!";
            }
            else if (curTab == tabPageProgress)
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
            // TODO: Implement ignoreDataGridItemChange
            //ignoreDataGridItemChange = true;
            populator.SelectAll = true;
            //ignoreDataGridItemChange = false;
        }

        private void buttonUnselectAll_Click(object sender, EventArgs e)
        {
            // TODO: Implement ignoreDataGridItemChange
            //ignoreDataGridItemChange = true;
            populator.SelectAll = false;
            //ignoreDataGridItemChange = false;
        }
    }
}
