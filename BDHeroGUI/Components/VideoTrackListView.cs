using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;

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
            }
        }

        private Playlist _playlist;

        private ListViewItem[] Transform(IList<Track> videoTracks)
        {
            return videoTracks.Select(delegate(Track track, int i)
                {
                    var item = new ListViewItem(track.Codec.CommonName)
                        {
                            Checked = i == 0,
                            Tag = track
                        };
                    item.SubItems.AddRange(new[]
                        {
                            track.VideoFormatDisplayable,
                            track.FrameRateDisplayable,
                            track.AspectRatioDisplayable
                        });
                    return item;
                }).ToArray();
        }

        public VideoTrackListView()
        {
            InitializeComponent();
        }
    }
}
