using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;

namespace BDHeroGUI.Components
{
    public partial class VideoTrackListView : UserControl
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;

                listViewVideoTracks.Items.Clear();

                if (_playlist == null) return;

                var items = Transform(_playlist.VideoTracks);
                listViewVideoTracks.Items.AddRange(items);
                AutoSizeColumns();
            }
        }

        private Playlist _playlist;

        private ListViewItem[] Transform(IList<Track> videoTracks)
        {
            return videoTracks.Select(delegate(Track track, int i)
                {
                    var item = new ListViewItem(track.Codec.CommonName)
                        {
                            Checked = track.Keep,
                            Tag = track
                        };
                    item.SubItems.AddRange(new[]
                        {
                            new ListViewItem.ListViewSubItem(item, track.VideoFormatDisplayable),
                            new ListViewItem.ListViewSubItem(item, track.FrameRateDisplayable),
                            new ListViewItem.ListViewSubItem(item, track.AspectRatioDisplayable),
                            new ListViewItem.ListViewSubItem(item, track.IndexOfType.ToString("D")) { Tag = track.IndexOfType }
                        });
                    return item;
                }).ToArray();
        }

        public VideoTrackListView()
        {
            InitializeComponent();
            Load += OnLoad;
            listViewVideoTracks.ItemChecked += ListViewVideoTracksOnItemChecked;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            listViewVideoTracks.SetSortColumn(columnHeaderIndex.Index);
        }

        private static void ListViewVideoTracksOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var track = args.Item.Tag as Track;
            if (track != null)
            {
                track.Keep = args.Item.Checked;
            }
        }

        public void AutoSizeColumns()
        {
            listViewVideoTracks.AutoSizeColumns();
        }
    }
}
