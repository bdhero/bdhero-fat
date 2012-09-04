using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDInfo.views
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

            int clipCount = 0;
            foreach (TSStreamClip clip in playlist.StreamClips)
            {
                if (clip.AngleIndex == 0)
                {
                    ++clipCount;
                }

                ListViewItem.ListViewSubItem clipIndex =
                    new ListViewItem.ListViewSubItem();
                clipIndex.Text = clipCount.ToString();
                clipIndex.Tag = clipCount;

                ListViewItem.ListViewSubItem clipName =
                    new ListViewItem.ListViewSubItem();
                clipName.Text = clip.DisplayName;
                clipName.Tag = clip.Name;
                if (clip.AngleIndex > 0)
                {
                    clipName.Text += string.Format(
                        " ({0})", clip.AngleIndex);
                }

                TimeSpan clipLengthSpan =
                    new TimeSpan((long)(clip.Length * 10000000));

                ListViewItem.ListViewSubItem clipLength =
                    new ListViewItem.ListViewSubItem();
                clipLength.Text = string.Format(
                    "{0:D2}:{1:D2}:{2:D2}",
                    clipLengthSpan.Hours,
                    clipLengthSpan.Minutes,
                    clipLengthSpan.Seconds);
                clipLength.Tag = clip.Length;

                ListViewItem.ListViewSubItem clipSize =
                    new ListViewItem.ListViewSubItem();
                if (BDInfoSettings.EnableSSIF &&
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

                ListViewItem.ListViewSubItem clipSize2 =
                    new ListViewItem.ListViewSubItem();
                if (clip.PacketSize > 0)
                {
                    clipSize2.Text = clip.PacketSize.ToString("N0");
                }
                else
                {
                    clipSize2.Text = "-";
                }
                clipSize2.Tag = clip.PacketSize;

                ListViewItem.ListViewSubItem[] streamFileSubItems =
                    new ListViewItem.ListViewSubItem[]
                    {
                        clipName,
                        clipIndex,
                        clipLength,
                        clipSize,
                        clipSize2
                    };

                ListViewItem streamFileItem =
                    new ListViewItem(streamFileSubItems, 0);
                streams.Items.Add(streamFileItem);
            }

            foreach (TSStream stream in playlist.SortedStreams)
            {
                ListViewItem.ListViewSubItem codec =
                    new ListViewItem.ListViewSubItem();
                codec.Text = stream.CodecName;
                if (stream.AngleIndex > 0)
                {
                    codec.Text += string.Format(
                        " ({0})", stream.AngleIndex);
                }
                codec.Tag = stream.CodecName;

                if (stream.IsHidden)
                {
                    codec.Text = "* " + codec.Text;
                }

                ListViewItem.ListViewSubItem language =
                    new ListViewItem.ListViewSubItem();
                language.Text = stream.LanguageName;
                language.Tag = stream.LanguageName;

                ListViewItem.ListViewSubItem bitrate =
                    new ListViewItem.ListViewSubItem();

                if (stream.AngleIndex > 0)
                {
                    if (stream.ActiveBitRate > 0)
                    {
                        bitrate.Text = string.Format(
                            "{0} kbps", Math.Round((double)stream.ActiveBitRate / 1000));
                    }
                    else
                    {
                        bitrate.Text = "-";
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
                        bitrate.Text = "-";
                    }
                    bitrate.Tag = stream.BitRate;
                }

                ListViewItem.ListViewSubItem description =
                    new ListViewItem.ListViewSubItem();
                description.Text = stream.Description;
                description.Tag = stream.Description;

                ListViewItem.ListViewSubItem[] streamSubItems =
                    new ListViewItem.ListViewSubItem[]
                    {
                        codec,
                        language,
                        bitrate,
                        description
                    };

                ListViewItem streamItem =
                    new ListViewItem(streamSubItems, 0);
                streamItem.Tag = stream.PID;
                tracks.Items.Add(streamItem);
            }
        }
    }
}
