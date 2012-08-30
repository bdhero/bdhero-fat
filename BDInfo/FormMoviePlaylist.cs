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

namespace BDInfo
{
    public enum ReleaseType
    {
        Theatrical,
        Special,
        Extended,
        Unrated
    }

    public partial class FormMoviePlaylist : Form
    {
        private BDROM BDROM;
        private List<TSPlaylistFile> allPlaylists;
        private List<TSPlaylistFile> mainPlaylists;
        private MovieResult movieResult;

        private BindingList<PlaylistGridItem> bindingList = new BindingList<PlaylistGridItem>();

        private IList<Language> languages = new List<Language>();
        private IList<string> languageCodes = new List<string>();
        private IList<string> languageNames = new List<string>();

        private List<TSPlaylistFile> Playlists
        {
            get { return showAllPlaylistsCheckbox.Checked ? allPlaylists : mainPlaylists; }
        }

        public FormMoviePlaylist(BDROM BDROM, List<TSPlaylistFile> allPlaylists, List<TSPlaylistFile> mainPlaylists, MovieResult movieResult)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.allPlaylists = allPlaylists;
            this.mainPlaylists = mainPlaylists;
            this.movieResult = movieResult;

            this.Load += FormMoviePlaylist_Load;
        }

        private void FormMoviePlaylist_Load(object sender, System.EventArgs e)
        {
            InitDataGridView();
            ResetPlaylists();
        }

        private void InitDataGridView()
        {
            playlistDataGridView.AutoGenerateColumns = false;
            playlistDataGridView.AutoSize = true;

            playlistDataGridView.Columns.Add(CreateIsMainMovieColumn());
            playlistDataGridView.Columns.Add(CreateFilenameColumn());
            playlistDataGridView.Columns.Add(CreateLengthColumn());
            playlistDataGridView.Columns.Add(CreateSizeColumn());
            playlistDataGridView.Columns.Add(CreateVideoLanguageColumn());
            playlistDataGridView.Columns.Add(CreateReleaseTypeColumn());
            playlistDataGridView.Columns.Add(CreateHasCommentaryColumn());
        }

        private DataGridViewCheckBoxColumn CreateIsMainMovieColumn()
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "IsMainMovie";
            column.Name = "Main Movie";
            return column;
        }

        private DataGridViewTextBoxColumn CreateFilenameColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Filename";
            column.Name = "Filename";
            column.ReadOnly = true;
            return column;
        }

        private DataGridViewTextBoxColumn CreateLengthColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Length";
            column.Name = "Length";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.ReadOnly = true;
            return column;
        }

        private DataGridViewTextBoxColumn CreateSizeColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Size";
            column.Name = "Size";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.ReadOnly = true;
            return column;
        }

        private DataGridViewComboBoxColumn CreateVideoLanguageColumn()
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            
            combo.DataSource = languageCodes;
            combo.DataPropertyName = "VideoLanguage";
            //combo.DisplayMember = "Name";
            //combo.ValueMember = "ISO_639_2";

            combo.Name = "Video Language";
            return combo;
        }

        private DataGridViewComboBoxColumn CreateReleaseTypeColumn()
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.DataSource = Enum.GetValues(typeof(ReleaseType));
            combo.DataPropertyName = "ReleaseType";
            combo.Name = "Release Type";
            return combo;
        }

        private DataGridViewCheckBoxColumn CreateHasCommentaryColumn()
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "HasCommentary";
            column.Name = "Commentary Available";
            return column;
        }

        public static string HumanFriendlyLength(double length)
        {
            TimeSpan playlistLengthSpan = new TimeSpan((long)(length * 10000000));
            return string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                playlistLengthSpan.Hours,
                playlistLengthSpan.Minutes,
                playlistLengthSpan.Seconds);
        }

        private void ResetPlaylists()
        {
            playlistDataGridView.DataSource = null;
            
            bindingList.Clear();
            languages.Clear();
            languageCodes.Clear();
            languageNames.Clear();

            foreach (TSPlaylistFile playlistFile in Playlists)
            {
                foreach (TSStream stream in playlistFile.SortedStreams)
                {
                    if (stream.LanguageCode != null && !languageCodes.Contains(stream.LanguageCode))
                    {
                        Language lang = LanguageCodes.GetLanguage(stream.LanguageCode);
                        languages.Add(lang);
                        languageCodes.Add(stream.LanguageCode);
                        languageNames.Add(stream.LanguageName);
                    }
                }
            }

            foreach (TSPlaylistFile playlist in Playlists)
            {
                bindingList.Add(new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null));
            }

            playlistDataGridView.DataSource = bindingList;
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
            jsonDisc.ISO_639_2 = BDROM.DiscLanguage.ISO_639_2;

            jsonDisc.tmdb_id = movieResult.id;
            jsonDisc.movie_title = movieResult.title;
            jsonDisc.year = Convert.ToInt32(String.IsNullOrEmpty(movieResult.release_date) ? null : Regex.Replace(movieResult.release_date, @"^(\w{4})-.*", "$1", RegexOptions.IgnoreCase));

            jsonDisc.playlists = new List<JsonPlaylist>();

            foreach (PlaylistGridItem item in bindingList)
            {
                // Skip unchecked items
                if (!item.IsMainMovie) continue;

                JsonPlaylist jsonPlaylist = new JsonPlaylist();

                jsonPlaylist.filename = item.Filename;
                jsonPlaylist.filesize = item.Playlist.FileSize;
                jsonPlaylist.length = item.Playlist.TotalLength;
                jsonPlaylist.ISO_639_2 = item.VideoLanguage;

                jsonPlaylist.is_main = true;
                jsonPlaylist.is_theatrical = item.ReleaseType.Equals(ReleaseType.Theatrical);
                jsonPlaylist.is_special = item.ReleaseType.Equals(ReleaseType.Special);
                jsonPlaylist.is_extended = item.ReleaseType.Equals(ReleaseType.Extended);
                jsonPlaylist.is_unrated = item.ReleaseType.Equals(ReleaseType.Unrated);
                jsonPlaylist.is_commentary = item.HasCommentary;

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

    public class PlaylistGridItem
    {
        private TSPlaylistFile playlist;
        private bool isMainMovie;
        private string filename;
        private string length;
        private string size;
        private string ISO_639_2;
        private ReleaseType releaseType;
        private bool hasCommentary;

        public PlaylistGridItem(TSPlaylistFile playlist, string ISO_639_2)
        {
            this.playlist = playlist;
            this.isMainMovie = playlist.IsMainPlaylist;
            this.filename = playlist.Name;
            this.length = FormMoviePlaylist.HumanFriendlyLength(playlist.TotalLength);
            this.size = playlist.FileSize.ToString("N0");
            this.ISO_639_2 = ISO_639_2;
            this.releaseType = ReleaseType.Theatrical;
            this.hasCommentary = false;
        }

        public TSPlaylistFile Playlist
        {
            get { return playlist; }
        }

        public bool IsMainMovie
        {
            get { return isMainMovie; }
            set { isMainMovie = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public string Length
        {
            get { return length; }
            set { length = value; }
        }

        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        public string VideoLanguage
        {
            get { return ISO_639_2; }
            set { ISO_639_2 = value; }
        }

        public ReleaseType ReleaseType
        {
            get { return releaseType; }
            set { releaseType = value; }
        }

        public bool HasCommentary
        {
            get { return hasCommentary; }
            set { hasCommentary = value; }
        }
    }
}
