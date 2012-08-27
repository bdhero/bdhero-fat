using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WatTmdb.V3;

namespace BDInfo
{
    public partial class FormMovieName : Form
    {
        private string DiscNameSearchable;
        private string ISO_639_1;

        public FormMovieName(string DiscNameSearchable, string ISO_639_1)
        {
            InitializeComponent();

            this.DiscNameSearchable = DiscNameSearchable;
            this.ISO_639_1 = ISO_639_1;

            SearchTmdb();
        }

        private void SearchTmdb()
        {
            this.searchTextBox.Text = DiscNameSearchable;

            Tmdb api;

            if (String.IsNullOrEmpty(ISO_639_1))
            {
                api = new Tmdb("b59b366b0f0a457d58995537d847409a");
            }
            else
            {
                api = new Tmdb("b59b366b0f0a457d58995537d847409a", ISO_639_1);
            }

            TmdbMovieSearch movieSearch = api.SearchMovie(DiscNameSearchable, 1);

            foreach (MovieResult movieResult in movieSearch.results)
            {
                ListViewItem.ListViewSubItem moviePosterSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem movieNameSubItem = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem movieYearSubItem = new ListViewItem.ListViewSubItem();

                moviePosterSubItem.Text = "";
                movieNameSubItem.Text = movieResult.title;
                movieYearSubItem.Text = movieResult.release_date;

                ListViewItem.ListViewSubItem[] searchResultSubItems =
                    new ListViewItem.ListViewSubItem[]
                        {
                            moviePosterSubItem,
                            movieNameSubItem,
                            movieYearSubItem
                        };

                ListViewItem searchResultListItem =
                    new ListViewItem(searchResultSubItems, 0);

                searchResultListView.Items.Add(searchResultListItem);

                searchResultTextBox.Text += movieResult.title + " (" + movieResult.release_date + ")" + "\n";
            }
        }
    }
}
