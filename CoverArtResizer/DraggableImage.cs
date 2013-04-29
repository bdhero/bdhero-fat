using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.Views;

namespace CoverArtResizer
{
    public partial class DraggableImage : UserControl
    {
        private ExternalDragProvider _dragProvider;

        [DefaultValue(null)]
        public String ImagePath
        {
            get { return pictureBox.ImageLocation; }
            set
            {
                label.Text = Path.GetFileName(value ?? "") ?? "";
                if (!string.IsNullOrWhiteSpace(value) && File.Exists(value))
                {   
                    pictureBox.ImageLocation = value;
                    contextMenuStrip.Enabled = true;
                    InitializeDrag();
                }
                else
                {
                    pictureBox.Image = null;
                    contextMenuStrip.Enabled = false;
                    DestroyDrag();
                }
                OnResize(EventArgs.Empty);
            }
        }

        public DraggableImage()
        {
            InitializeComponent();
        }

        private void InitializeDrag()
        {
            _dragProvider = new ExternalDragProvider(this)
            {
                PathGetter = control => ImagePath,
                Threshold = 5
            };
        }

        private void DestroyDrag()
        {
            if (_dragProvider != null)
                _dragProvider.Destroy();
        }

        private void toolStripMenuItemCopyPath_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ImagePath);
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItemCopyPath.Enabled = !string.IsNullOrWhiteSpace(ImagePath);
        }
    }
}
