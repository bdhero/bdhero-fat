using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.Views;
using BDAutoMuxerCore.Annotations;
using BDAutoMuxerCore.Services;
using BDAutoMuxerCore.Tools;
using DotNetUtils;

namespace CoverArtResizer
{
    public partial class FormResizer : Form
    {
        public FormResizer()
        {
            InitializeComponent();
            InitializeSelectAll();
            InitializeResizeButton();
            InitializePictureBoxes();
            InitializeDragDrop();
        }

        private void InitializeSelectAll()
        {
            FormUtils.TextBox_EnableSelectAll(this);
        }

        private void InitializeResizeButton()
        {
            buttonResize.Click += (sender, args) => ResizePoster();
        }

        private void InitializePictureBoxes()
        {
            draggableImageCover.ImagePath = null;
            draggableImageSmallCover.ImagePath = null;

            draggableImageCover.MouseEnter += DraggableImageOnMouseEnter;
            draggableImageSmallCover.MouseEnter += DraggableImageOnMouseEnter;

            draggableImageCover.MouseLeave += DraggableImageOnMouseLeave;
            draggableImageSmallCover.MouseLeave += DraggableImageOnMouseLeave;
        }

        private void DraggableImageOnMouseEnter(object sender, EventArgs eventArgs)
        {
            toolStripStatusLabel.Text = "";
            var draggableImage = sender as DraggableImage;
            if (draggableImage != null && !string.IsNullOrWhiteSpace(draggableImage.ImagePath) && File.Exists(draggableImage.ImagePath))
            {
                using (var image = FileUtils.ImageFromFile(draggableImage.ImagePath))
                {
                    toolStripStatusLabel.Text = string.Format("{0}x{1}", image.Width, image.Height);
                }
            }
        }

        private void DraggableImageOnMouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "";
        }

        private void InitializeDragDrop()
        {
            DragEnter += FormDragEnter;
            DragDrop += FormDragDrop;
            DragLeave += FormDragLeave;
        }

        private static bool AcceptDrop(DragEventArgs e)
        {
            var hasUnicodeText = e.Data.GetDataPresent(DataFormats.UnicodeText);
            var isExternalDragProvider = e.Data.GetDataPresent(typeof (ExternalDragProvider));
            Uri myUri;
            var isUri = Uri.TryCreate(DragUtils.GetUnicodeText(e), UriKind.RelativeOrAbsolute, out myUri);
            return hasUnicodeText && isUri && !isExternalDragProvider;
        }

        private void FormDragEnter(object sender, DragEventArgs e)
        {
            var accept = AcceptDrop(e);
            e.Effect = accept ? DragDropEffects.All : DragDropEffects.None;
            textBoxImageUrl.BorderStyle = accept ? BorderStyle.FixedSingle : BorderStyle.Fixed3D;
        }

        private void FormDragDrop(object sender, DragEventArgs e)
        {
            var url = DragUtils.GetUnicodeText(e);

            if (AcceptDrop(e) && url != null)
            {
                textBoxImageUrl.Text = url;
                ResizePoster();
            }

            textBoxImageUrl.BorderStyle = BorderStyle.Fixed3D;
        }

        private void FormDragLeave(object sender, EventArgs e)
        {
            textBoxImageUrl.BorderStyle = BorderStyle.Fixed3D;
        }

        private void ResizePoster()
        {
            buttonResize.Enabled = false;
            draggableImageCover.ImagePath = null;
            draggableImageSmallCover.ImagePath = null;

            try
            {
                var poster = HttpImageCache.Instance.GetImage(textBoxImageUrl.Text);

                string hash;
                using (MD5 md5Hash = MD5.Create())
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(textBoxImageUrl.Text);
                    hash = GetMd5Hash(md5Hash, fileNameWithoutExtension);
                }

                draggableImageCover.ImagePath = ResizeCoverArt(poster, CoverArtSize.Large, "cover.jpg", hash);
                draggableImageSmallCover.ImagePath = ResizeCoverArt(poster, CoverArtSize.Small, "small_cover.jpg", hash);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            buttonResize.Enabled = true;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        /// <summary>
        /// Resizes the cover art image to the appropriate dimensions and saves it to a temporary file with the given filename.
        /// </summary>
        /// <param name="image">Full size cover art image from TMDb.  If <paramref name="image"/> is null, this method will return null.</param>
        /// <param name="width">120 for small or 600 for large</param>
        /// <param name="filename">cover.{jpg,png} or small_cover.{jpg,png}</param>
        /// <returns>Full, absolute path to the resized image on disk if <paramref name="image"/> is not null; otherwise null.</returns>
        [NotNull]
        private string ResizeCoverArt([CanBeNull] Image image, CoverArtSize width, [NotNull] string filename, [CanBeNull] string subfolder)
        {
            if (image == null) return null;
            var ext = Path.GetExtension(filename.ToLowerInvariant());
            var format = ext == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
            var path = AssemblyUtils.GetTempFilePath(GetType(), filename, subfolder);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (var scaledImage = ScaleImage(image, (int) width, int.MaxValue))
            {
                scaledImage.Save(path, format);
            }
            return path;
        }

        [NotNull]
        private static Image ScaleImage([NotNull] Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
            }
            return newImage;
        }
    }
}
