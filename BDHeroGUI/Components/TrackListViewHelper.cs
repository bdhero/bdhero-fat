using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;

namespace BDHeroGUI.Components
{
    class TrackListViewHelper
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;

                _listView.Items.Clear();

                if (_playlist == null) return;

                var items = Transform(_playlist.Tracks);
                _listView.Items.AddRange(items);
                _listView.AutoSizeColumns();
            }
        }

        private Playlist _playlist;

        private readonly ListView2 _listView;
        private readonly Func<Track, bool> _filter;
        private readonly Func<Track, ICollection<ListViewCell>> _transform;

        public TrackListViewHelper(ListView2 listView, Func<Track, bool> filter, Func<Track, ICollection<ListViewCell>> transform)
        {
            _listView = listView;
            _filter = filter;
            _transform = transform;

            _listView.MultiSelect = true;
            _listView.ItemChecked += ListViewOnItemChecked;
        }

        public void OnLoad(object sender = null, EventArgs eventArgs = null)
        {
            _listView.SetSortColumn(_listView.FirstDisplayedColumn.Index);
            _listView.AutoSizeColumns();
        }

        private static void ListViewOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var track = args.Item.Tag as Track;
            if (track != null)
            {
                track.Keep = args.Item.Checked;
            }
        }

        private ListViewItem[] Transform(IEnumerable<Track> tracks)
        {
            return tracks.Where(_filter).Select(delegate(Track track)
                {
                    var cells = _transform(track);
                    var firstCell = cells.First();
                    var subCells = cells.Skip(1);

                    var item = new ListViewItem(firstCell.Text)
                        {
                            Checked = track.Keep,
                            Tag = track
                        };

                    item.SubItems.AddRange(subCells.Select(cell => CreateListViewSubItem(item, cell)).ToArray());

                    return item;
                }).ToArray();
        }

        private static ListViewItem.ListViewSubItem CreateListViewSubItem(ListViewItem item, ListViewCell cell)
        {
            return new ListViewItem.ListViewSubItem(item, cell.Text) {Tag = cell.Tag};
        }
    }
}
