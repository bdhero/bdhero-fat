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
            }
        }

        private Playlist _playlist;

        public TracksPanel()
        {
            InitializeComponent();
        }
    }
}
