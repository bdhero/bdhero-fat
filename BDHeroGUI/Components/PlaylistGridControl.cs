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
using DotNetUtils;
using DotNetUtils.Controls;
using WindowsOSUtils.FormUtils;

namespace BDHeroGUI.Components
{
    public partial class PlaylistGridControl : UserControl
    {
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

        public Playlist SelectedPlaylist
        {
            get { return listView.SelectedItems.Count > 0 ? listView.SelectedItems[0].Tag as Playlist : null; }
            set
            {
                foreach (var item in listView.Items.OfType<ListViewItem>().Where(item => item.Tag == value))
                {
                    item.Selected = true;
                }
                if (listView.SelectedItems.Count == 0 && listView.Items.Count > 0)
                {
                    listView.Items[0].Selected = true;
                }
            }
        }

        private IList<Playlist> _playlists;

        private readonly ListViewColumnSorter _columnSorter = new ListViewColumnSorter();

        public Func<Playlist, bool> Filter;

        public PlaylistGridControl()
        {
            InitializeComponent();
            Load += OnLoad;

            listView.ListViewItemSorter = _columnSorter;
            listView.ColumnClick += (_, e) => ListViewOnColumnClick(e.Column);
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            DoubleBuffered = true;
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
