﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHeroGUI.Forms;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;

namespace BDHeroGUI.Components
{
    public partial class PlaylistListView : UserControl
    {
        /// <summary>
        /// Gets or sets the  list of playlists to display, maintaining the user's current selection if possible.
        /// </summary>
        public IList<Playlist> Playlists
        {
            get { return _playlists; }
            set
            {
                var selected = SelectedPlaylist;

                _playlists = value;
                listView.Items.Clear();

                if (_playlists == null || !_playlists.Any()) return;

                var items = Transform(_playlists);
                listView.Items.AddRange(items);
                listView.AutoSizeColumns();

                SelectedPlaylist = selected;
            }
        }

        public IList<Playlist> VisiblePlaylistsInSortOrder
        {
            get { return listView.Items.OfType<ListViewItem>().Select(item => item.Tag as Playlist).ToList(); }
        }

        /// <summary>
        /// Gets or sets the currently selected playlist.  A value of <c>null</c> indicates that no playlist is selected.
        /// </summary>
        public Playlist SelectedPlaylist
        {
            get { return listView.SelectedItems.Count > 0 ? listView.SelectedItems[0].Tag as Playlist : null; }
            set
            {
                if (value == null)
                {
                    listView.SelectNone();
                    return;
                }

                listView.SelectWhere(item => item.Tag == value);

                if (listView.SelectedItems.Count == 0 && listView.Items.Count > 0)
                {
                    listView.Items[0].Selected = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether all playlists are shown, regardless of the user's filter settings.
        /// </summary>
        public bool ShowAll
        {
            get { return checkBoxShowAllPlaylists.Checked; }
            set { checkBoxShowAllPlaylists.Checked = value; }
        }

        /// <summary>
        /// Triggered whenever the user selects a new playlist or deselects the current playlist.
        /// </summary>
        public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;

        public event EventHandler ShowAllChanged;

        private IList<Playlist> _playlists;

        private readonly PlaylistFilter _filter = new PlaylistFilter();

        private bool _showAllPlaylists;

        public PlaylistListView()
        {
            InitializeComponent();
            Load += OnLoad;

            listView.ItemSelectionChanged += delegate(object sender, ListViewItemSelectionChangedEventArgs args)
                {
                    if (ItemSelectionChanged != null)
                        ItemSelectionChanged(sender, args);
                };
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            listView.SetSortColumn(columnHeaderType.Index);
        }

        private ListViewItem[] Transform(IEnumerable<Playlist> playlists)
        {
            return playlists.Where(ShowPlaylist).Select(Transform).ToArray();
        }

        private bool ShowPlaylist(Playlist playlist)
        {
            return _filter.Show(playlist) || _showAllPlaylists;
        }

        private static ListViewItem Transform(Playlist playlist)
        {
            var item = new ListViewItem(playlist.FileName) { Tag = playlist };

            item.SubItems.AddRange(new[]
                {
                    new ListViewItem.ListViewSubItem(item, playlist.Length.ToStringMedium()) { Tag = playlist.Length },
                    new ListViewItem.ListViewSubItem(item, playlist.ChapterCount.ToString()) { Tag = playlist.ChapterCount },
                    new ListViewItem.ListViewSubItem(item, playlist.FileSize.ToString("n0")) { Tag = playlist.FileSize },
                    new ListViewItem.ListViewSubItem(item, playlist.Type.ToString()) { Tag = playlist.Type },
                    new ListViewItem.ListViewSubItem(item, playlist.Cut.ToString()) { Tag = playlist.Cut },
                    new ListViewItem.ListViewSubItem(item, playlist.FirstVideoLanguage.ToString()) { Tag = playlist.FirstVideoLanguage },
                    new ListViewItem.ListViewSubItem(item, playlist.Warnings) { Tag = playlist.Warnings },
                });

            if (IsBestChoice(playlist))
                MarkBestChoice(item);

            return item;
        }

        private static bool IsBestChoice(Playlist playlist)
        {
            return playlist.IsBestGuess;
        }

        private static void MarkBestChoice(ListViewItem item)
        {
            item.MarkBestChoice();
            item.AppendToolTip("Best choice based on your preferences");
        }

        public void ShowFilterWindow()
        {
            var result = new FormPlaylistFilter(_filter).ShowDialog(this);

            if (result == DialogResult.OK)
            {
                RefreshPlaylists();
            }
        }

        public void SelectFirstPlaylist()
        {
            if (!VisiblePlaylistsInSortOrder.Any())
                return;
            SelectedPlaylist = VisiblePlaylistsInSortOrder.First();
        }

        private void linkLabelShowFilterWindow_Click(object sender, EventArgs e)
        {
            ShowFilterWindow();
        }

        private void checkBoxShowAllPlaylists_CheckedChanged(object sender, EventArgs e)
        {
            _showAllPlaylists = checkBoxShowAllPlaylists.Checked;

            RefreshPlaylists();

            if (ShowAllChanged != null)
                ShowAllChanged(this, EventArgs.Empty);
        }

        private void RefreshPlaylists()
        {
            Playlists = Playlists;
        }

        public void ReconfigurePlaylist(Playlist playlist)
        {
            if (!Playlists.Contains(playlist))
                return;

            var newItem = Transform(playlist);

            var items = listView.Items.OfType<ListViewItem>().Where(curItem => curItem.Tag == newItem.Tag).ToArray();
            var oldItem = items.FirstOrDefault();
            if (oldItem == null)
                return;

            newItem.Selected = oldItem.Selected;

            // Replacing the item by inserting the new one and removing the old one
            // causes a stack overflow because it triggers a SelectedIndexChanged event, which then
            // calls this method, etc.  So we must resort to copying the new values into the old subitems.

            oldItem.Text = newItem.Text;
            oldItem.Tag = newItem.Tag;
            oldItem.ToolTipText = newItem.ToolTipText;
            oldItem.Font = newItem.Font;
            oldItem.BackColor = newItem.BackColor;
            oldItem.ForeColor = newItem.ForeColor;

            var oldSubItems = oldItem.SubItems.OfType<ListViewItem.ListViewSubItem>().ToArray();
            var newSubItems = newItem.SubItems.OfType<ListViewItem.ListViewSubItem>().ToArray();

            for (var i = 0; i < oldSubItems.Length; i++)
            {
                oldSubItems[i].Text = newSubItems[i].Text;
                oldSubItems[i].Tag = newSubItems[i].Tag;
                oldSubItems[i].Font = newSubItems[i].Font;
                oldSubItems[i].BackColor = newSubItems[i].BackColor;
                oldSubItems[i].ForeColor = newSubItems[i].ForeColor;
            }
        }
    }
}
