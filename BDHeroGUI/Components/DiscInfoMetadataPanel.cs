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
    public partial class DiscInfoMetadataPanel : UserControl
    {
        private const string NotFound = "(not found)";

        public DiscInfoMetadataPanel()
        {
            InitializeComponent();
        }

        public void SetMetadata(DiscMetadata metadata)
        {
            var raw = metadata.Raw;
            var der = metadata.Derived;

            textBoxHardwareVolumeLabel.Text = raw.HardwareVolumeLabel;
            textBoxAnyDVDDiscInf.Text = raw.DiscInf != null ? raw.DiscInf.ToString() : NotFound;
            textBoxDboxTitle.Text = raw.DboxTitle ?? NotFound;
            textBoxVISAN.Text = raw.V_ISAN != null ? raw.V_ISAN.ToString() : NotFound;
            textBoxAllBdmtTitles.Text = string.Join(Environment.NewLine, raw.AllBdmtTitles.Select(pair => string.Format("{0}: {1}", pair.Key, pair.Value)));

            textBoxVolumeLabel.Text = der.VolumeLabel;
            textBoxVolumeLabelSanitized.Text = der.VolumeLabelSanitized;
            textBoxDboxTitleSanitized.Text = der.DboxTitleSanitized ?? NotFound;
            textBoxIsan.Text = der.ISAN != null ? der.ISAN.ToString() : NotFound;
            textBoxValidBdmtTitles.Text = string.Join(Environment.NewLine, der.ValidBdmtTitles.Select(pair => string.Format("{0}: {1}", pair.Key, pair.Value)));
        }
    }
}
