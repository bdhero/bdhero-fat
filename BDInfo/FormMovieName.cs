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

namespace BDInfo
{
    public partial class FormMovieName : Form
    {
        private readonly string api_key = "b59b366b0f0a457d58995537d847409a";
        private readonly Tmdb api;

        private string DiscNameSearchable;
        private string ISO_639_1;
        private TmdbMovieSearch movieSearch;

        private FormMovieNameDelegate formMovieNameDelegate;
        private BackgroundWorker worker;

        public FormMovieName(string DiscNameSearchable, string ISO_639_1, FormMovieNameDelegate formMovieNameDelegate)
        {
            InitializeComponent();

            this.formMovieNameDelegate = formMovieNameDelegate;
            this.DiscNameSearchable = DiscNameSearchable;
            this.ISO_639_1 = ISO_639_1;

            this.searchTextBox.Text = DiscNameSearchable;

            if (String.IsNullOrEmpty(ISO_639_1))
            {
                api = new Tmdb(api_key);
            }
            else
            {
                api = new Tmdb(api_key, ISO_639_1);
            }

            SearchTmdb();
        }

        private void SearchTmdb()
        {
            if (String.IsNullOrEmpty(this.searchTextBox.Text))
                return;

            searchResultListView.Items.Clear();

            EnableForm(false);

            movieSearch = api.SearchMovie(this.searchTextBox.Text, 1);

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

            ResetColumnWidths(20);
        }

        private void EnableForm(bool enabled)
        {
            searchTextBox.Enabled = enabled;
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

        private bool ResetContinueButton()
        {
            return continueButton.Enabled = searchResultListView.SelectedItems.Count > 0;
        }

        private void FormMovieName_Resize(object sender, EventArgs e)
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
            MovieResult movieResult = null;

            if (searchResultListView.SelectedIndices.Count > 0)
            {
                movieResult = movieSearch.results[0];
            }

            formMovieNameDelegate.Invoke(movieResult);

            Close();
        }
    }
}
