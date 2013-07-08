using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHeroGUI.Forms;

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
            }
        }

        private Playlist _playlist;

        /// <summary>
        /// Gets or sets whether all playlists are shown, regardless of the user's filter settings.
        /// </summary>
        public bool ShowAll
        {
            get { return _showAllTracks; }
            set
            {
                _showAllTracks = value;
                RefreshPlaylist();
            }
        }

        private readonly TrackFilter _filter = new TrackFilter();

        private bool _showAllTracks;

        public TracksPanel()
        {
            InitializeComponent();

            videoTrackListView.Filter = ShowTrack;
            audioTrackListView.Filter = ShowTrack;
            subtitleTrackListView.Filter = ShowTrack;
        }

        public void ShowFilterWindow()
        {
            var result = new FormTrackFilter(_filter).ShowDialog(this);

            if (result == DialogResult.OK)
            {
                RefreshPlaylist();
            }
        }

        private bool ShowTrack(Track track)
        {
            return _filter.Show(track) || _showAllTracks;
        }

        private void RefreshPlaylist()
        {
            Playlist = Playlist;
        }
    }
}
