using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
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
        /// Function that determines the visibility of playlists within the ListView.
        /// </summary>
        public Func<Playlist, bool> Filter;

        /// <summary>
        /// Triggered whenever the user selects a new playlist or deselects the current playlist.
        /// </summary>
        public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged;

        private IList<Playlist> _playlists;

        private readonly ListViewColumnSorter _columnSorter = new ListViewColumnSorter();

        public PlaylistListView()
        {
            InitializeComponent();
            Load += OnLoad;

            listView.ListViewItemSorter = _columnSorter;
            listView.ColumnClick += (_, e) => ListViewOnColumnClick(e.Column);
            listView.ItemSelectionChanged += delegate(object sender, ListViewItemSelectionChangedEventArgs args)
                {
                    if (ItemSelectionChanged != null)
                        ItemSelectionChanged(sender, args);
                };

            // Automatically resize the last column to take up all remaining free space
            Resize += (sender, args) => listView.AutoSizeLastColumn();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            listView.SetDoubleBuffered(true);
            ListViewOnColumnClick(columnHeaderType.Index);
        }

        private void ListViewOnColumnClick(int columnIndex)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (columnIndex == _columnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                _columnSorter.Order = _columnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _columnSorter.SortColumn = columnIndex;
                _columnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listView.Sort();
            listView.SetSortIcon(_columnSorter.SortColumn, _columnSorter.Order);
        }

        private ListViewItem[] Transform(IEnumerable<Playlist> playlists)
        {
            return playlists.Where(ShowPlaylist).Select(Transform).ToArray();
        }

        private bool ShowPlaylist(Playlist playlist)
        {
            return Filter == null || Filter(playlist);
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
            return item;
        }
    }
}
