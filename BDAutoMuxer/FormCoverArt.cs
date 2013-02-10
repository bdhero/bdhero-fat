using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BDAutoMuxerCore.Services;

namespace BDAutoMuxer
{
    public partial class FormCoverArt : Form
    {
        private readonly HttpImageCache _imageCache = HttpImageCache.Instance;
        public int SelectedIndex { get; private set;}

        public FormCoverArt(List<string> posterList, int index, string language)
        {
            InitializeComponent();
            PopulateCoverArt(posterList, index);
            AcceptButton.DialogResult = DialogResult.OK;
            

            if (string.IsNullOrEmpty(language))
            {
                this.Text = "Posters";
            }
            else
            {
                try
                {
                    var lang = new CultureInfo(language).DisplayName;
                    this.Text = lang + " Posters";
                }
                catch
                {
                }
            }
        }

        #region Private Methods

        private void PopulateCoverArt(List<string> posterList, int index)
        {

            var coverArtImages = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(93, 139) };
            
            for (var i = 0; i < posterList.Count; i++)
            {
                var posterUrl = posterList[i];
                var listViewItem = new ListViewItem("", i);

                listViewCovers.Items.Add(listViewItem);
                coverArtImages.Images.Add(_imageCache.GetImage(posterUrl));
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
