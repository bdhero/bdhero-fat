using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.tools;
using BDAutoMuxer.views;
using Newtonsoft.Json;

namespace BDAutoMuxer
{
    public partial class FormTrackNamer : Form
    {
        private BackgroundWorker _worker;

        private readonly List<MediaInfo> _mediaInfos = new List<MediaInfo>();

        private readonly Dictionary<MIFormat, string> _videoFormats = new Dictionary<MIFormat, string>();
        private readonly Dictionary<MIFormat, string> _audioFormats = new Dictionary<MIFormat, string>();

        private readonly Dictionary<MICodec, string> _videoCodecs = new Dictionary<MICodec, string>();
        private readonly Dictionary<MICodec, string> _audioCodecs = new Dictionary<MICodec, string>();

        private DateTime _startTime;
        private TimeSpan _runTime;

        private string[] _filePaths;

        public FormTrackNamer()
        {
            InitializeComponent();

            textBoxMkvInfoPath.Text = AbstractExternalTool.FindExe("mkvinfo.exe", "mmg.exe");
            textBoxMkvPropEditPath.Text = AbstractExternalTool.FindExe("mkvpropedit.exe", "mmg.exe");
            textBoxToolsDirPath.Text = BDAutoMuxerSettings.ConfigDir;

            FormUtils.TextBox_EnableSelectAll(this);
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            if (_worker == null)
                StartScan();
            else
                CancelScan();
        }

        private void StartScan()
        {
            _filePaths = textBoxInputFiles.Lines.Select(line => line.Trim()).Where(File.Exists).ToArray();

            textBoxInputFiles.Lines = _filePaths;

            var numFiles = _filePaths.Length;

            if (numFiles < 1)
                return;

            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar1.Maximum = numFiles;

            SetLabelText();

            buttonScan.Text = "Stop";

            _worker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            _worker.ProgressChanged += (o, args) =>
                                           {
                                               progressBar1.PerformStep();
                                               SetLabelText();
                                           };

            _worker.RunWorkerCompleted += (o, args) =>
                                              {
                                                  SetLabelText();
                                                  buttonScan.Enabled = true;
                                                  buttonScan.Text = "Scan";
                                                  _worker = null;
                                              };
            _worker.DoWork += (sender, args) => Scan(_filePaths);
            _worker.RunWorkerAsync();
        }

        private void SetLabelText()
        {
            var numFiles = _filePaths.Length;
            var percent = (100.0d * progressBar1.Value / numFiles);
            var scanResult = new ScanResult(
                new Formats(_videoFormats, _audioFormats),
                new Codecs(_videoCodecs, _audioCodecs)
            );
            var curFileNum = progressBar1.Value < numFiles ? progressBar1.Value + 1 : numFiles;
            labelCount.Text = string.Format("Scanning file {0} of {1}", curFileNum, numFiles);
            textBoxCurFile.Text = progressBar1.Value < numFiles ? _filePaths[progressBar1.Value] : null;
            labelPercent.Text = string.Format("{0}%", percent.ToString("0.00"));
            textBoxStdOut.Text = JsonConvert.SerializeObject(scanResult, Formatting.Indented);
        }

        private void CancelScan()
        {
            if (_worker != null)
            {
                _worker.CancelAsync();
                buttonScan.Enabled = false;
                buttonScan.Text = "Stopping...";
            }
        }

        private void Scan(IList<string> filePaths)
        {
            MediaInfo.Config.CLIPath = @"C:\Tools\MediaInfo\MediaInfo.exe";
            MediaInfo.Config.CSVPath = @"C:\Projects\bdautomuxer\BDAutoMuxer\Resources\MediaInfo_XML.csv";

            _mediaInfos.Clear();
            _videoFormats.Clear();
            _audioFormats.Clear();
            _videoCodecs.Clear();
            _audioCodecs.Clear();

            var startAll = DateTime.Now;

            for (var i = 0; i < filePaths.Count && !_worker.CancellationPending; i++)
            {
                var filePath = filePaths[i];
                var mediaInfo = new MediaInfo(filePath).Scan();

                _mediaInfos.Add(mediaInfo);

                foreach (var track in mediaInfo.VideoTracks)
                {
                    _videoFormats[track.Format] = track.FilePath;
                    _videoCodecs[track.Codec] = track.FilePath;
                }

                foreach (var track in mediaInfo.AudioTracks)
                {
                    _audioFormats[track.Format] = track.FilePath;
                    _audioCodecs[track.Codec] = track.FilePath;
                }
                
                _worker.ReportProgress((int)(100.0d * i / filePaths.Count));
            }

            _runTime = DateTime.Now - startAll;

            /*
            var miVideoTracks = _mediaInfos.SelectMany(mi => mi.VideoTracks).ToList();
            var miAudioTracks = _mediaInfos.SelectMany(mi => mi.AudioTracks).ToList();

            miVideoTracks.ForEach(track => _videoFormats[track.Format] = track.FilePath);
            miAudioTracks.ForEach(track => _audioFormats[track.Format] = track.FilePath);
            */
        }
    }

    #region JSON

    class Formats
    {
        public readonly Dictionary<MIFormat, string> video;
        public readonly Dictionary<MIFormat, string> audio;

        public Formats(Dictionary<MIFormat, string> video, Dictionary<MIFormat, string> audio)
        {
            this.video = video;
            this.audio = audio;
        }
    }

    class Codecs
    {
        public readonly Dictionary<MICodec, string> video;
        public readonly Dictionary<MICodec, string> audio;

        public Codecs(Dictionary<MICodec, string> video, Dictionary<MICodec, string> audio)
        {
            this.video = video;
            this.audio = audio;
        }
    }

    class ScanResult
    {
        public readonly Formats formats;
        public readonly Codecs codecs;

        public ScanResult(Formats formats, Codecs codecs)
        {
            this.formats = formats;
            this.codecs = codecs;
        }
    }

    #endregion

}
