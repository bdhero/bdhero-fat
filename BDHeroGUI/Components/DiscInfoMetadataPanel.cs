using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using I18N;

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
            textBoxVISAN.Text = GetIsanText(raw.V_ISAN);
            textBoxAllBdmtTitles.Text = GetBdmtTitles(raw.AllBdmtTitles);

            textBoxVolumeLabel.Text = der.VolumeLabel;
            textBoxVolumeLabelSanitized.Text = der.VolumeLabelSanitized;
            textBoxDboxTitleSanitized.Text = der.DboxTitleSanitized ?? NotFound;
            textBoxIsan.Text = GetIsanText(raw.ISAN);
            textBoxValidBdmtTitles.Text = GetBdmtTitles(der.ValidBdmtTitles);
        }

        private static string GetIsanText(Isan isan)
        {
            if (isan == null)
                return NotFound;
            var lines = new List<string>();
            lines.Add(isan.IsSearchable ? "Valid:" : "Invalid:");
            lines.Add(isan.NumberFormatted);
            if (!string.IsNullOrWhiteSpace(isan.Title))
                lines.Add(string.Format("{0} ({1} - {2} min)", isan.Title, isan.Year, isan.LengthMin));
            return string.Join(Environment.NewLine, lines);
        }

        private static string GetBdmtTitles(IDictionary<Language, string> bdmtTitles)
        {
            return !bdmtTitles.Any()
                       ? NotFound
                       : string.Join(Environment.NewLine,
                                     bdmtTitles.Select(pair => string.Format("{0}: {1}", pair.Key.ISO_639_2, pair.Value)));
        }
    }
}
