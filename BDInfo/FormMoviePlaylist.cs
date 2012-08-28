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
    public partial class FormMoviePlaylist : Form
    {
        private BDROM BDROM;
        private List<TSPlaylistFile> allPlaylists;
        private List<TSPlaylistFile> mainPlaylists;

        public FormMoviePlaylist(BDROM BDROM, List<TSPlaylistFile> allPlaylists, List<TSPlaylistFile> mainPlaylists)
        {
            InitializeComponent();

            this.BDROM = BDROM;
            this.allPlaylists = allPlaylists;
            this.mainPlaylists = mainPlaylists;

            ResetPlaylists();
        }

        private void ResetPlaylists()
        {
            List<TSPlaylistFile> playlists;

            if (showAllPlaylistsCheckbox.Checked)
            {
                playlists = allPlaylists;
            }
            else
            {
                playlists = mainPlaylists;
            }

            this.playlistListView.Items.Clear();

            foreach (TSPlaylistFile playlist in playlists)
            {
                AddListItem(playlist);
            }
        }

        private bool IsMainPlaylist(TSPlaylistFile playlist)
        {
            foreach (TSPlaylistFile mainPlaylist in mainPlaylists)
            {
                if (mainPlaylist == playlist)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddListItem(TSPlaylistFile playlist)
        {
            ListViewItem.ListViewSubItem filenameSubItem = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lengthSubItem = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem trackTypeSubItem = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem releaseTypeSubItem = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem hasCommentarySubItem = new ListViewItem.ListViewSubItem();

            filenameSubItem.Text = playlist.Name;
            lengthSubItem.Text = playlist.FileSize.ToString();
            //trackTypeSubItem.Text = 

            ListViewItem.ListViewSubItem[] playlistSubItems = new ListViewItem.ListViewSubItem[]
            {
                filenameSubItem,
                lengthSubItem,
                trackTypeSubItem,
                releaseTypeSubItem,
                hasCommentarySubItem
            };

            ListViewItem playlistListItem = new ListViewItem(playlistSubItems, 0);

            if (IsMainPlaylist(playlist))
            {
                playlistListItem.Checked = true;
            }

            playlistListView.Items.Add(playlistListItem);
        }

        private void showAllPlaylistsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResetPlaylists();
        }
    }
}
