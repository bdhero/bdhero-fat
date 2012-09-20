using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BDInfo.controllers
{
    class MkvMerge : AbstractExternalTool
    {
        private string exe_path;

        private string basePath;
        private string m2tsPath;
        private string chapterTxtPath;
        private string mkvPath;

        protected override string Name { get { return "MkvMerge"; } }
        protected override string Filename { get { return "mkvmerge.exe"; } }

        private BDROM BDROM;
        private ICollection<TSAudioStream> selectedAudioTracks;

        public MkvMerge(BDROM BDROM, string m2tsPath, ICollection<TSAudioStream> selectedAudioTracks)
            : base()
        {
            this.BDROM = BDROM;
            this.m2tsPath = m2tsPath;
            this.selectedAudioTracks = selectedAudioTracks;

            this.basePath = Path.Combine(Path.GetDirectoryName(m2tsPath), Path.GetFileNameWithoutExtension(m2tsPath));
            this.chapterTxtPath = basePath + ".chapters.txt";
            this.mkvPath = basePath + ".mkv";

            this.DoWork += Mux;
        }

        private void Mux(object sender, DoWorkEventArgs e)
        {
            //outputFilePath = e.Argument as string;

            ExtractResources();

            Execute(new List<string>() { "-o", mkvPath, "--chapters", chapterTxtPath, m2tsPath }, sender, e);
        }

        protected override void ExtractResources()
        {
            exe_path = this.ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string regex = @"^Progress: ([\d\.]+)\%";
            if (Regex.IsMatch(line, regex))
            {
                Match match = Regex.Match(line, regex);
                Double.TryParse(match.Groups[1].Value, out progress);
                worker.ReportProgress((int)progress);
            }
        }
    }
}
