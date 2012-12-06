using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.tools;
using BDAutoMuxer.views;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace BDAutoMuxer
{
    public partial class FormRemux : Form
    {
        private MkvMerge _mkvMerge;

        public FormRemux()
        {
            InitializeComponent();
            FormUtils.TextBox_EnableSelectAll(this);
        }

        ~FormRemux()
        {
            CancelRemux();
        }

        private void FormRemux_Load(object sender, EventArgs e)
        {
            progressLabel.Text = "";
            statusStripProgressBar.Visible = false;

            MinimumSize = new Size(0, Size.Height);
            MaximumSize = new Size(Screen.FromHandle(Handle).Bounds.Width, Size.Height);
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

        private static void BrowseFile(Control textBox, string title, string fileTypeName, string fileExt, bool checkFileExists = true)
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
            Remux();
        }

        private void mkvMerge_Started()
        {
            buttonRemux.Text = "Pause";
            buttonClose.Text = "Stop";
        }

        private CancelEventArgs PauseOrResume()
        {
            var e = new CancelEventArgs();

            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                mkvMerge_Started();

                if (_mkvMerge.IsPaused)
                {
                    _mkvMerge.Resume();
                }
                else
                {
                    _mkvMerge.Pause();
                    buttonRemux.Text = "Resume";
                }

                mkvMerge_ProgressChanged(this, null);

                e.Cancel = true;
            }

            return e;
        }

        private CancelEventArgs OverwriteExistingFile()
        {
            var e = new CancelEventArgs();

            if (File.Exists(textBoxOutputMkv.Text))
            {
                if (DialogResult.Yes != MessageBox.Show(this,
                    string.Format("Overwrite \"{0}\"?", textBoxOutputMkv.Text),
                    "File already exists",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                {
                    e.Cancel = true;
                }
            }

            return e;
        }

        private CancelEventArgs CheckInvalidFilePaths()
        {
            var e = new CancelEventArgs();

            if (!ValidFilePaths)
            {
                string msg = "Whoops!  The programmer made a boo-boo :-(";
                TextBox tb = null;

                if (!IsInputM2tsFileValid)
                {
                    msg = "Please select a valid input M2TS file.";
                    tb = textBoxInputM2ts;
                }
                else if (!IsInputMkvFileValid)
                {
                    msg = "Please select a valid input MKV file.";
                    tb = textBoxInputMkv;
                }
                else if (!IsInputChapterFileValid)
                {
                    msg = "Please select a valid input chapter file.";
                    tb = textBoxInputChapters;
                }
                else if (!IsOutputMkvFileValid)
                {
                    msg = "Please enter a valid output MKV file.";
                    tb = textBoxOutputMkv;
                }

                MessageBox.Show(this, msg, BDAutoMuxerSettings.AssemblyName + " Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (tb != null)
                {
                    tb.Focus();
                    tb.SelectAll();
                }

                e.Cancel = true;
            }

            return e;
        }

        private void Remux()
        {
            if (PauseOrResume().Cancel)
                return;

            if (CheckInvalidFilePaths().Cancel)
                return;

            if (OverwriteExistingFile().Cancel)
                return;

            statusStripProgressBar.Visible = true;

            mkvMerge_Started();

            _mkvMerge = new MkvMerge(textBoxInputM2ts.Text, textBoxInputMkv.Text, textBoxInputChapters.Text, textBoxOutputMkv.Text, radioButtonUseM2tsAudio.Checked)
                            {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            _mkvMerge.ProgressChanged += mkvMerge_ProgressChanged;
            _mkvMerge.RunWorkerCompleted += mkvMerge_RunWorkerCompleted;
            _mkvMerge.RunWorkerAsync();

            mkvMerge_ProgressChanged(this, null);
        }

        private bool ValidFilePaths
        {
            get
            {
                return
                    IsInputM2tsFileValid &&
                    IsInputMkvFileValid &&
                    IsInputChapterFileValid &&
                    IsOutputMkvFileValid;
            }
        }

        private bool IsInputM2tsFileValid
        {
            get { return File.Exists(textBoxInputM2ts.Text); }
        }

        private bool IsInputMkvFileValid
        {
            get { return File.Exists(textBoxInputMkv.Text); }
        }

        private bool IsInputChapterFileValid
        {
            // Allow chapter file to be optional, but if specified, it must be a valid file
            get { return File.Exists(textBoxInputChapters.Text) || string.IsNullOrEmpty(textBoxInputChapters.Text); }
        }

        private bool IsOutputMkvFileValid
        {
            get { return !string.IsNullOrEmpty(textBoxOutputMkv.Text); }
        }

        private void CancelRemux()
        {
            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                _mkvMerge.Resume();
                _mkvMerge.CancelAsync();
            }
        }

        private void mkvMerge_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_mkvMerge == null) return;

            var progress = (int)(10 * _mkvMerge.Progress);

            try
            {
                // TODO: This throws a NPE if the window is closed while muxing is in progress
                statusStripProgressBar.Value = progress;

                TaskbarProgress.SetProgressState(TaskbarProgressBarState.Normal, Handle);
                TaskbarProgress.SetProgressValue(progress, 1000, Handle);
            }
            catch
            {
            }

            statusStripLabel.Text = _mkvMerge.Progress.ToString("##0.0") + "%";

            if (_mkvMerge.IsPaused)
                statusStripLabel.Text += " (paused)";
            if (_mkvMerge.IsError)
                statusStripLabel.Text += " (error)";
            if (_mkvMerge.IsCanceled)
                statusStripLabel.Text += " (canceled)";
            if (_mkvMerge.IsSuccess)
                statusStripLabel.Text += " (completed)";
        }

        private void mkvMerge_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonRemux.Text = "Mux!";
            buttonClose.Text = "Close";

            TaskbarProgress.SetProgressState(TaskbarProgressBarState.NoProgress, Handle);

            statusStripProgressBar.Visible = false;

            if (_mkvMerge != null)
            {
                if (_mkvMerge.IsError)
                {
                    TaskbarProgress.SetProgressState(TaskbarProgressBarState.Error, Handle);
                    MessageBox.Show(this, _mkvMerge.ErrorMessage, "mkvmerge Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                if (DialogResult.Yes != MessageBox.Show(this, "Are you sure you want to cancal the remux?", "Cancel remux?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;

                CancelRemux();
            }
            else
            {
                Close();
            }
        }

        private void FormRemux_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_mkvMerge != null && _mkvMerge.IsBusy)
            {
                if (DialogResult.Yes != MessageBox.Show(this, "Are you sure you want to cancal the remux?", "Cancel remux?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    e.Cancel = true;
                    return;
                }

                CancelRemux();
            }
        }
    }
}
