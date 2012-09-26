using System;
using System.Globalization;
using System.Windows.Forms;

namespace BDAutoMuxer.views
{
    class StreamTrackListViewPopulator
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
// ReSharper disable LocalizableElement
                    clipSize.Text = "-";
// ReSharper restore LocalizableElement
                    clipSize.Tag = clip.FileSize;
                }

                var clipSize2 = new ListViewItem.ListViewSubItem {Text = clip.PacketSize > 0 ? clip.PacketSize.ToString("N0") : "-", Tag = clip.PacketSize};

                var streamFileSubItems =
                    new[]
                    {
                        clipName,
                        clipIndex,
                        clipLength,
                        clipSize,
                        clipSize2
                    };

                streams.Items.Add(new ListViewItem(streamFileSubItems, 0) {Tag = clip});
            }

            foreach (var stream in playlist.SortedStreams)
            {
                var codec = new ListViewItem.ListViewSubItem {Text = stream.CodecName};
                if (stream.AngleIndex > 0)
                {
                    codec.Text += string.Format(
                        " ({0})", stream.AngleIndex);
                }
                codec.Tag = stream.CodecName;

                if (stream.IsHidden)
                {
// ReSharper disable LocalizableElement
                    codec.Text = "* " + codec.Text;
// ReSharper restore LocalizableElement
                }

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

                tracks.Items.Add(new ListViewItem(streamSubItems, 0) {Tag = stream.PID});
            }
        }
    }
}
