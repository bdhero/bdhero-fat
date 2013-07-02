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
    public partial class TracksPanel : UserControl
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;
                videoTrackListView.Playlist = _playlist;
                audioTrackListView.Playlist = _playlist;
                subtitleTrackListView.Playlist = _playlist;
                AutoResizeSplitContainers();
            }
        }

        private Playlist _playlist;

        public TracksPanel()
        {
            InitializeComponent();
        }

        private void AutoResizeSplitContainers()
        {
            if (Playlist == null)
                return;

            var numTracks = (double)Playlist.Tracks.Count;
            var pctVideo = Playlist.VideoTracks.Count / numTracks;
            var pctAudio = Playlist.AudioTracks.Count / numTracks;
            var pctSub = Playlist.SubtitleTracks.Count / numTracks;

            var height = splitContainerOuter.Height - (splitContainerOuter.SplitterWidth + splitContainerInner.SplitterWidth);
            var outerDistance = Math.Max(pctVideo * height, 65);
            var innerDistance = Math.Max(pctAudio * height, 80);

            splitContainerOuter.SplitterDistance = (int) outerDistance;
            splitContainerInner.SplitterDistance = (int) innerDistance;
        }
    }
}
