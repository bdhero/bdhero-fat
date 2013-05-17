using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaInfoWrapper;

namespace BDHero
{
    public partial class FormFindIncompleteMedia : Form
    {
        private List<MediaInfo> _mediaInfos = new List<MediaInfo>(); 

        public FormFindIncompleteMedia()
        {
            InitializeComponent();
        }

        private bool EnableControls
        {
            set
            {
                textBoxSourceDir.Enabled = value;
                textBoxMissingChapters.Enabled = value;
                textBoxMissingSubtitles.Enabled = value;
                buttonScan.Enabled = value;
            }
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            string[] mkvFilePaths = Directory.GetFiles(textBoxSourceDir.Text, "*.mkv", SearchOption.AllDirectories);

            if (mkvFilePaths.Length == 0) return;

            _mediaInfos = mkvFilePaths.Select(mkvFilePath => new MediaInfo(mkvFilePath)).ToList();

            EnableControls = false;

            var bg = new Scanner();
            bg.ProgressChanged += BgOnProgressChanged;
            bg.RunWorkerCompleted += BgOnRunWorkerCompleted;
            bg.RunWorkerAsync(_mediaInfos);
        }

        private void BgOnProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            var curIndex = (double) args.ProgressPercentage + 1;
            var numFiles = (double) _mediaInfos.Count;
            var pct = 100 * curIndex / numFiles;
            if (curIndex > numFiles) curIndex = numFiles;
            var mediaInfo = _mediaInfos[(int) curIndex - 1];
            labelProgress.Text = string.Format("Scanning file {0} of {1} ({2}%): {3}", curIndex, numFiles, pct.ToString("0.00"), mediaInfo.File != null ? mediaInfo.File.Path : "...");
        }

        private void BgOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            EnableControls = true;
            labelProgress.Text = string.Format("Finished scanning {0} files", _mediaInfos.Count);

            textBoxMissingChapters.Lines = _mediaInfos.Where(info => info.File != null && !info.ChapterTracks.Any())
                                                      .Select(info => info.File.Path).ToArray();
            textBoxMissingSubtitles.Lines = _mediaInfos.Where(info => info.File != null && !info.SubtitleTracks.Any())
                                                       .Select(info => info.File.Path).ToArray();
        }
    }

    public class Scanner : BackgroundWorker
    {
        public Scanner()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += OnDoWork;
        }

        private void OnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var mediaInfos = doWorkEventArgs.Argument as List<MediaInfo>;
            for (var i = 0; i < mediaInfos.Count; i++)
            {
                ReportProgress(i);
                mediaInfos[i].Scan();
            }
            ReportProgress(mediaInfos.Count);
        }
    }
}
