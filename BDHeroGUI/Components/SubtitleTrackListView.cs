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
    public partial class SubtitleTrackListView : UserControl
    {
        public Playlist Playlist
        {
            get { return _helper.Playlist; }
            set { _helper.Playlist = value; }
        }

        private readonly TrackListViewHelper _helper;

        public SubtitleTrackListView()
        {
            InitializeComponent();
            _helper = new TrackListViewHelper(listViewAudioTracks, track => track.IsSubtitle, GetListItem);
            Load += _helper.OnLoad;
        }

        private static ICollection<ListViewCell> GetListItem(Track track)
        {
            return new[]
                {
                    new ListViewCell { Text = track.Codec.CommonName },
                    new ListViewCell { Text = track.Type.ToString(), Tag = track.Type },
                    new ListViewCell { Text = track.IndexOfType.ToString("D"), Tag = track.IndexOfType }
                };
        }
    }
}
