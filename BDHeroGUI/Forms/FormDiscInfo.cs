﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;

namespace BDHeroGUI.Forms
{
    public partial class FormDiscInfo : Form
    {
        public FormDiscInfo(Disc disc)
        {
            InitializeComponent();

            var fs = disc.FileSystem;
            var metadata = disc.Metadata;
            var features = disc.Features;

            labelQuickSummary.Text = string.Format("{0} {1}",
                                                   metadata.Derived.VolumeLabel,
                                                   fs.Directories.Root.FullName
                );

            discInfoMetadataPanel.SetMetadata(metadata);
            discInfoFeaturesPanel.SetFeatures(features);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}