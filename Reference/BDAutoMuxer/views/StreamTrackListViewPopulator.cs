using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using BDAutoMuxer.Properties;
using BDHeroCore;
using BDHeroCore.BDInfo;

namespace BDAutoMuxer.Views
{
    static class StreamTrackListViewPopulator
    {

        public static void Populate(
            TSPlaylistFile playlist,
            ListView streams,
            ListView tracks
            )
        {
            streams.Items.Clear();
            tracks.Items.Clear();

            var clipCount = 0;

            foreach (var clip in playlist.StreamClips)
            {
                if (clip.AngleIndex == 0)
                {
                    ++clipCount;
                }

                var clipIndex = new ListViewItem.ListViewSubItem {Text = clipCount.ToString(CultureInfo.InvariantCulture), Tag = clipCount};
                var clipName = new ListViewItem.ListViewSubItem {Text = clip.DisplayName, Tag = clip.Name};

                if (clip.AngleIndex > 0)
                {
                    clipName.Text += string.Format(
                        " ({0})", clip.AngleIndex);
                }

                var clipLengthSpan = new TimeSpan((long)(clip.Length * 10000000));
                var clipLength =
                    new ListViewItem.ListViewSubItem
                        {
                            Text = string.Format(
                                "{0:D2}:{1:D2}:{2:D2}",
                                clipLengthSpan.Hours,
                                clipLengthSpan.Minutes,
                                clipLengthSpan.Seconds),
                            Tag = clip.Length
                        };

                var clipSize = new ListViewItem.ListViewSubItem();

                if (BDAutoMuxerSettings.EnableSSIF &&
                    clip.InterleavedFileSize > 0)
                {
                    clipSize.Text = clip.InterleavedFileSize.ToString("N0");
                    clipSize.Tag = clip.InterleavedFileSize;
                }
                else if (clip.FileSize > 0)
                {
                    clipSize.Text = clip.FileSize.ToString("N0");
                    clipSize.Tag = clip.FileSize;
                }
                else
                {
                    clipSize.Text = "-";
                    clipSize.Tag = clip.FileSize;
                }

                var streamFileSubItems =
                    new[]
                    {
                        clipName,
                        clipIndex,
                        clipLength,
                        clipSize
                    };

                streams.Items.Add(new ListViewItem(streamFileSubItems, 0) {Tag = clip});
            }

            var i = 0;
            ImageList trackIcons = new ImageList { ColorDepth = ColorDepth.Depth32Bit };

            foreach (var stream in playlist.SortedStreams)
            {
                var codec = new ListViewItem.ListViewSubItem {Text = stream.CodecName};
                if (stream.AngleIndex > 0)
                {
                    codec.Text += string.Format(
                        " ({0})", stream.AngleIndex);
                }
                codec.Tag = stream.CodecName;

                // ReSharper disable LocalizableElement
                if (stream.StreamType == TSStreamType.DTS_HD_SECONDARY_AUDIO || stream.StreamType == TSStreamType.INTERACTIVE_GRAPHICS)
                {
                    codec.Text = "** " + codec.Text;
                    codec.ForeColor = SystemColors.GrayText;
                }
                else if (stream.IsHidden)
                {
                    codec.Text = "* " + codec.Text;
                    codec.ForeColor = SystemColors.GrayText;
                }
                // ReSharper restore LocalizableElement

                var language = new ListViewItem.ListViewSubItem {Text = stream.LanguageName, Tag = stream.LanguageName};
                var bitrate = new ListViewItem.ListViewSubItem();

                if (stream.AngleIndex > 0)
                {
                    if (stream.ActiveBitRate > 0)
                    {
                        bitrate.Text = string.Format(
                            "{0} kbps", Math.Round((double)stream.ActiveBitRate / 1000));
                    }
                    else
                    {
// ReSharper disable LocalizableElement
                        bitrate.Text = "-";
// ReSharper restore LocalizableElement
                    }
                    bitrate.Tag = stream.ActiveBitRate;
                }
                else
                {
                    if (stream.BitRate > 0)
                    {
                        bitrate.Text = string.Format(
                            "{0} kbps", Math.Round((double)stream.BitRate / 1000));
                    }
                    else
                    {
// ReSharper disable LocalizableElement
                        bitrate.Text = "-";
// ReSharper restore LocalizableElement
                    }
                    bitrate.Tag = stream.BitRate;
                }

                var description = new ListViewItem.ListViewSubItem {Text = stream.Description, Tag = stream.Description};

                var streamSubItems =
                    new[]
                    {
                        codec,
                        language,
                        bitrate,
                        description
                    };

                trackIcons.Images.Add(TSStream.GetCodecIcon(stream.StreamType));

                var listViewItem = new ListViewItem(streamSubItems, 0) {Tag = stream.PID, ImageIndex = i++};

                if (stream.IsHidden)
                    listViewItem.ForeColor = SystemColors.GrayText;

                tracks.Items.Add(listViewItem);
            }

            tracks.SmallImageList = trackIcons;

            AddCodecReferenceContextMenu(tracks, playlist.SortedStreams);
        }

        private static void AddCodecReferenceContextMenu(ListView listView, List<TSStream> streams)
        {
            AddCodecReferenceContextMenu(listView, () => streams);
        }

        public static void AddCodecReferenceContextMenu(ListView listView, Func<List<TSStream>> streamsGetter)
        {
            listView.ContextMenuStrip = new ContextMenuStrip();
            listView.ContextMenuStrip.Items.Add("View codec information", Resources.info,
                                                (sender, args) =>
                                                    {
                                                        var streams = streamsGetter();
                                                        if (streams != null)
                                                            ViewCodecInformation(listView, streams);
                                                    }
                                                );
            listView.ContextMenuStrip.Opening += (sender, args) =>
                                                     {
                                                         if (listView.SelectedItems.Count == 0)
                                                             args.Cancel = true;
                                                     };
        }

        public static void ViewCodecInformation(ListView listViewTracks, IList<TSStream> streams)
        {
            var selectedTrack = listViewTracks.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (selectedTrack == null) return;
            var stream = streams[selectedTrack.Index];
            FormCodecReference.ShowReference(MediaInfoHelper.CodecFromStream(stream));
        }
    }
}
