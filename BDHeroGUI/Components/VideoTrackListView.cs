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
            get { return _helper.Playlist; }
            set { _helper.Playlist = value; }
        }

        public event PlaylistReconfiguredEventHandler PlaylistReconfigured;

        private readonly TrackListViewHelper _helper;

        public Func<Track, bool> Filter = track => true;

        public VideoTrackListView()
        {
            InitializeComponent();
            _helper = new TrackListViewHelper(listViewVideoTracks, track => track.IsVideo && Filter(track), GetListItem);
            Load += _helper.OnLoad;
            _helper.PlaylistReconfigured += HelperOnPlaylistReconfigured;
        }

        private void HelperOnPlaylistReconfigured(Playlist playlist)
        {
            if (PlaylistReconfigured != null)
                PlaylistReconfigured(playlist);
        }

        private static ICollection<ListViewCell> GetListItem(Track track)
        {
            return new[]
                {
                    new ListViewCell { Text = track.Codec.DisplayName },
                    new ListViewCell { Text = track.VideoFormatDisplayable },
                    new ListViewCell { Text = track.FrameRateDisplayable },
                    new ListViewCell { Text = track.AspectRatioDisplayable },
                    new ListViewCell { Text = track.Type.ToString(), Tag = track.Type },
                    new ListViewCell { Text = (track.IndexOfType + 1).ToString("D"), Tag = track.IndexOfType }
                };
        }
    }
}
