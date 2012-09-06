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

namespace BDInfo
{
    public partial class FormDetails : Form
    {
        private readonly string api_key = "b59b366b0f0a457d58995537d847409a";
        private readonly Tmdb api;

        private BDROM BDROM;
        private IList<TSPlaylistFile> playlists;
        private ISet<Language> languages;
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
            this.languages = languages;

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

        private void FormDetails_Load(object sender, System.EventArgs e)
        {
            this.movieNameTextBox.Text = BDROM.DiscNameSearchable;
            this.discLanguageComboBox.DataSource = languageCodes;

            InitOutputTab();

            populator.VideoLanguageChanged += VideoLanguageComboSelectionChanged;

            listViewStreamFiles.Enabled = true;
            listViewStreams.Enabled = true;

            ResetPlaylists();
            QueryMainMovie();
        }

        private void VideoLanguageComboSelectionChanged(object sender, EventArgs e)
        {
            InitOutputTab();
        }

        private void InitOutputTab()
        {
            comboBoxAudienceLanguage.DataSource = null;
            comboBoxAudienceLanguage.DataSource = new List<Language>(languages).ToArray();
            comboBoxAudienceLanguage.Enabled = languages.Count > 1;

            IList<Language> selectedVideoLanguages = populator.SelectedVideoLanguages;
            comboBoxVideoLanguage.DataSource = null;
            comboBoxVideoLanguage.DataSource = selectedVideoLanguages;
            comboBoxVideoLanguage.Enabled = selectedVideoLanguages.Count > 1;

            IList<Cut> selectedCuts = populator.SelectedCuts;
            comboBoxCut.DataSource = null;
            comboBoxCut.DataSource = selectedCuts;
            comboBoxCut.Enabled = selectedCuts.Count > 1;

            IList<CommentaryOption> selectedCommentaryOptions = populator.SelectedCommentaryOptions;
            comboBoxCommentary.DataSource = null;
            comboBoxCommentary.DataSource = selectedCommentaryOptions;
            comboBoxCommentary.Enabled = selectedCommentaryOptions.Count > 1;

            Language[] audioLanguages = AudioLanguages;
            listBoxAudioLanguages.DataSource = null;
            listBoxAudioLanguages.DataSource = audioLanguages;
            listBoxAudioLanguages.Enabled = audioLanguages.Length > 1;

            Language[] subtitleLanguages = AudioLanguages;
            listBoxSubtitleLanguages.DataSource = null;
            listBoxSubtitleLanguages.DataSource = SubtitleLanguages;
            listBoxSubtitleLanguages.Enabled = subtitleLanguages.Length > 1;
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
                Language[] audioLanguagesArray = new Language[audioLanguages.Count];
                audioLanguages.CopyTo(audioLanguagesArray, 0);

                // TODO: This doesn't seem to have any effect
                Array.Sort(audioLanguagesArray, delegate(Language lang1, Language lang2)
                {
                    return lang1.ISO_639_2.CompareTo(lang2.ISO_639_2);
                });

                return audioLanguagesArray;
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
                Language[] subtitleLanguagesArray = new Language[subtitleLanguages.Count];
                subtitleLanguages.CopyTo(subtitleLanguagesArray, 0);

                // TODO: This doesn't seem to have any effect
                Array.Sort(subtitleLanguagesArray, delegate(Language lang1, Language lang2)
                {
                    return lang1.ISO_639_2.CompareTo(lang2.ISO_639_2);
                });

                return subtitleLanguagesArray;
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

            tmdbBackgroundWorker = new BackgroundWorker();
            tmdbBackgroundWorker.WorkerReportsProgress = true;
            tmdbBackgroundWorker.WorkerSupportsCancellation = true;
            tmdbBackgroundWorker.DoWork += tmdbBackgroundWorker_DoWork;
            tmdbBackgroundWorker.ProgressChanged += tmdbBackgroundWorker_ProgressChanged;
            tmdbBackgroundWorker.RunWorkerCompleted += tmdbBackgroundWorker_RunWorkerCompleted;
            tmdbBackgroundWorker.RunWorkerAsync(this.movieNameTextBox.Text);
        }

        #endregion

        #region View Manipulation

        private void EnableForm(bool enabled)
        {
            discLanguageComboBox.Enabled = enabled;
            movieNameTextBox.Enabled = enabled;
            searchButton.Enabled = enabled;
            searchResultListView.Enabled = enabled;
            ResetContinueButton();
        }

        private void ResetColumnWidths(int offset = 0)
        {
            var width = searchResultListView.ClientSize.Width;
            var columns = searchResultListView.Columns;

            columns[0].Width = (int)(width - columns[1].Width - columns[2].Width) - offset;
        }

        private void ResetPlaylists()
        {
            populator.ShowAllPlaylists = showAllPlaylistsCheckbox.Checked;
        }

        private bool ResetContinueButton()
        {
            return continueButton.Enabled = searchResultListView.SelectedItems.Count > 0;
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
                EnableForm(true);
                searchResultListView.Enabled = false;
                continueButton.Enabled = false;
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
                this.movieNameTextBox.Text = disc.movie_title + " (" + disc.year + ")";

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
                movieSearch = api.SearchMovie((string)e.Argument, 1);
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

        private void tmdbBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (MovieResult movieResult in movieSearch.results)
            {
                ListViewItem.ListViewSubItem movieNameSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem movieYearSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem moviePopularitySubItem = new ListViewItem.ListViewSubItem();

                movieNameSubItem.Text = movieResult.title;
                movieYearSubItem.Text = movieResult.release_date != null ? Regex.Replace(movieResult.release_date, @"^(\d{4})-(\d{1,2})-(\d{1,2})$", @"$1", RegexOptions.IgnoreCase) : null;
                moviePopularitySubItem.Text = movieResult.popularity.ToString("N3", CultureInfo.CurrentUICulture);

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

            EnableForm(true);

            searchResultListView.Select();

            if (movieSearch.results.Count > 0)
            {
                searchResultListView.Items[0].Selected = true;
            }

            ResetColumnWidths(0);
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
            populator.MainLanguageCode = discLanguageComboBox.SelectedValue as string;
        }

        private void FormDetails_Resize(object sender, EventArgs e)
        {
            ResetColumnWidths();
        }

        private void searchResultListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetContinueButton();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SearchTmdb();
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (searchResultListView.SelectedIndices.Count > 0)
            {
                int index = searchResultListView.SelectedIndices[0];
                movieResult = movieSearch.results[index];

                if (auto_configured)
                {
                    if (movieResult.id != auto_tmdb_id || populator.HasChanged)
                    {
                        DialogResult answer = MessageBox.Show(this, "Submit a new disc to the database?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (answer == DialogResult.Yes)
                        {
                            SubmitJsonDisc();
                        }
                    }
                }
                else
                {
                    SubmitJsonDisc();
                }
            }

            //Close();
        }

        #endregion

        private void SubmitJsonDisc()
        {
            DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to submit to the database?", "Confirm database submit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No) return;

            JsonDisc jsonDisc = new JsonDisc();

            jsonDisc.disc_name = BDROM.DiscName;
            jsonDisc.volume_label = BDROM.VolumeLabel;
            jsonDisc.ISO_639_2 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_2 : null;

            jsonDisc.tmdb_id = movieResult.id;
            jsonDisc.movie_title = movieResult.title;
            jsonDisc.year = Convert.ToInt32(String.IsNullOrEmpty(movieResult.release_date) ? null : Regex.Replace(movieResult.release_date, @"^(\w{4})-.*", "$1", RegexOptions.IgnoreCase));

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
    }
}
