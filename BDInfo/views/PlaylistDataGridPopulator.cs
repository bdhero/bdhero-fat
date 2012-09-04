using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using BDInfo.models;

namespace BDInfo.views
{
    class PlaylistDataGridPopulator
    {
        private DataGridView playlistDataGridView;
        private IList<TSPlaylistFile> playlists;
        private IList<string> languageCodes;
        private BindingList<PlaylistGridItem> bindingList = new BindingList<PlaylistGridItem>();

        private bool showAllPlaylists = false;

        private IList<PlaylistGridItem> playlistGridItems = new List<PlaylistGridItem>();

        public PlaylistDataGridPopulator(DataGridView playlistDataGridView, IList<TSPlaylistFile> playlists, IList<string> languageCodes)
        {
            this.playlistDataGridView = playlistDataGridView;
            this.playlists = playlists;
            this.languageCodes = languageCodes;

            this.playlistDataGridView.AutoGenerateColumns = false;
            this.playlistDataGridView.AutoSize = true;

            playlistDataGridView.Columns.Add(CreateIsMainMovieColumn());
            playlistDataGridView.Columns.Add(CreateFilenameColumn());
            playlistDataGridView.Columns.Add(CreateLengthColumn());
            playlistDataGridView.Columns.Add(CreateSizeColumn());
            playlistDataGridView.Columns.Add(CreateVideoLanguageColumn());
            playlistDataGridView.Columns.Add(CreateCutColumn());
            playlistDataGridView.Columns.Add(CreateHasCommentaryColumn());

            foreach (TSPlaylistFile playlist in playlists)
            {
                playlistGridItems.Add(new PlaylistGridItem(playlist, languageCodes.Count > 0 ? languageCodes[0] : null));
            }

            ShowAllPlaylists = false;
        }

        public bool ShowAllPlaylists
        {
            get { return showAllPlaylists; }
            set
            {
                showAllPlaylists = value;

                playlistDataGridView.DataSource = null;
                bindingList.Clear();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (showAllPlaylists || item.Playlist.IsMainPlaylist)
                    {
                        bindingList.Add(item);
                    }
                }
                
                playlistDataGridView.DataSource = bindingList;
            }
        }

        public IList<JsonPlaylist> JsonPlaylists
        {
            get
            {
                IList<JsonPlaylist> jsonPlaylists = new List<JsonPlaylist>();

                foreach (PlaylistGridItem item in playlistGridItems)
                {
                    if (item.Playlist.IsMainPlaylist)
                    {
                        jsonPlaylists.Add(item.JsonPlaylist);
                    }
                }

                return jsonPlaylists;
            }
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

            combo.Name = "Video Language";
            return combo;
        }

        private DataGridViewComboBoxColumn CreateCutColumn()
        {
            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.DataSource = Enum.GetValues(typeof(Cut));
            combo.DataPropertyName = "Cut";
            combo.Name = "Cut";
            return combo;
        }

        private DataGridViewCheckBoxColumn CreateHasCommentaryColumn()
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "HasCommentary";
            column.Name = "Commentary Available";
            return column;
        }
    }

    public enum Cut
    {
        Theatrical,
        Special,
        Extended,
        Unrated
    }

    public class PlaylistGridItem
    {
        private TSPlaylistFile playlist;
        private bool isMainMovie;
        private string filename;
        private double length;
        private string size;
        private string ISO_639_2;
        private Cut cut;
        private bool hasCommentary;

        public PlaylistGridItem(TSPlaylistFile playlist, string ISO_639_2)
        {
            this.playlist = playlist;
            this.isMainMovie = playlist.IsMainPlaylist;
            this.filename = playlist.Name;
            this.length = playlist.TotalLength;
            this.size = playlist.FileSize.ToString("N0");
            this.ISO_639_2 = ISO_639_2;
            this.cut = Cut.Theatrical;
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
        }

        public string Length
        {
            get { return HumanFriendlyLength(length); }
        }

        public string Size
        {
            get { return size; }
        }

        public string VideoLanguage
        {
            get { return ISO_639_2; }
            set { ISO_639_2 = value; }
        }

        public Cut Cut
        {
            get { return cut; }
            set { cut = value; }
        }

        public bool HasCommentary
        {
            get { return hasCommentary; }
            set { hasCommentary = value; }
        }

        public JsonPlaylist JsonPlaylist
        {
            get
            {
                JsonPlaylist jsonPlaylist = new JsonPlaylist();

                jsonPlaylist.filename = Filename;
                jsonPlaylist.filesize = Playlist.FileSize;
                jsonPlaylist.length = Playlist.TotalLength;
                jsonPlaylist.ISO_639_2 = VideoLanguage;

                jsonPlaylist.is_main = IsMainMovie;
                jsonPlaylist.is_theatrical = Cut.Equals(Cut.Theatrical);
                jsonPlaylist.is_special = Cut.Equals(Cut.Special);
                jsonPlaylist.is_extended = Cut.Equals(Cut.Extended);
                jsonPlaylist.is_unrated = Cut.Equals(Cut.Unrated);
                jsonPlaylist.is_commentary = HasCommentary;

                return jsonPlaylist;
            }
        }

        private static string HumanFriendlyLength(double length)
        {
            TimeSpan playlistLengthSpan = new TimeSpan((long)(length * 10000000));
            return string.Format(
                "{0:D2}:{1:D2}:{2:D2}",
                playlistLengthSpan.Hours,
                playlistLengthSpan.Minutes,
                playlistLengthSpan.Seconds);
        }
    }
}
