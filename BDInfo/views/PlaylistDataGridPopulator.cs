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

        public event EventHandler SelectionChanged;

        private TSPlaylistFile selectedPlaylist = null;
        public TSPlaylistFile SelectedPlaylist
        {
            get { return selectedPlaylist; }
        }

        public PlaylistDataGridPopulator(DataGridView dataGridView, IList<TSPlaylistFile> playlists, IList<string> languageCodes)
        {
            this.playlistDataGridView = dataGridView;
            this.playlists = playlists;
            this.languageCodes = languageCodes;

            this.playlistDataGridView.AutoGenerateColumns = false;
            this.playlistDataGridView.AutoSize = true;

            dataGridView.Columns.Add(CreatePlayButtonColumn());
            dataGridView.Columns.Add(CreateIsMainMovieColumn());
            dataGridView.Columns.Add(CreateFilenameColumn());
            dataGridView.Columns.Add(CreateLengthColumn());
            dataGridView.Columns.Add(CreateSizeColumn());
            dataGridView.Columns.Add(CreateVideoLanguageColumn());
            dataGridView.Columns.Add(CreateCutColumn());
            dataGridView.Columns.Add(CreateHasCommentaryColumn());

            this.playlistDataGridView.SelectionChanged += dataGridView_SelectionChanged;

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

        private void playButton_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells.
            //if (e.RowIndex < 0 || e.ColumnIndex != playlistDataGridView.Columns["Preview"].Index)
            if (e.RowIndex < 0 || e.ColumnIndex != 0)
                return;

            PlaylistGridItem item = bindingList[e.RowIndex];

            System.Diagnostics.Process.Start(item.Playlist.FullName);
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Skip if nothing is selected
            if (playlistDataGridView.SelectedRows.Count == 0 &&
                playlistDataGridView.SelectedCells.Count == 0) return;

            int rowIndex = -1;

            if (playlistDataGridView.SelectedRows.Count > 0)
                rowIndex = playlistDataGridView.SelectedRows[0].Index;
            else
                rowIndex = playlistDataGridView.SelectedCells[0].RowIndex;

            if (rowIndex == -1) return;

            PlaylistGridItem playlistItem = bindingList[rowIndex];

            if (playlistItem == null) return;

            selectedPlaylist = playlistItem.Playlist;

            if (selectedPlaylist == null) return;

            string playlistFileName = selectedPlaylist.Name;

            SelectionChanged.Invoke(this, EventArgs.Empty);
        }

        private DataGridViewButtonColumn CreatePlayButtonColumn()
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();
            //column.Name = "Preview";
            column.Text = "Play";
            //column.ToolTipText = "Open this file in the default application";
            column.UseColumnTextForButtonValue = true;

            // Add a CellClick handler to handle clicks in the button column.
            playlistDataGridView.CellClick += new DataGridViewCellEventHandler(playButton_CellClick);

            return column;
        }

        private DataGridViewCheckBoxColumn CreateIsMainMovieColumn()
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.Name = "Main Movie";
            column.DataPropertyName = "IsMainMovie";
            return column;
        }

        private DataGridViewTextBoxColumn CreateFilenameColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = "Filename";
            column.DataPropertyName = "Filename";
            column.ReadOnly = true;
            return column;
        }

        private DataGridViewTextBoxColumn CreateLengthColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = "Length";
            column.DataPropertyName = "Length";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.ReadOnly = true;
            return column;
        }

        private DataGridViewTextBoxColumn CreateSizeColumn()
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = "Size";
            column.DataPropertyName = "Size";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.ReadOnly = true;
            return column;
        }

        private DataGridViewComboBoxColumn CreateVideoLanguageColumn()
        {
            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            column.Name = "Video Language";
            column.DataSource = languageCodes;
            column.DataPropertyName = "VideoLanguage";
            return column;
        }

        private DataGridViewComboBoxColumn CreateCutColumn()
        {
            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            column.Name = "Cut";
            column.DataSource = Enum.GetValues(typeof(Cut));
            column.DataPropertyName = "Cut";
            return column;
        }

        private DataGridViewCheckBoxColumn CreateHasCommentaryColumn()
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            column.Name = "Commentary Available";
            column.DataPropertyName = "HasCommentary";
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
