using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using System.Text.RegularExpressions;
using System.IO;
using BDAutoMuxer.tools;
using BDAutoMuxer.views;

namespace BDAutoMuxer
{
    public partial class FormRemux : Form
    {
        private MkvMerge mkvMerge;

        public FormRemux()
        {
            InitializeComponent();
            FormUtils.TextBox_EnableSelectAll(this);
        }

        private void FormRemux_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(0, Size.Height);
            this.MaximumSize = new Size(Screen.FromHandle(Handle).Bounds.Width, Size.Height);
        }

        private void FormRemux_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && DragUtils.HasFileExtension(e, new string[] { "m2ts", "mkv", "txt", "xml" }))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FormRemux_DragDrop(object sender, DragEventArgs e)
        {
            if (DragUtils.HasFileExtension(e, "m2ts"))
                textBoxInputM2ts.Text = DragUtils.GetFirstFileWithExtension(e, "m2ts");
            if (DragUtils.HasFileExtension(e, "mkv"))
                textBoxInputMkv.Text = DragUtils.GetFirstFileWithExtension(e, "mkv");
            if (DragUtils.HasFileExtension(e, new string[] { "txt", "xml" }))
                textBoxInputChapters.Text = DragUtils.GetFirstFileWithExtension(e, new string[] { "txt", "xml" });
        }

        /// <see cref="http://stackoverflow.com/a/2140908/467582"/>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case 0x84: //WM_NCHITTEST
                    var result = (HitTest)m.Result.ToInt32();
                    if (result == HitTest.Top || result == HitTest.Bottom)
                        m.Result = new IntPtr((int)HitTest.Caption);
                    if (result == HitTest.TopLeft || result == HitTest.BottomLeft)
                        m.Result = new IntPtr((int)HitTest.Left);
                    if (result == HitTest.TopRight || result == HitTest.BottomRight)
                        m.Result = new IntPtr((int)HitTest.Right);

                    break;
            }
        }

        enum HitTest
        {
            Caption = 2,
            Transparent = -1,
            Nowhere = 0,
            Client = 1,
            Left = 10,
            Right = 11,
            Top = 12,
            TopLeft = 13,
            TopRight = 14,
            Bottom = 15,
            BottomLeft = 16,
            BottomRight = 17,
            Border = 18
        }

        private void buttonInputM2tsBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxInputM2ts, "Select an Input M2TS File:", "BDAV Transport Stream", "m2ts");
        }

        private void buttonInputMkvBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxInputMkv, "Select an Input MKV File:", "Matroska Video", "mkv");
        }

        private void buttonInputChaptersBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxInputChapters, "Select an Input Chapter File:", "Matroska XML or OGM text", "txt;xml");
        }

        private void buttonOutputMkvBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile(textBoxOutputMkv, "Select an Output MKV File:", "Matroska Video", "mkv", false);
        }

        private void BrowseFile(TextBox textBox, string title, string fileTypeName, string fileExt, bool checkFileExists = true)
        {
            string path = null;
            try
            {
                // Normalize "ext", ".ext", and "*.ext" to "*.ext"
                fileExt = Regex.Replace(fileExt.Trim(), @"\*?\.?(\w+)", "*.$1");

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = string.Format("{0} ({1})|{1}", fileTypeName, fileExt);
                dialog.Title = title;
                dialog.CheckFileExists = checkFileExists;

                string cur = textBox.Text;
                if (!string.IsNullOrEmpty(cur))
                {
                    dialog.FileName = cur;
                }
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.FileName;
                    textBox.Text = path;
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Error opening path {0}: {1}{2}",
                    path,
                    ex.Message,
                    Environment.NewLine);

                MessageBox.Show(msg, "BDAutoMuxer Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRemux_Click(object sender, EventArgs e)
        {
            buttonRemux.Text = "Pause";
            buttonClose.Text = "Stop";

            if (mkvMerge != null && mkvMerge.IsBusy)
            {
                if (mkvMerge.IsPaused)
                    mkvMerge.Resume();
                else
                {
                    mkvMerge.Pause();
                    buttonRemux.Text = "Resume";
                }
            }
            else if(
                File.Exists(textBoxInputM2ts.Text) &&
                File.Exists(textBoxInputMkv.Text) &&
                File.Exists(textBoxInputChapters.Text))
            {

                mkvMerge = new MkvMerge(textBoxInputM2ts.Text, textBoxInputMkv.Text, textBoxInputChapters.Text, textBoxOutputMkv.Text, radioButtonUseM2tsAudio.Checked);
                mkvMerge.WorkerReportsProgress = true;
                mkvMerge.WorkerSupportsCancellation = true;
                mkvMerge.ProgressChanged += mkvMerge_ProgressChanged;
                mkvMerge.RunWorkerCompleted += mkvMerge_RunWorkerCompleted;
                mkvMerge.RunWorkerAsync();
            }

            mkvMerge_ProgressChanged(this, null);
        }

        private void mkvMerge_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // TODO: This throws a NPE if the window is closed while muxing is in progress
            statusStripProgressBar.Value = (int)(10 * mkvMerge.Progress);
            statusStripLabel.Text = mkvMerge.Progress.ToString("##0.0") + "%";

            if (mkvMerge.IsPaused)
                statusStripLabel.Text += " (paused)";
            if (mkvMerge.IsError)
                statusStripLabel.Text += " (error)";
            if (mkvMerge.IsCanceled)
                statusStripLabel.Text += " (canceled)";
            if (mkvMerge.IsCompleted)
                statusStripLabel.Text += " (completed)";
        }

        private void mkvMerge_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonRemux.Text = "Mux!";
            buttonClose.Text = "Close";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (mkvMerge != null && mkvMerge.IsBusy)
            {
                if (DialogResult.Yes != MessageBox.Show(this, "Are you sure you want to cancal the remux?", "Cancel remux?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;

                mkvMerge.Resume();
                mkvMerge.CancelAsync();
            }
            else
            {
                Close();
            }
        }

        private void FormRemux_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mkvMerge != null && mkvMerge.IsBusy)
            {
                if (DialogResult.Yes != MessageBox.Show(this, "Are you sure you want to cancal the remux?", "Cancel remux?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    e.Cancel = true;
                    return;
                }

                mkvMerge.Resume();
                mkvMerge.CancelAsync();
            }
        }
    }
}
