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
    public partial class AudioTrackListView : UserControl
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;

                listViewAudioTracks.Items.Clear();

                if (_playlist == null) return;

                var items = Transform(_playlist.AudioTracks);
                listViewAudioTracks.Items.AddRange(items);
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
                            new ListViewItem.ListViewSubItem(item, track.ChannelCount.ToString("F1")) { Tag = track.ChannelCount }, 
                            new ListViewItem.ListViewSubItem(item, track.Language.Name) { Tag = track.Language.Name },
                            new ListViewItem.ListViewSubItem(item, track.IndexOfType.ToString("D")) { Tag = track.IndexOfType }
                        });
                    return item;
                }).ToArray();
        }

        public AudioTrackListView()
        {
            InitializeComponent();
            Load += OnLoad;
            listViewAudioTracks.ItemChecked += ListViewAudioTracksOnItemChecked;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            listViewAudioTracks.SetSortColumn(columnHeaderIndex.Index);
        }

        private static void ListViewAudioTracksOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var track = args.Item.Tag as Track;
            if (track != null)
            {
                track.Keep = args.Item.Checked;
            }
        }

        public void AutoSizeColumns()
        {
            listViewAudioTracks.AutoSizeColumns();
        }
    }
}
