﻿using System;
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

        private readonly TrackListViewHelper _helper;

        public VideoTrackListView()
        {
            InitializeComponent();
            _helper = new TrackListViewHelper(listViewVideoTracks, columnHeaderIndex.Index, track => track.IsVideo, GetListItem);
            Load += _helper.OnLoad;
        }

        private static ICollection<ListViewCell> GetListItem(Track track)
        {
            return new[]
                {
                    new ListViewCell { Text = track.Codec.CommonName },
                    new ListViewCell { Text = track.VideoFormatDisplayable },
                    new ListViewCell { Text = track.FrameRateDisplayable },
                    new ListViewCell { Text = track.AspectRatioDisplayable },
                    new ListViewCell { Text = track.IndexOfType.ToString("D"), Tag = track.IndexOfType }
                };
        }
    }
}
