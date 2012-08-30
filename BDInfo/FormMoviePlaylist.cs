using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        private BindingList<PlaylistGridItem> bindingList = new BindingList<PlaylistGridItem>();
        private IList<Language> languages = new List<Language>();
        private IList<string> languageCodes = new List<string>();
        private IList<string> languageNames = new List<string>();

        private List<TSPlaylistFile> Playlists
        {
            get { return showAllPlaylistsCheckbox.Checked ? allPlaylists : mainPlaylists; }
        }

        public FormMoviePlaylist(BDROM BDROM, List<TSPlaylistFile> allPlaylists, List<TSPlaylistFile> mainPlaylists)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.allPlaylists = allPlaylists;
            this.mainPlaylists = mainPlaylists;

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

        private string HumanFriendlyLength(double length)
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

            foreach (TSPlaylistFile playlistFile in Playlists)
            {
                bindingList.Add(
                    new PlaylistGridItem(
                        playlistFile.IsMainPlaylist,
                        playlistFile.Name,
                        HumanFriendlyLength(playlistFile.TotalLength),
                        playlistFile.FileSize.ToString("N0"),
                        languageCodes.Count > 0 ? languageCodes[0] : null
                    )
                );
            }

            playlistDataGridView.DataSource = bindingList;
        }

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylists();
        }
    }

    public class PlaylistGridItem
    {
        private bool isMainMovie;
        private string filename;
        private string length;
        private string size;
        private string videoLanguage;
        private ReleaseType releaseType;
        private bool hasCommentary;

        public PlaylistGridItem(bool isMainMovie, string filename, string length, string size, string videoLanguage)
        {
            this.isMainMovie = isMainMovie;
            this.filename = filename;
            this.length = length;
            this.size = size;
            this.videoLanguage = videoLanguage;
            this.releaseType = ReleaseType.Theatrical;
            this.hasCommentary = false;
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
            get { return videoLanguage; }
            set { videoLanguage = value; }
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
