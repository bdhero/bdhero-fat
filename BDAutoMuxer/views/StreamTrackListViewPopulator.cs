using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BDAutoMuxer.views
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
// ReSharper disable LocalizableElement
                    clipSize.Text = "-";
// ReSharper restore LocalizableElement
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
            ImageList trackIcons = new ImageList();

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

                // TODO: Handle unknown types (icon cannot be null)
                trackIcons.Images.Add(CodecIcon(stream.StreamType));

                tracks.Items.Add(new ListViewItem(streamSubItems, 0) {Tag = stream.PID, ImageIndex = i++});
            }

            tracks.SmallImageList = trackIcons;
        }

        private static Bitmap CodecIcon(TSStreamType streamType)
        {
            switch (streamType)
            {
                case TSStreamType.AC3_AUDIO:
                    return Properties.Resources.dd;
                case TSStreamType.AC3_PLUS_AUDIO:
                    return Properties.Resources.dd_plus;
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                    return Properties.Resources.dd_plus;
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                    return Properties.Resources.truehd;
                case TSStreamType.DTS_AUDIO:
                    return Properties.Resources.dts;
                case TSStreamType.DTS_HD_AUDIO:
                    return Properties.Resources.dts_hd;
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                    return Properties.Resources.dts_hd;
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                    return Properties.Resources.dts_hd;
                case TSStreamType.LPCM_AUDIO:
                    return Properties.Resources.lpcm;
                case TSStreamType.MPEG1_AUDIO:
                    return Properties.Resources.mpeg1_audio;
                case TSStreamType.MPEG2_AUDIO:
                    return Properties.Resources.mpeg2_audio;

                case TSStreamType.AVC_VIDEO:
                    return Properties.Resources.avc;
                case TSStreamType.MPEG1_VIDEO:
                    return Properties.Resources.mpeg1_video;
                case TSStreamType.MPEG2_VIDEO:
                    return Properties.Resources.mpeg2_video;

                case TSStreamType.PRESENTATION_GRAPHICS:
                    return Properties.Resources.pgs;
                case TSStreamType.SUBTITLE:
                    return Properties.Resources.txt;

                default:
                    return null;
            }
        }
    }
}
