using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.models;
using BDAutoMuxer.views;

namespace BDAutoMuxer
{
    public partial class FormCodecReference : Form
    {
        public FormCodecReference()
        {
            InitializeComponent();

            AddCodecs(MICodec.VideoCodecs.Select(c => c as MICodec).ToList());
            AddCodecs(MICodec.AudioCodecs.Select(c => c as MICodec).ToList());
            AddCodecs(MICodec.SubtitleCodecs.Select(c => c as MICodec).ToList());

            columnHeaderCommonName.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            columnHeaderOfficialName.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            columnHeaderAlternateNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            columnHeaderCodecId.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

            // Auto-select the first codec
            listViewCodecs.Items[0].Selected = true;

            FormUtils.TextBox_EnableSelectAll(this);
        }

        private void AddCodecs(IEnumerable<MICodec> codecs)
        {
            foreach (var codec in codecs)
            {
                var videoCodec = codec as MIVideoCodec;
                var audioCodec = codec as MIAudioCodec;

                var core = audioCodec != null && audioCodec.Core != null ? audioCodec.Core.CommonName : null;
                var compression = audioCodec != null ? (audioCodec.Lossless ? "Lossless" : "Lossy") : (codec.IsVideo ? "Lossy" : "N/A");
                var altDisplayNames = string.Join(",  ", new HashSet<string>(codec.AltDisplayNames));
                var isMuxable = codec.IsMuxable ? "yes" : "NO";

                var subitems = new[]
                                   {
                                       new ListViewItem.ListViewSubItem { Text = codec.CommonName },
                                       new ListViewItem.ListViewSubItem { Text = codec.FullName },
                                       new ListViewItem.ListViewSubItem { Text = altDisplayNames },
                                       new ListViewItem.ListViewSubItem { Text = codec.CodecId },
                                       new ListViewItem.ListViewSubItem { Text = core },
                                       new ListViewItem.ListViewSubItem { Text = compression },
                                       new ListViewItem.ListViewSubItem { Text = isMuxable }
                                   };
                listViewCodecs.Items.Add(new ListViewItem(subitems, 0) { Tag = codec });
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewCodecs_SelectedIndexChanged(object sender = null, EventArgs e = null)
        {
            textBoxDescription.Text = null;
            labelOfficialBlurayValue.Text = "N/A";
            labelOfficialDVDValue.Text = "N/A";

            var item = listViewCodecs.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (item == null) return;

            var codec = item.Tag as MICodec;
            if (codec == null) return;

            textBoxDescription.Text = codec.Description;
            labelOfficialBlurayValue.Text = codec.IsOfficialBlurayCodec
                                                ? string.Format("Yes ({0})",
                                                                codec.IsRequiredBlurayCodec ? "required" : "optional")
                                                : "No";
            labelOfficialDVDValue.Text = codec.IsOfficialDVDCodec
                                             ? string.Format("Yes ({0})",
                                                             codec.IsRequiredDVDCodec ? "required" : "optional")
                                             : "No";
        }
    }
}
