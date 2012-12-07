using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.models;

namespace BDAutoMuxer
{
    public partial class FormCodecs : Form
    {
        public FormCodecs()
        {
            InitializeComponent();

            var groups = listViewCodecs.Groups;

            AddCodecs(groups[0], MICodec.VideoCodecs.Select(c => c as MICodec).ToList());
            AddCodecs(groups[1], MICodec.AudioCodecs.Select(c => c as MICodec).ToList());
            AddCodecs(groups[2], MICodec.SubtitleCodecs.Select(c => c as MICodec).ToList());

            columnHeaderCommonName.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            columnHeaderOfficialName.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            columnHeaderAlternateNames.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            columnHeaderDescription.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void AddCodecs(ListViewGroup group, IEnumerable<MICodec> codecs)
        {
            foreach (var codec in codecs)
            {
                var subitems = new[]
                                   {
                                       new ListViewItem.ListViewSubItem { Text = codec.ShortName },
                                       new ListViewItem.ListViewSubItem { Text = codec.FullName },
                                       new ListViewItem.ListViewSubItem { Text = codec.MicroName },
                                       new ListViewItem.ListViewSubItem { Text = codec.Description }
                                   };
                listViewCodecs.Items.Add(new ListViewItem(subitems, 0) { Tag = codec });
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
