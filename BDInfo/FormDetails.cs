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

        private MovieResult movieResult = null;
        private BackgroundWorker tmdbBackgroundWorker;

        private PlaylistDataGridPopulator populator;

        public FormDetails(BDROM BDROM, List<TSPlaylistFile> playlists, ISet<Language> languages)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.playlists = playlists;
            this.languages = languages;

            string ISO_639_1 = BDROM.DiscLanguage != null ? BDROM.DiscLanguage.ISO_639_1 : null;

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

            this.Load += FormDetails_Load;
        }

        private void FormDetails_Load(object sender, System.EventArgs e)
        {
            this.movieNameTextBox.Text = BDROM.DiscNameSearchable;
            this.discLanguageComboBox.DataSource = languageCodes;

            ResetPlaylists();
            SearchTmdb();
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
            }

            Close();
        }

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

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylists();
        }

        private void SubmitJsonDisc()
        {
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

            string jsonString = JsonConvert.SerializeObject(jsonDisc);
            Clipboard.SetText(jsonString);
            MessageBox.Show("Copied to clipboard: \n\n" + jsonString);
        }
    }
}
