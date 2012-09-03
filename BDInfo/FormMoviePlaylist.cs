using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDInfo.models;
using WatTmdb.V3;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using BDInfo.views;

namespace BDInfo
{
    public partial class FormMoviePlaylist : Form
    {
        private BDROM BDROM;
        private IList<TSPlaylistFile> playlists;
        private ISet<Language> languages;
        private IList<string> languageCodes = new List<string>();
        private MovieResult movieResult;

        private PlaylistDataGridPopulator populator;

        public FormMoviePlaylist(BDROM BDROM, List<TSPlaylistFile> playlists, ISet<Language> languages, MovieResult movieResult)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.playlists = playlists;
            this.languages = languages;
            this.movieResult = movieResult;

            foreach (Language lang in languages)
            {
                languageCodes.Add(lang.ISO_639_2);
            }

            this.Load += FormMoviePlaylist_Load;
        }

        private void FormMoviePlaylist_Load(object sender, System.EventArgs e)
        {
            InitDataGridView();
            ResetPlaylists();
        }

        private void InitDataGridView()
        {
            populator = new PlaylistDataGridPopulator(playlistDataGridView, playlists, languageCodes);
        }

        private void ResetPlaylists()
        {
            populator.ShowAllPlaylists = showAllPlaylistsCheckbox.Checked;
        }

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylists();
        }

        private void submitButton_Click(object sender, EventArgs e)
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
