using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BDHeroCore.Services;
using DotNetUtils;

namespace BDAutoMuxer
{
    public partial class FormCoverArt : Form
    {
        private readonly ImageList coverArtImages = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(93, 139) };
            
        public int SelectedIndex { get; private set;}

        public FormCoverArt(List<string> posterList, int index, string language)
        {
            InitializeComponent();
            PopulateCoverArt(posterList, index);
            AcceptButton.DialogResult = DialogResult.OK;

            if (coverArtImages != null)
            {
                try
                {
                    Icon = Icon.FromHandle(((Bitmap)coverArtImages.Images[index]).GetHicon());
                }
                catch {}
            }

            if (string.IsNullOrEmpty(language))
            {
                Text = "Posters";
            }
            else
            {
                try
                {
                    var lang = new CultureInfo(language).DisplayName;
                    Text = lang + " Posters";
                }
                catch
                {
                }
            }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #region Private Methods

        private void PopulateCoverArt(List<string> posterList, int index)
        {
            for (var i = 0; i < posterList.Count; i++)
            {
                var posterUrl = posterList[i];
                var listViewItem = new ListViewItem("", i);

                listViewCovers.Items.Add(listViewItem);
                coverArtImages.Images.Add(HttpRequest.GetImage(posterUrl));
            }

            listViewCovers.LargeImageList = coverArtImages;
            listViewCovers.Items[index].Selected = true;

        }
      
        #endregion
        
        #region Event Handlers

        private void listViewCovers_DoubleClick(object sender, EventArgs e)
        {
            buttonSave.PerformClick();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (listViewCovers.SelectedIndices.Count > 0)
            {
                SelectedIndex = listViewCovers.SelectedIndices[0];
            }
            Close();
        }

        #endregion
    }
}
